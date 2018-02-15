using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour
{
    public int playerNr = 1;
    
    [Tooltip("For hand controls.")]
    [SerializeField] float pushForce = 100f;
    [Tooltip("For torso when pulling when gripped.")]
    [SerializeField] float pullForce = 50f;
    [Tooltip("How much pull and push force will be multiplied when the player climbs good.")]
    [SerializeField] float boostMult = 1.5f;
    [Tooltip("How high a hand must be above the other when gripping in order to get a speed boost.")]
    [SerializeField] float reqHandHeightForBoost = 1f;
    [Tooltip("The timeframe the player has to grip after the other hand has gripped to get speed boost.")]
    [SerializeField] float gripTimeframeForBoost = 0.75f;
    [Tooltip("How long the boost is active after performing a good climb.")]
    [SerializeField] float boostTime = 1f;
    [Tooltip("How many successful climbs need to be performed in a row to get boost.")]
    [SerializeField] int reqGoodClimbs = 3;
    [Tooltip("If boost can be activated continuously even if it already is activated.")]
    [SerializeField] bool continuousBoost = false;

    // Rigidbodies for bodyparts
    [SerializeField] Rigidbody leftHand;
    [SerializeField] Rigidbody rightHand;
    [SerializeField] Rigidbody head;

    // Grip objects on hands with joints
    [SerializeField] GameObject grabObjLeft;
    [SerializeField] GameObject grabObjRight;

    [SerializeField] ParticleSystem boostEffect;

    //Vibration Timer
    [SerializeField] float rightTimer;
    [SerializeField] float leftTimer;

    //Timers for vibrating states
    [SerializeField] float justGrabbed = 0.5f;
    [SerializeField] float losingGrip;
    [SerializeField] float lostGrip;

    // If hands are currently gripping
    bool gripLeft = false;
    bool gripRight = false;


    //Timer if the arms are too tired to climb with
    [SerializeField] float rightNumbArm = 0;
    [SerializeField] float leftNumbArm = 0;

    // If rewarding boost is active
    bool boostActive = false;
    // If the hand can trigger a boost
    bool leftBoostReady = false;
    bool rightBoostReady = false;

    //"Stamina bools". If set false, said hand wont be able to climb.
    bool rightCanClimb = true;
    bool leftCanClimb = true;

    // How many good climbs has been performed in a row
    int goodClimbs = 0;

    // The initial forces, used for resetting
    float startPushForce;
    float startPullForce;

    // How long the hands have gripped
    float leftGripTimer = 0f;
    float rightGripTimer = 0f;
    // How long the boost has been activated
    float boostTimer = 0f;

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


    void Start()
    {
        startPushForce = pushForce;
        startPullForce = pullForce;

        rightCanClimb = true;
        leftCanClimb = true;
    }


    void Update()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);

        // Left arm and joystick
        if (gripLeft)
        {
            // Gets joystick X- and Y-axis, clamps Y between 0 and 1
            pullDirLeft = new Vector3(Mathf.Clamp(-state.ThumbSticks.Left.X, -0.5f, 0.5f), Mathf.Clamp(-state.ThumbSticks.Left.Y, 0, 1f));

            // Counts time for how long this hand has gripped
            leftGripTimer += Time.deltaTime;

            // Resets pushDir
            pushDirLeft = Vector3.zero;
        }
        else
        {
            if(leftCanClimb == true)
            {
                // Gets direction of joystick axis
                pushDirLeft = new Vector3(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
            }
            else
            {
                gripLeft = false;
                leftNumbArm += Time.deltaTime;

                if(leftNumbArm >= 5)
                {
                    leftNumbArm = 0;
                    leftCanClimb = true;
                }
            }

            // Straightens wrist
            leftHand.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

            // Resets pullDir
            pullDirLeft = Vector3.zero;
        }
        // Right arm and joystick
        if (gripRight)
        {
            // Gets joystick X- and Y-axis, clamps Y between 0 and 1
             pullDirRight = new Vector3(Mathf.Clamp(-state.ThumbSticks.Right.X, -0.5f, 0.5f), Mathf.Clamp(-state.ThumbSticks.Right.Y, 0, 1f));

            // Counts time for how long this hand has gripped
            rightGripTimer += Time.deltaTime;

            // Resets pushDir
            pushDirRight = Vector3.zero;
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

                if(rightNumbArm >= 5)
                {
                    rightNumbArm = 0;
                    rightCanClimb = true;
                }
            }

            // Straightens wrist
            rightHand.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

            // Resets pullDir
            pullDirRight = Vector3.zero;
        }

        // Left grip controls
        if (state.Triggers.Left == 1 && !gripLeft)
        {
            if (leftCanClimb == true)
            {
                grabObjLeft.SetActive(true);
                gripLeft = true;

                // Gets distance from the other hand
                float handDist = leftHand.position.y - rightHand.position.y;

                // If distance is above the required amount AND if the other arm has been gripped within the interval AND if the other hand is gripping && if this hand can activate boost
                if (handDist >= reqHandHeightForBoost && rightGripTimer <= gripTimeframeForBoost && gripRight && leftBoostReady)
                {
                    // A good climb has been performed
                    goodClimbs++;

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
        else if (state.Triggers.Left == 0 && gripLeft)
        {
            grabObjLeft.SetActive(false);

            leftGripTimer = 0f;

            gripLeft = false;
        }
        // Right grip controls
        if (state.Triggers.Right == 1 && !gripRight)
        {
            if (rightCanClimb == true)
            {
                grabObjRight.SetActive(true);
                gripRight = true;

                // Gets distance from the other hand
                float handDist = rightHand.position.y - leftHand.position.y;

                // If distance is above the required amount AND if the other arm has been gripped within the interval AND if the other hand is gripping && if this hand can activate boost
                if (handDist >= reqHandHeightForBoost && leftGripTimer <= gripTimeframeForBoost && gripLeft && rightBoostReady)
                {
                    // A good climb has been performed
                    goodClimbs++;

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
        else if (state.Triggers.Right == 0 && gripRight)
        {
            grabObjRight.SetActive(false);

            rightGripTimer = 0f;

            gripRight = false;
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
        if (gripRight == true)
        {
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
            }

            if (rightTimer >= lostGrip)
            {
                rightCanClimb = false;
                GamePad.SetVibration(playerIndex, 0f, 0f);
                grabObjRight.SetActive(false);
                gripRight = false;
            }
        }

        if (gripRight == false)
        {
            GamePad.SetVibration(playerIndex, 0f, 0f);
            rightTimer -= Time.deltaTime;
            rightTimer = Mathf.Clamp(rightTimer, 0f, lostGrip);
        }

        //A timer when that counts how long the player is using the left hand. Hold too long and a vibration stars. Keep holding and you will fall.
        if (gripLeft == true)
        {
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
            }

            if (leftTimer > lostGrip)
            {
                GamePad.SetVibration(playerIndex, 0f, 0f);
                leftCanClimb = false;
                grabObjLeft.SetActive(false);
                gripLeft = false;
            }
        }

        if (gripLeft == false)
        {
            GamePad.SetVibration(playerIndex, 0f, 0f);
            leftTimer -= Time.deltaTime;
            leftTimer = Mathf.Clamp(leftTimer, 0f, lostGrip);
        }

        
    }


    private void FixedUpdate()
    {
        // Add push force to hands
        leftHand.AddForce(pushDirLeft * pushForce);
        rightHand.AddForce(pushDirRight * pushForce);

        // Add pull force for torso
        head.AddForce(pullDirLeft * pullForce);
        head.AddForce(pullDirRight * pullForce);
    }


    public void SetGamePad(int index)
    {
        playerIndex = (PlayerIndex)index;
    }


    void ActivateBoost()
    {
        // Aborts if the boos isn't continuous and if it already is activated
        if (!continuousBoost && boostActive)
            return;

        // Boosts forces
        pushForce = startPushForce * boostMult;
        pullForce = startPullForce * boostMult;

        boostTimer = 0f;
        boostEffect.Play();
        boostActive = true;
    }
}
