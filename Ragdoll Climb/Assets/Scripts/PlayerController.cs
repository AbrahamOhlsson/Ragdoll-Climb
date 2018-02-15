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

    bool gripLeft = false;
    bool gripRight = false;

    bool boostActive = false;
    bool leftBoostReady = false;
    bool rightBoostReady = false;

    int goodClimbs = 0;

    float startPushForce;
    float startPullForce;

    float leftGripTimer_boost = 0f;
    float rightGripTimer_boost = 0f;
    float boostTimer = 0f;

    // Directions of pulling torso with hands
    Vector3 pullDirLeft;
    Vector3 pullDirRight;

    // Directions of pushing hands
    Vector3 pushDirLeft;
    Vector3 pushDirRight;

    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;


    void Start()
    {
        startPushForce = pushForce;
        startPullForce = pullForce;
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

            leftGripTimer_boost += Time.deltaTime;

            // Resets pushDir
            pushDirLeft = Vector3.zero;
        }
        else
        {
            // Gets direction of joystick axis
            pushDirLeft = new Vector3(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);

            leftHand.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

            // Resets pullDir
            pullDirLeft = Vector3.zero;
        }
        // Right arm and joystick
        if (gripRight)
        {
            // Gets joystick X- and Y-axis, clamps Y between 0 and 1
            pullDirRight = new Vector3(Mathf.Clamp(-state.ThumbSticks.Right.X, -0.5f, 0.5f), Mathf.Clamp(-state.ThumbSticks.Right.Y, 0, 1f));

            rightGripTimer_boost += Time.deltaTime;

            // Resets pushDir
            pushDirRight = Vector3.zero;
        }
        else
        {
            // Gets direction of joystick axis
            pushDirRight = new Vector3(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);

            rightHand.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

            // Resets pullDir
            pullDirRight = Vector3.zero;
        }

        // Left grip controls
        if (state.Triggers.Left == 1 && !gripLeft)
        {
            grabObjLeft.SetActive(true);

            gripLeft = true;

            float handDist = leftHand.position.y - rightHand.position.y;

            print("Hand distance: " + handDist);

            if (handDist >= reqHandHeightForBoost && rightGripTimer_boost <= gripTimeframeForBoost && gripRight && leftBoostReady)
            {
                ActivateBoost();
            }

            // If the left hand is above the right
            if (handDist > 0)
                // The right hand can now activate boost
                rightBoostReady = true;
            else
                // The right hand cannot activate boost, this prevents exploiting the boost
                rightBoostReady = false;
        }
        else if (state.Triggers.Left == 0 && gripLeft)
        {
            grabObjLeft.SetActive(false);

            leftGripTimer_boost = 0f;

            gripLeft = false;
        }
        // Right grip controls
        if (state.Triggers.Right == 1 && !gripRight)
        {
            grabObjRight.SetActive(true);

            gripRight = true;

            float handDist = rightHand.position.y - leftHand.position.y;

            if (handDist >= reqHandHeightForBoost && leftGripTimer_boost <= gripTimeframeForBoost && gripLeft && rightBoostReady)
            {
                ActivateBoost();
            }

            // If the right hand is above the left
            if (handDist > 0)
                // The left hand can now activate boosts
                leftBoostReady = true;
            else
                // The left hand cannot activate boost, this prevents exploiting the boost
                leftBoostReady = false;
        }
        else if (state.Triggers.Right == 0 && gripRight)
        {
            grabObjRight.SetActive(false);

            rightGripTimer_boost = 0f;

            gripRight = false;
        }

        // If boost is active
        if (boostActive)
        {
            boostTimer += Time.deltaTime;

            // Turns off boost if boost timer has reached its limit
            if (boostTimer >= boostTime)
            {
                pushForce = startPushForce;
                pullForce = startPullForce;

                boostEffect.Stop();
                boostActive = false;
            }
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
        if (!continuousBoost && boostActive)
            return;

        print("Activate");

        pushForce = startPushForce * boostMult;
        pullForce = startPullForce * boostMult;

        boostTimer = 0f;
        boostEffect.Play();
        boostActive = true;
    }
}
