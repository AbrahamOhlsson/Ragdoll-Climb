using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour
{
    public int playerNr = 1;
    public bool canMove = true;

    [Header("Forces and Movement")]
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
    [Tooltip("Force that will be applied to a throwable object after it is released.")]
    [Range(0f, 2000f)]
    [SerializeField] float throwForce = 500f;

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
    [SerializeField] float justGrabbed = 0.5f;
    [Range(0f, 10f)]
    [SerializeField] float losingGrip;
    [Range(0f, 10f)]
    [SerializeField] float lostGrip;

    //How much faster the player regain its stamina (Original value was 1.5)
    [Tooltip("How much faster the player regain its stamina.")]
    [Range(0f, 10f)]
    [SerializeField] float staminaRegen = 1.5f;

    // Determines how long the arms are out cold when extending stamina value.
    [Tooltip("Determines how long the arms are out cold when extending stamina value.")]
    [Range(0f, 10f)]
    [SerializeField] float armTimeOut = 1.45f;

    // Rigidbodies for bodyparts
    [Header("Rigidbodies")]
    [SerializeField] Rigidbody leftHand;
    [SerializeField] Rigidbody rightHand;
    [SerializeField] Rigidbody head;
    [SerializeField] Rigidbody leftShoulder;
    [SerializeField] Rigidbody rightShoulder;

    [Header("Particle Systems")]
    [SerializeField] ParticleSystem boostEffect;
    [SerializeField] ParticleSystem leftGoodClimbEffect;
    [SerializeField] ParticleSystem rightGoodClimbEffect;

    [Header("Stamina bars")]
    [SerializeField] Renderer leftStaminaBar;
    [SerializeField] Renderer rightStaminaBar;

    [Header("Audio clips")]
    [SerializeField] AudioClip goodClimbSfx;
    [SerializeField] AudioClip boostSfx;


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

    // How long the hands have gripped
    float leftGripTimer = 0f;
    float rightGripTimer = 0f;
    // How long the boost has been activated
    float boostTimer = 0f;

    //Vibration Timer
    float rightTimer;
    float leftTimer;

    //Timer if the arms are too tired to climb with
    float rightNumbArm = 0;
    float leftNumbArm = 0;

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

    CheckGrip checkGripLeft;
    CheckGrip checkGripRight;

    AudioSource source;


    void Start()
    {
        startPushForce = pushForce;
        startPullForce = pullForce;

        checkGripLeft = leftHand.GetComponent<CheckGrip>();
        checkGripRight = rightHand.GetComponent<CheckGrip>();

        source = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (canMove)
        {
            prevState = state;
            state = GamePad.GetState(playerIndex);

            // Left arm and joystick
            if (gripLeft)
            {
                if (checkGripLeft.currentGripping.tag == "Throwable")
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
            }
            // Right arm and joystick
            if (gripRight)
            {
                if (checkGripRight.currentGripping.tag == "Throwable")
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
            }

            // Left grip controls
            if ((state.Triggers.Left >= 0.8f || state.Buttons.LeftShoulder == ButtonState.Pressed) && !gripLeft && checkGripLeft.canGrip)
            {
                if (leftCanClimb == true)
                {
                    checkGripLeft.Connect();
                    gripLeft = true;

                    // Gets distance from the other hand
                    float handDist = leftHand.position.y - rightHand.position.y;

                    // If distance is above the required amount AND if the other arm has been gripped within the interval AND if the other hand is gripping && if this hand can activate boost
                    if (handDist >= reqHandHeightForBoost && rightGripTimer <= gripTimeframeForBoost && gripRight && leftBoostReady)
                    {
                        // A good climb has been performed
                        goodClimbs++;

                        // Plays particle effect and sound effect indicating a good climb
                        leftGoodClimbEffect.Play();
                        source.PlayOneShot(goodClimbSfx);

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
            else if (state.Triggers.Left == 0 && state.Buttons.LeftShoulder == ButtonState.Released && gripLeft)
            {
                if (checkGripLeft.currentGripable.tag == "Throwable")
                    ReleaseGrip(true, true);
                else
                    ReleaseGrip(true, false);
            }
            // Right grip controls
            if ((state.Triggers.Right >= 0.8f || state.Buttons.RightShoulder == ButtonState.Pressed) && !gripRight && checkGripRight.canGrip)
            {
                if (rightCanClimb == true)
                {
                    //grabObjRight.SetActive(true);
                    checkGripRight.Connect();

                    gripRight = true;

                    // Gets distance from the other hand
                    float handDist = rightHand.position.y - leftHand.position.y;

                    // If distance is above the required amount AND if the other arm has been gripped within the interval AND if the other hand is gripping && if this hand can activate boost
                    if (handDist >= reqHandHeightForBoost && leftGripTimer <= gripTimeframeForBoost && gripLeft && rightBoostReady)
                    {
                        // A good climb has been performed
                        goodClimbs++;

                        // Plays particle effect and sound effect indicating a good climb
                        rightGoodClimbEffect.Play();
                        source.PlayOneShot(goodClimbSfx);

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
            else if (state.Triggers.Right == 0 && state.Buttons.RightShoulder == ButtonState.Released && gripRight)
            {
                if (checkGripRight.currentGripable.tag == "Throwable")
                    ReleaseGrip(false, true);
                else
                    ReleaseGrip(false, false);
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

        //A timer when that counts how long the player is using the right hand. Hold too long and a vibration stars. Keep holding and you will fall.
        if (gripRight == true && !unlimitedStamina)
        {
            rightStaminaBar.enabled = true;

            rightTimer += Time.deltaTime;

            if (rightTimer < justGrabbed)
            {
                GamePad.SetVibration(playerIndex, 1f, 1f);
            }
            else
                GamePad.SetVibration(playerIndex, 0f, 0f);


            if (rightTimer >= losingGrip)
            {
                GamePad.SetVibration(playerIndex, 0f, 1f);
                rightStaminaBar.material.color = Color.red;
            }

            if (rightTimer >= lostGrip)
            {
                rightCanClimb = false;
                GamePad.SetVibration(playerIndex, 0f, 0f);

                ReleaseGrip(false, false);
            }

            rightStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(rightTimer / lostGrip, 0.01f, 1f));
        }

        if (gripRight == false)
        {
            GamePad.SetVibration(playerIndex, 0f, 0f);
            rightTimer -= Time.deltaTime * staminaRegen;
            rightTimer = Mathf.Clamp(rightTimer, 0f, lostGrip);
            rightStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(rightTimer / lostGrip, 0.01f, 1f));

            if (rightTimer <= losingGrip)
                rightStaminaBar.material.color = Color.green;
            if (rightTimer <= 0.01f)
                rightStaminaBar.enabled = false;
        }

        //A timer when that counts how long the player is using the left hand. Hold too long and a vibration stars. Keep holding and you will fall.
        if (gripLeft == true && !unlimitedStamina)
        {
            leftStaminaBar.enabled = true;

            leftTimer += Time.deltaTime;

            if (leftTimer < justGrabbed)
            {
                GamePad.SetVibration(playerIndex, 0.5f, 0.1f);
            }
            else
                GamePad.SetVibration(playerIndex, 0f, 0f);

            if (leftTimer >= losingGrip)
            {
                GamePad.SetVibration(playerIndex, 0.1f, 0f);
                leftStaminaBar.material.color = Color.red;
            }

            if (leftTimer > lostGrip)
            {
                GamePad.SetVibration(playerIndex, 0f, 0f);
                leftCanClimb = false;

                ReleaseGrip(true, false);
            }

            leftStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(leftTimer / lostGrip, 0.01f, 1f));
        }

        if (gripLeft == false)
        {
            GamePad.SetVibration(playerIndex, 0f, 0f);
            leftTimer -= Time.deltaTime * staminaRegen;
            leftTimer = Mathf.Clamp(leftTimer, 0f, lostGrip);
            leftStaminaBar.material.SetFloat("_Cutoff", Mathf.Clamp(leftTimer / lostGrip, 0.01f, 1f));

            if (leftTimer <= losingGrip)
                leftStaminaBar.material.color = Color.green;
            if (leftTimer <= 0.01f)
                leftStaminaBar.enabled = false;
            }
    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            // Add push force to hands
            leftHand.AddForce(pushDirLeft * pushForce);
            rightHand.AddForce(pushDirRight * pushForce);

            // Lerps hand positions to stableize into its proper position
            if (pushDirLeft != Vector3.zero)
                leftHand.position = Vector3.Lerp(leftHand.position, leftShoulder.position + pushDirLeft, handMoveSpeed);
            if (pushDirRight != Vector3.zero)
                rightHand.position = Vector3.Lerp(rightHand.position, rightShoulder.position + pushDirRight, handMoveSpeed);


            // Add pull force for torso
            head.AddForce(pullDirLeft * currentPullForceLeft);
            head.AddForce(pullDirRight * currentPullForceRight);

            // Adds equal pull force of grabbed object but in opposite direction
            if (gripLeft)
                checkGripLeft.currentGripping.AddForce(-pullDirLeft * currentPullForceLeft);
            if (gripRight)
                checkGripRight.currentGripping.AddForce(-pullDirRight * currentPullForceRight);
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
            leftHand.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

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
            rightHand.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

            // Resets pullDir
            pullDirRight = Vector3.zero;
        }
    }


    public void SetGamePad(int index)
    {
        playerIndex = (PlayerIndex)index;
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
        source.PlayOneShot(boostSfx);
        boostActive = true;

        return true;
    }


    public void ReleaseGrip(bool left, bool throwReleasedObj)
    {
        if (left)
        {
            // Disconnects from the grabbed object, also pushes it if it is a throwable
            if (throwReleasedObj)
                checkGripLeft.Disconnect(pushDirLeft, throwForce);
            else
                checkGripLeft.Disconnect();

            leftGripTimer = 0f;
            currentPullForceLeft = 0f;
            gripLeft = false;
        }
        else
        {
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
