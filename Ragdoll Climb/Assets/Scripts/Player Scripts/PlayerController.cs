using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour
{
    [Header("Forces and Movement")]
    public bool canMove = true;

    [Tooltip("For hand controls.")]
    [Range(0f, 300f)]
    [SerializeField] float pushForce = 100f;
    [Tooltip("How fast the hand gets to the proper position.")]
    [Range(0f, 1f)]
    [SerializeField] float handMoveSpeed = 0.2f;
    [Tooltip("For head when pulling when gripped.")]
    [Range(0f, 600f)]
    [SerializeField] float pullForce = 300;
    [Tooltip("How fast the pull force will reach it's value set above.")]
    [Range(0f, 1f)]
    [SerializeField] float pullForceGainSpeed = 0.3f;
    [Tooltip("Swing force in X-axis when grabbing with only one hand")]
    [Range(0f, 500f)]
    [SerializeField] float swingForceX_single = 150f;
    [Tooltip("Swing force in X-axis when grabbing with both hands")]
    [Range(0f, 500f)]
    [SerializeField] float swingForceX_double = 200f;
    [Tooltip("Swing force in Y-axis. This always pushes down when swinging.")]
    [Range(0f, 300f)]
    [SerializeField] float swingForceY = 35;
    [Tooltip("Force that will be applied to a throwable object after it is released.")]
    [Range(0f, 2000f)]
    [SerializeField] float throwForce = 500f;
    [Tooltip("The time it takes to release grip form sticky surfaces.")]
    [Range(0.1f, 5f)]
    [SerializeField] float stickyReleaseDelay = 1f;
    [Tooltip("The percentage of mass tht the arm limbs will have when they are controlled")]
    [Range(0f, 1f)]
    [SerializeField] float armMassDecrease = 0.4f;

    [Header("Punching")]
    //[Tooltip("The force that is applied to the arm to pull it back before the actual punch.")]
    //[SerializeField] float punchPullBackForce = 500;
    [Tooltip("The force that is applied to the arm to for punching.")]
    [SerializeField] float punchForce = 2000f;
    //[Tooltip("The time it takes for the punch force to be applied after the punch pull back force.")]
    //[SerializeField] float punchDelay = 0.1f;
    [Tooltip("The time it takes for the punch state to be reset after the punch force has been applied.")]
    [SerializeField] float punchStateResetDelay = 0.2f;
    [Tooltip("The percentage of stamina lost by a punch.")]
    [Range(0f, 1f)]
    [SerializeField] float punchStaminaCost = 0.1f;

    [Header("Boost")]
    [Tooltip("How much pull and push force will be multiplied when the player climbs good.")]
    [Range(1.01f, 10f)]
    [SerializeField] float boostMult = 1.5f;
    [Tooltip("How high a hand must be above the other when gripping in order to get a good climb.")]
    [Range(0.1f, 2f)]
    [SerializeField] float reqHandHeightForBoost = 1f;
    [Tooltip("The timeframe the player has to grip after the other hand has gripped to get a good climb.")]
    [Range(0.1f, 5f)]
    [SerializeField] float gripTimeframeForBoost = 0.75f;
    [Tooltip("How many successful climbs need to be performed in a row to get boost.")]
    [Range(1f, 10f)]
    [SerializeField] int reqGoodClimbs = 3;
    [Tooltip("How long the boost is active.")]
    [Range(0.1f, 5f)]
    [SerializeField] float boostTime = 1f;
    [Tooltip("If boost can be activated continuously even if it already is activated.")]
    [SerializeField] bool continuousBoost = false;
    
    [Header("Stamina")]
    //Timers for vibrating states
    [Range(0f, 5f)]
    [SerializeField] float justGrabbed = 0.25f;
    [Range(0f, 10f)]
    [SerializeField] float losingGrip = 3f;
    [Range(0f, 10f)]
    [SerializeField] float lostGrip = 5f;
    [Range(0f, 10f)]
    [SerializeField] float staminaDrainStart = 1f;
    [Range(0f, 1f)]
    [SerializeField] float staminaDrainReduce = 0.5f;

    //How much faster the player regain its stamina (Original value was 1.5)
    [Tooltip("How much faster the player regain its stamina.")]
    [Range(0f, 10f)]
    [SerializeField] float staminaRegen = 1.5f;

    // Determines how long the arms are out cold when extending stamina value.
    [Tooltip("Determines how long the arms are out cold when extending stamina value.")]
    [Range(0f, 10f)]
    [SerializeField] float armTimeOut = 1.45f;

	[SerializeField] GameObject respawnCounterObj;

    internal CheckGrip checkGripLeft;
    internal CheckGrip checkGripRight;

    // Rigidbodies for bodyparts
    Rigidbody leftHand;
    Rigidbody rightHand;
    Rigidbody leftShoulder;
    Rigidbody rightShoulder;
    Rigidbody leftElbow;
    Rigidbody rightElbow;
    Rigidbody head;
    Rigidbody root;
    Rigidbody spine;
    Rigidbody leftFoot;
    Rigidbody rightFoot;
    
    ParticleSystem boostEffect;
    ParticleSystem leftGoodClimbEffect;
    ParticleSystem rightGoodClimbEffect;
    
    Renderer leftStaminaBar;
    Renderer rightStaminaBar;

    // sound manager
    playerSound soundManager;

    // grunt Manager
    PlayerGrunts gruntManager;

    internal bool leftPunching = false;
    internal bool rightPunching = false;

    // If hands are currently gripping
    bool gripLeft = false;
    bool gripRight = false;
    
    // If rewarding boost is active
    bool boostActive = false;

    bool unlimitedStamina = false;

    // If the hand can trigger a boost
    bool leftBoostReady = false;
    bool rightBoostReady = false;

    //"Stamina bools". If set false, said hand wont be able to climb.
    bool rightCanClimb = true;
    bool leftCanClimb = true;
    
    // How many good climbs has been performed in a row
    int goodClimbs = 0;

    int invertedPull = -1;
    
    // The initial forces, used for resetting
    float startPushForce;
    float startPullForce;

    float currentPullForceLeft = 0;
    float currentPullForceRight = 0;
    float currentSwingForce = 0;

    // How long the hands have gripped
    float leftGripTimer = 0f;
    float rightGripTimer = 0f;

    // How long the boost has been activated
    float boostTimer = 0f;

    //Vibration Timer
    float rightTimer;
    float leftTimer;
    float leftVibrationAmount;
    float rightVibrationAmount;
    
    float staminaDrain = 1f;

    //Timer if the arms are too tired to climb with
    float rightNumbArm = 0;
    float leftNumbArm = 0;

	Quaternion wristStartRotLeft;
	Quaternion wristStartRotRight;

	// Directions of pulling torso with hands
	Vector3 pullDirLeft;
    Vector3 pullDirRight;

    // Directions of pushing hands
    Vector3 pushDirLeft;
    Vector3 pushDirRight;

    // Controller variables
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

	GameObject respawnCounter;
    
    PlayerInfo playerInfo;

    VibrationManager vibrator;

    List<Rigidbody> bodyParts = new List<Rigidbody>();
    
    IEnumerator releaseGripDelayedRight;
    IEnumerator releaseGripDelayedLeft;

    // Death ########
    float deathTimer;
    bool died = false;
    [Space]
	[SerializeField] float deathPressTime;

	void Start()
    {
        bodyParts.AddRange(GetComponentsInChildren<Rigidbody>());

        // Finds the important bodyparts
        leftHand = bodyParts.Find(x => (x.name.Contains("Wrist") || x.name.Contains("wrist")) && x.name.Contains("L"));
        rightHand = bodyParts.Find(x => (x.name.Contains("Wrist") || x.name.Contains("wrist")) && x.name.Contains("R"));
        leftShoulder = bodyParts.Find(x => (x.name.Contains("Shoulder") || x.name.Contains("shoulder")) && x.name.Contains("L"));
        rightShoulder = bodyParts.Find(x => (x.name.Contains("Shoulder") || x.name.Contains("shoulder")) && x.name.Contains("R"));
        leftElbow = bodyParts.Find(x => (x.name.Contains("Elbow") || x.name.Contains("elbow")) && x.name.Contains("L"));
        rightElbow = bodyParts.Find(x => (x.name.Contains("Elbow") || x.name.Contains("elbow")) && x.name.Contains("R"));
        leftFoot = bodyParts.Find(x => (x.name.Contains("Ankle") || x.name.Contains("ankle")) && x.name.Contains("L"));
        rightFoot = bodyParts.Find(x => (x.name.Contains("Ankle") || x.name.Contains("ankle")) && x.name.Contains("R"));
        head = bodyParts.Find(x => x.name.Contains("Head") || x.name.Contains("head"));
        root = bodyParts.Find(x => x.name.Contains("Root") || x.name.Contains("root"));
        spine = bodyParts.Find(x => x.name.Contains("Spine") || x.name.Contains("spine"));

        // Gets all objects with WorldSpaceUI to find stamina bars, this narrows the search
        List<WorldSpaceUI> uis = new List<WorldSpaceUI>();
        uis.AddRange(GetComponentsInChildren<WorldSpaceUI>());
        // Finds the stamina bars
        leftStaminaBar = uis.Find(x => x.name.Contains("Left Stamina Bar")).GetComponent<Renderer>();
        rightStaminaBar = uis.Find(x => x.name.Contains("Right Stamina Bar")).GetComponent<Renderer>();

        // Gets all particle systems and finds the ones we need
        List<ParticleSystem> partSys = new List<ParticleSystem>();
        partSys.AddRange(GetComponentsInChildren<ParticleSystem>());
        boostEffect = partSys.Find(x => x.name.Contains("Boost Effect"));
        leftGoodClimbEffect = partSys.Find(x => x.name.Contains("Left Good Climb Effect"));
        rightGoodClimbEffect = partSys.Find(x => x.name.Contains("Right Good Climb Effect"));

        startPushForce = pushForce;
        startPullForce = pullForce;

        checkGripLeft = leftHand.GetComponent<CheckGrip>();
        checkGripRight = rightHand.GetComponent<CheckGrip>();

        playerInfo = GetComponent<PlayerInfo>();

        vibrator = GetComponent<VibrationManager>();
        
        releaseGripDelayedLeft = ReleaseGripDelayed(true);
        releaseGripDelayedRight = ReleaseGripDelayed(false);

        leftHand.maxAngularVelocity = Mathf.Infinity;
        rightHand.maxAngularVelocity = Mathf.Infinity;
        root.maxAngularVelocity = Mathf.Infinity;
        leftShoulder.maxAngularVelocity = Mathf.Infinity;
        rightShoulder.maxAngularVelocity = Mathf.Infinity;

		deathTimer = -1000;

		wristStartRotLeft = leftHand.transform.localRotation;
		wristStartRotRight = rightHand.transform.localRotation;

        soundManager = GetComponent<playerSound>();

        gruntManager = transform.GetComponent<PlayerGrunts>();
    }


    void Update()
    {
        if (canMove && Time.timeScale > 0f)
        {
            prevState = state;
            state = GamePad.GetState(playerIndex);

            if (!checkGripLeft.onFire)
            {
                // Left arm and joystick
                if (gripLeft)
                {
                    if (checkGripLeft.currentGripping != null && checkGripLeft.currentGripping.tag == "Throwable")
                    {
                        ArmControl(true);
                    }
                    else
                    {
                        // Gets joystick X- and Y-axis, invertes if needed
                        pullDirLeft = new Vector3(-state.ThumbSticks.Left.X, -state.ThumbSticks.Left.Y) * invertedPull;

                        // Clamps pullDir so that X isn't too big and Y can only be above 0
                        pullDirLeft = new Vector3(Mathf.Clamp(pullDirLeft.x, -0.5f, 0.5f), Mathf.Clamp(pullDirLeft.y, 0f, 1f));

                        // Counts time for how long this hand has gripped
                        leftGripTimer += Time.deltaTime;

                        // Increases pull force over time
                        currentPullForceLeft = Mathf.Lerp(currentPullForceLeft, pullForce, pullForceGainSpeed);

                        // Resets pushDir
                        pushDirLeft = Vector3.zero;
                    }
                }
                else
                {
                    ArmControl(true);

                    // Punch
                    if (state.Buttons.LeftShoulder == ButtonState.Pressed && prevState.Buttons.LeftShoulder == ButtonState.Released && !leftPunching && leftCanClimb)
                        StartCoroutine(Punch(leftHand, pushDirLeft));
                }
            }
            if (!checkGripRight.onFire)
            {
                // Right arm and joystick
                if (gripRight)
                {
                    if (checkGripRight.currentGripping != null && checkGripRight.currentGripping.tag == "Throwable")
                    {
                        ArmControl(false);
                    }
                    else
                    {
                        // Gets joystick X- and Y-axis, invertes if needed
                        pullDirRight = new Vector3(-state.ThumbSticks.Right.X, -state.ThumbSticks.Right.Y) * invertedPull;

                        // Clamps pullDir so that X isn't too big and Y can only be above 0
                        pullDirRight = new Vector3(Mathf.Clamp(pullDirRight.x, -0.5f, 0.5f), Mathf.Clamp(pullDirRight.y, 0f, 1f));

                        // Counts time for how long this hand has gripped
                        rightGripTimer += Time.deltaTime;

                        // Increases pull force over time
                        currentPullForceRight = Mathf.Lerp(currentPullForceRight, pullForce, pullForceGainSpeed);

                        // Resets pushDir
                        pushDirRight = Vector3.zero;
                    }
                }
                else
                {
                    ArmControl(false);

                    // Punch
                    if (state.Buttons.RightShoulder == ButtonState.Pressed && prevState.Buttons.RightShoulder == ButtonState.Released && !rightPunching && rightCanClimb)
                        StartCoroutine(Punch(rightHand, pushDirRight));
                }
            }

            if (!checkGripLeft.onFire)
            {
                // Left grip controls
                if (state.Triggers.Left >= 0.8f /*&& prevState.Triggers.Left < 0.8f*/ && !gripLeft)
                {
                    StopCoroutine(releaseGripDelayedLeft);

                    if (leftCanClimb == true && !gripLeft && checkGripLeft.canGrip)
                    {
                        gripLeft = true;
                        checkGripLeft.Connect();

                        vibrator.VibrateTimed(0.3f, 0f, justGrabbed, 2);
                        gruntManager.PlayGrunt();

                        // Gets distance from the other hand
                        float handDist = leftHand.position.y - rightHand.position.y;

                        // If distance is above the required amount AND if the other arm has been gripped within the interval AND if the other hand is gripping && if this hand can activate boost
                        if (handDist >= reqHandHeightForBoost && rightGripTimer <= gripTimeframeForBoost && gripRight && leftBoostReady)
                        {
                            // A good climb has been performed
                            goodClimbs++;

                            // Plays particle effect and sound effect indicating a good climb
                            leftGoodClimbEffect.Play();
                            soundManager.PlaySound("goodClimb");

                            // Activates boost if the player has performed the required amounts of good climbs
                            if (goodClimbs >= reqGoodClimbs)
                                ActivateBoost();
                        }
                        else
                            goodClimbs = 0;

                        // If the left hand is above the right
                        if (handDist > 0)
                            // The right hand can now activate boost
                            rightBoostReady = true;
                        else
                            // The right hand cannot activate boost, this prevents exploiting the boost
                            rightBoostReady = false;
                    }
                }
                // If trigger is released
                else if (state.Triggers.Left == 0/* && prevState.Triggers.Left > 0*/ && gripLeft)
                {
                    if (checkGripLeft.currentGripping.tag == "Throwable")
                        ReleaseGrip(true, true);
                    else if (checkGripLeft.currentGripping.tag == "Sticky")
                    {
                        releaseGripDelayedLeft = ReleaseGripDelayed(true);
                        StartCoroutine(releaseGripDelayedLeft);
                    }
                    else
                        ReleaseGrip(true, false);

                    leftVibrationAmount = 0;
                }
            }
            if (!checkGripRight.onFire && canMove)
            {
                // Right grip controls
                if (state.Triggers.Right >= 0.8f /*&& prevState.Triggers.Right < 0.8f*/ && !gripRight)
                {
                    StopCoroutine(releaseGripDelayedRight);

                    if (rightCanClimb == true && !gripRight && checkGripRight.canGrip)
                    {
                        gripRight = true;
                        checkGripRight.Connect();

                        vibrator.VibrateTimed(0f, 0.2f, justGrabbed, 2);
                        gruntManager.PlayGrunt();

                        // Gets distance from the other hand
                        float handDist = rightHand.position.y - leftHand.position.y;

                        // If distance is above the required amount AND if the other arm has been gripped within the interval AND if the other hand is gripping && if this hand can activate boost
                        if (handDist >= reqHandHeightForBoost && leftGripTimer <= gripTimeframeForBoost && gripLeft && rightBoostReady)
                        {
                            // A good climb has been performed
                            goodClimbs++;

                            // Plays particle effect and sound effect indicating a good climb
                            rightGoodClimbEffect.Play();
                            soundManager.PlaySound("goodClimb");

                            // Activates boost if the player has performed the required amounts of good climbs
                            if (goodClimbs >= reqGoodClimbs)
                                ActivateBoost();
                        }
                        else
                            goodClimbs = 0;

                        // If the right hand is above the left
                        if (handDist > 0)
                            // The left hand can now activate boosts
                            leftBoostReady = true;
                        else
                            // The left hand cannot activate boost, this prevents exploiting the boost
                            leftBoostReady = false;
                    }
                }
                // If trigger is released
                else if (state.Triggers.Right == 0 /*&& (prevState.Triggers.Right > 0*/ && gripRight)
                {
                    if (checkGripRight.currentGripping.tag == "Throwable")
                        ReleaseGrip(false, true);
                    else if (checkGripRight.currentGripping.tag == "Sticky")
                    {
                        releaseGripDelayedRight = ReleaseGripDelayed(false);
                        StartCoroutine(releaseGripDelayedRight);
                    }
                    else
                        ReleaseGrip(false, false);

                    rightVibrationAmount = 0f;
                }
            }
        }

        // If boost is active
        if (boostActive)
        {
            boostTimer += Time.deltaTime;

            // Turns off boost if boost timer has reached its limit
            if (boostTimer >= boostTime)
            {
                // Resets forces
                pushForce = startPushForce;
                pullForce = startPullForce;

                boostEffect.Stop();
                boostActive = false;

                // Resets amount of good climbs if the boos isn't continuous
                if (!continuousBoost)
                    goodClimbs = 0;
            }
        }

        //A timer when that counts how long the player is using the right hand. Hold too long and a vibration starts. Keep holding and you will fall.
        if (gripRight == true && !unlimitedStamina && checkGripRight.currentGripping != null && checkGripRight.currentGripping.tag != "Throwable")
        {
            rightStaminaBar.gameObject.SetActive(true);

            rightTimer += staminaDrain * Time.deltaTime;

            if (rightTimer >= losingGrip)
            {
                rightVibrationAmount = (rightTimer - losingGrip) / (lostGrip - losingGrip);
                rightStaminaBar.material.color = new Color(Mathf.Clamp01(((rightTimer - losingGrip) / ((lostGrip - losingGrip) / 2))), Mathf.Clamp01((lostGrip - rightTimer) / (lostGrip - losingGrip) * 2), rightStaminaBar.material.color.b);
            }

            if (rightTimer >= lostGrip)
            {
                rightCanClimb = false;
                rightVibrationAmount = 0f;

                ReleaseGrip(false, false);
            }

            rightStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(rightTimer / lostGrip, 0.01f, 1f));
        }

        if (gripRight == false)
        {
            rightTimer -= Time.deltaTime * staminaRegen;
            rightTimer = Mathf.Clamp(rightTimer, 0f, lostGrip);
            rightStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(rightTimer / lostGrip, 0.01f, 1f));

            if (rightTimer >= losingGrip && rightCanClimb)
                rightStaminaBar.material.color = new Color(Mathf.Clamp01(((rightTimer - losingGrip) / ((lostGrip - losingGrip) / 2))), Mathf.Clamp01((lostGrip - rightTimer) / (lostGrip - losingGrip) * 2), rightStaminaBar.material.color.b);
            else if (rightTimer <= 0.01f)
                rightStaminaBar.gameObject.SetActive(false);
        }

        //A timer when that counts how long the player is using the left hand. Hold too long and a vibration stars. Keep holding and you will fall.
        if (gripLeft == true && !unlimitedStamina && checkGripLeft.currentGripping != null && checkGripLeft.currentGripping.tag != "Throwable")
        {
            leftStaminaBar.gameObject.SetActive(true);

            leftTimer += staminaDrain * Time.deltaTime;
            
            if (leftTimer >= losingGrip)
            {
                leftVibrationAmount = (leftTimer - losingGrip) / (lostGrip - losingGrip);
                leftStaminaBar.material.color = new Color(Mathf.Clamp01(((leftTimer - losingGrip) / ((lostGrip - losingGrip) / 2))), Mathf.Clamp01((lostGrip - leftTimer) / (lostGrip - losingGrip) * 2), leftStaminaBar.material.color.b);
            }

            if (leftTimer > lostGrip)
            {
                leftVibrationAmount = 0f;
                leftCanClimb = false;

                ReleaseGrip(true, false);
            }

            leftStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(leftTimer / lostGrip, 0.01f, 1f));
        }

        if (gripLeft == false)
        {
            leftTimer -= Time.deltaTime * staminaRegen;
            leftTimer = Mathf.Clamp(leftTimer, 0f, lostGrip);
            leftStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(leftTimer / lostGrip, 0.01f, 1f));

            if (leftTimer >= losingGrip && leftCanClimb)
                leftStaminaBar.material.color = new Color(Mathf.Clamp01(((leftTimer - losingGrip) / ((lostGrip - losingGrip) / 2))), Mathf.Clamp01((lostGrip - leftTimer) / (lostGrip - losingGrip) * 2), leftStaminaBar.material.color.b);
            else if (leftTimer <= 0.01f)
                leftStaminaBar.gameObject.SetActive(false);
        }

        // Stamina drain is lesser if both hands are gripped
        if (gripLeft && gripRight)
            staminaDrain = staminaDrainStart * staminaDrainReduce;
        else
            staminaDrain = staminaDrainStart;

        // Sets controller vibration
        vibrator.VibrationManual(leftVibrationAmount, rightVibrationAmount, 1);


        // DPad DEATH     ###############################################################################################################
        if (state.DPad.Down == ButtonState.Pressed && !died)
		{
			if (prevState.DPad.Down == ButtonState.Released)
			{
				respawnCounter = Instantiate(respawnCounterObj, root.position /*new Vector3(head.position.x , head.position.y , head.position.z)*/, Quaternion.Euler(0f, 0f, 0f));
				respawnCounter.GetComponent<Renderer>().material.SetTexture("_MainTex", Resources.Load<Sprite>("Sprites/respawn_3").texture);
				//respawnCounter.transform.localScale = (new Vector3(0.1f, 0.1f, 0.1f));
				respawnCounter.transform.parent = head.transform;
			}

			if(deathTimer >1 && deathTimer<1.1f && respawnCounter!=null)
			{
				respawnCounter.GetComponent<Renderer>().material.SetTexture("_MainTex", Resources.Load<Sprite>("Sprites/respawn_2").texture);
			}

			if (deathTimer > 2 && deathTimer < 2.1f && respawnCounter != null)
			{
				respawnCounter.GetComponent<Renderer>().material.SetTexture("_MainTex", Resources.Load<Sprite>("Sprites/respawn_1").texture);
			}

			if(deathTimer > deathPressTime)
			{
				deathTimer = 0;
				Destroy(respawnCounter);
                died = true;
				GetComponent<DeathManager>().Death();
				GetComponent<FreezePD>().StartFreeze(1f, true);
			}

			deathTimer += Time.deltaTime;
		}


		if (state.DPad.Down == ButtonState.Released && prevState.DPad.Down == ButtonState.Pressed)
		{
            if (deathTimer != 0)
			{
				//respawnCounter.GetComponent<Renderer>().material.SetTexture("_MainTex", Resources.Load<Sprite>("Sprites/respawn_3").texture);
				deathTimer = 0;
				Destroy(respawnCounter);
			}

            died = false;
        }
	}


    private void FixedUpdate()
    {
        if (canMove)
        {
            // Add push force to hands
            leftHand.AddForce(pushDirLeft * pushForce);
            rightHand.AddForce(pushDirRight * pushForce);

            // Add pull force for torso
            head.AddForce(0f, pullDirLeft.y * currentPullForceLeft, 0f);
            head.AddForce(0f, pullDirRight.y * currentPullForceRight, 0f);

            // Swinging
            if (gripRight && gripLeft)
                currentSwingForce = swingForceX_double / 2;
            else
                currentSwingForce = swingForceX_single;

            leftFoot.AddForce(pullDirLeft.x * currentSwingForce, -Mathf.Abs(pullDirLeft.x) * swingForceY, 0f);
            leftFoot.AddForce(pullDirRight.x * currentSwingForce, -Mathf.Abs(pullDirRight.x) * swingForceY, 0f);
            rightFoot.AddForce(pullDirLeft.x * currentSwingForce, -Mathf.Abs(pullDirLeft.x) * swingForceY, 0f);
            rightFoot.AddForce(pullDirRight.x * currentSwingForce, -Mathf.Abs(pullDirRight.x) * swingForceY, 0f);

            // Adds equal pull force of grabbed object but in opposite direction
            if (checkGripLeft.currentGripping != null && gripLeft)
                checkGripLeft.currentGripping.AddForce(0f, -pullDirLeft.y * currentPullForceLeft, 0f);
            if (checkGripRight.currentGripping != null && gripRight)
                checkGripRight.currentGripping.AddForce(0f, -pullDirRight.y * currentPullForceRight, 0f);

            float totalMassLoss = 0;

            // If the left hand is being moved
            if (pushDirLeft != Vector3.zero)
            {
                // Lerps hand position to stableize into its proper position
                leftHand.position = Vector3.Lerp(leftHand.position, leftShoulder.position + pushDirLeft, handMoveSpeed);
                
                // Lowers mass of arms so they are easier to move
                leftHand.mass = playerInfo.targetMasses[bodyParts.IndexOf(leftHand)] * armMassDecrease;
                leftElbow.mass = playerInfo.targetMasses[bodyParts.IndexOf(leftElbow)] * armMassDecrease;
                leftShoulder.mass = playerInfo.targetMasses[bodyParts.IndexOf(leftShoulder)] * armMassDecrease;

                // Counts the total loss of mass of the hand so it can be appliesd to other body parts
                totalMassLoss += playerInfo.targetMasses[bodyParts.IndexOf(leftHand)] - playerInfo.targetMasses[bodyParts.IndexOf(leftHand)] * armMassDecrease;
                totalMassLoss += playerInfo.targetMasses[bodyParts.IndexOf(leftElbow)] - playerInfo.targetMasses[bodyParts.IndexOf(leftElbow)] * armMassDecrease;
                totalMassLoss += playerInfo.targetMasses[bodyParts.IndexOf(leftShoulder)] - playerInfo.targetMasses[bodyParts.IndexOf(leftShoulder)] * armMassDecrease;
            }
            else
            {
                // Resets masses to the masses they should be
                leftHand.mass = playerInfo.targetMasses[bodyParts.IndexOf(leftHand)];
                leftElbow.mass = playerInfo.targetMasses[bodyParts.IndexOf(leftElbow)];
                leftShoulder.mass = playerInfo.targetMasses[bodyParts.IndexOf(leftShoulder)];
            }
            // If the right hand is being moved
            if (pushDirRight != Vector3.zero)
            {
                // Lerps hand position to stableize into its proper position
                rightHand.position = Vector3.Lerp(rightHand.position, rightShoulder.position + pushDirRight, handMoveSpeed);

                // Lowers mass of arms so they are easier to move
                rightHand.mass = playerInfo.targetMasses[bodyParts.IndexOf(rightHand)] * armMassDecrease;
                rightElbow.mass = playerInfo.targetMasses[bodyParts.IndexOf(rightElbow)] * armMassDecrease;
                rightShoulder.mass = playerInfo.targetMasses[bodyParts.IndexOf(rightShoulder)] * armMassDecrease;

                // Counts the total loss of mass of the hand so it can be appliesd to other body parts
                totalMassLoss += playerInfo.targetMasses[bodyParts.IndexOf(rightHand)] - playerInfo.targetMasses[bodyParts.IndexOf(rightHand)] * armMassDecrease;
                totalMassLoss += playerInfo.targetMasses[bodyParts.IndexOf(rightElbow)] - playerInfo.targetMasses[bodyParts.IndexOf(rightElbow)] * armMassDecrease;
                totalMassLoss += playerInfo.targetMasses[bodyParts.IndexOf(rightShoulder)] - playerInfo.targetMasses[bodyParts.IndexOf(rightShoulder)] * armMassDecrease;
            }
            else
            {
                // Resets masses to the masses they should be
                rightHand.mass = playerInfo.targetMasses[bodyParts.IndexOf(rightHand)];
                rightElbow.mass = playerInfo.targetMasses[bodyParts.IndexOf(rightElbow)];
                rightShoulder.mass = playerInfo.targetMasses[bodyParts.IndexOf(rightShoulder)];
            }

            // The loss of mass of the hands is applied equally between root, spine and head
            root.mass = playerInfo.targetMasses[bodyParts.IndexOf(root)] + totalMassLoss / 3;
            spine.mass = playerInfo.targetMasses[bodyParts.IndexOf(spine)] + totalMassLoss / 3;
            head.mass = playerInfo.targetMasses[bodyParts.IndexOf(spine)] + totalMassLoss / 3;
        }

        // Stableizes z position
        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].velocity = new Vector3(bodyParts[i].velocity.x, bodyParts[i].velocity.y, 0f);
        }
    }


    private void ArmControl(bool left)
    {
        if (left)
        {
            if (leftCanClimb == true)
            {
                // Gets direction of joystick axis
                pushDirLeft = new Vector3(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
            }
            else
            {
                gripLeft = false;
                leftNumbArm += Time.deltaTime;

                if (leftNumbArm >= armTimeOut)
                {
                    leftNumbArm = 0;
                    leftCanClimb = true;
                    leftStaminaBar.material.color = Color.green;
                }
            }

            // Straightens wrist
            leftHand.transform.localRotation = wristStartRotLeft;

            // Resets pullDir
            pullDirLeft = Vector3.zero;
        }
        else
        {
            if (rightCanClimb == true)
            {
                // Gets direction of joystick axis
                pushDirRight = new Vector3(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
            }
            else
            {
                gripRight = false;
                rightNumbArm += Time.deltaTime;

                if (rightNumbArm >= armTimeOut)
                {
                    rightNumbArm = 0;
                    rightCanClimb = true;
                    rightStaminaBar.material.color = Color.green;
                }
            }

            // Straightens wrist
            rightHand.transform.localRotation = wristStartRotRight;

            // Resets pullDir
            pullDirRight = Vector3.zero;
        }
    }
    

    IEnumerator Punch(Rigidbody hand, Vector2 direction)
    {
        if (hand == leftHand)
        {
            leftPunching = true;

            // Stamina stuff
            if (!unlimitedStamina)
            {
                leftStaminaBar.gameObject.SetActive(true);
                leftTimer += lostGrip * punchStaminaCost;
                if (leftTimer >= losingGrip)
                {
                    leftStaminaBar.material.color = new Color(Mathf.Clamp01(((leftTimer - losingGrip) / ((lostGrip - losingGrip) / 2))), Mathf.Clamp01((lostGrip - leftTimer) / (lostGrip - losingGrip) * 2), leftStaminaBar.material.color.b);
                }
                if (leftTimer > lostGrip)
                {
                    leftCanClimb = false;
                }
                leftStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(leftTimer / lostGrip, 0.01f, 1f));
            }

            // Makes sure the hand wont continue to stretch out
            pushDirLeft = Vector3.zero;

            vibrator.VibrateTimed(0.2f, 0f, 0.2f, 3);
        }
        else
        {
            rightPunching = true;

            // Stamina stuff
            if (!unlimitedStamina)
            {
                rightStaminaBar.gameObject.SetActive(true);
                rightTimer += lostGrip * punchStaminaCost;
                if (rightTimer >= losingGrip)
                {
                    rightStaminaBar.material.color = new Color(Mathf.Clamp01(((rightTimer - losingGrip) / ((lostGrip - losingGrip) / 2))), Mathf.Clamp01((lostGrip - rightTimer) / (lostGrip - losingGrip) * 2), rightStaminaBar.material.color.b);
                }
                if (rightTimer > lostGrip)
                {
                    rightCanClimb = false;
                }
                rightStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(rightTimer / lostGrip, 0.01f, 1f));
            }

            // Makes sure the hand wont continue to stretch out
            pushDirRight = Vector3.zero;

            vibrator.VibrateTimed(0f, 0.2f, 0.2f, 3);
        }

        hand.GetComponent<TrailRenderer>().enabled = true;

        int swooshIndex = Random.Range(1, 6);
        soundManager.PlaySound("PunchSwoosh" + swooshIndex);

        yield return new WaitForFixedUpdate();
        // Pulls back arm before punch
        //hand.AddForce(-direction * punchPullBackForce);

        //yield return new WaitForSeconds(punchDelay);
        yield return new WaitForFixedUpdate();
        // Pushes hand out to punch
        hand.AddForce(direction * punchForce);

        yield return new WaitForSeconds(punchStateResetDelay);
        // The punch is done
        if (hand == leftHand)
            leftPunching = false;
        else
            rightPunching = false;

        hand.GetComponent<TrailRenderer>().enabled = false;
        
        yield return null;
    }


    public bool ActivateBoost()
    {
        // Aborts if the boos isn't continuous and if it already is activated
        if (!continuousBoost && boostActive)
            return false;

        // Boosts forces
        pushForce = startPushForce * boostMult;
        pullForce = startPullForce * boostMult;

        boostTimer = 0f;
        boostEffect.Play();
        
        soundManager.PlaySound("comboBoost");
        boostActive = true;

        return true;
    }


    public void ReleaseGrip(bool left, bool throwReleasedObj)
    {
        if (left && gripLeft && checkGripLeft != null)
        {
            if (checkGripLeft.currentGripping != null && checkGripLeft.currentGripping.tag == "Sticky")
                soundManager.PlaySoundRandPitch("Slime1");

            // Disconnects from the grabbed object, also pushes it if it is a throwable
            if (throwReleasedObj)
                checkGripLeft.Disconnect(pushDirLeft, throwForce);
            else
                checkGripLeft.Disconnect();

            leftGripTimer = 0f;
            currentPullForceLeft = 0f;
            gripLeft = false;
        }
        else if (!left && gripRight && checkGripRight != null)
        {
            if (checkGripRight.currentGripping != null && checkGripRight.currentGripping.tag == "Sticky")
                soundManager.PlaySoundRandPitch("Slime1");

            // Disconnects from the grabbed object, also pushes it if it is a throwable
            if (throwReleasedObj)
                checkGripRight.Disconnect(pushDirRight, throwForce);
            else
                checkGripRight.Disconnect();

            rightGripTimer = 0f;
            currentPullForceRight = 0f;
            gripRight = false;
        }
    }


    public IEnumerator ReleaseGripDelayed(bool left)
    {
        yield return new WaitForSeconds(stickyReleaseDelay);
        
        ReleaseGrip(left, false);
        
        //soundManager.PlaySoundRandPitch("Slime1");

        yield return null;
    }


    public void SetGamePad(PlayerIndex index)
    {
        playerIndex = index;
    }
    
    
    // Inverts pull controls
    public void ToggleInvertPull()
    {
        invertedPull *= -1;
    }


    public void ToggleUnlimitedStamina()
    {
        if (unlimitedStamina)
        {
            unlimitedStamina = false;
        }
        else
        {
            leftTimer = 0;
            rightTimer = 0;

            unlimitedStamina = true;
        }
    }
}
