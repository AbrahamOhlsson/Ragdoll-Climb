using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour
{
    public int playerNr = 1;
    
    // For hand controls
    [SerializeField] float pushForce = 100f;
    // For torso when pulling when gripped
    [SerializeField] float pullForce = 50f;

    // Rigidbodies for bodyparts
    [SerializeField] Rigidbody leftHand;
    [SerializeField] Rigidbody rightHand;
    [SerializeField] Rigidbody leftShoulder;
    [SerializeField] Rigidbody rightShoulder;

    // Grip objects on hands with joints
    [SerializeField] GameObject grabObjLeft;
    [SerializeField] GameObject grabObjRight;

    bool gripLeft = false;
    bool gripRight = false;

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
    }


    void Update()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);

        // Left arm and joystick
        if (gripLeft)
        {
            // Gets joystick X- and Y-axis, clamps Y between 0 and 1
            //pullDirLeft = new Vector3(-Input.GetAxis("XB-leftjoystickX_p" + playerNr), Mathf.Clamp(Input.GetAxis("XB-leftjoystickY_p" + playerNr), 0f, 1f));
            pullDirLeft = new Vector3(Mathf.Clamp(-state.ThumbSticks.Left.X, -0.5f, 0.5f), Mathf.Clamp(-state.ThumbSticks.Left.Y, 0, 1f));

            // Resets pushDir
            pushDirLeft = Vector3.zero;
        }
        else
        {
            // Gets direction of joystick axis
            //pushDirLeft = new Vector3(Input.GetAxis("XB-leftjoystickX_p" + playerNr), -Input.GetAxis("XB-leftjoystickY_p" + playerNr), 0f);
            pushDirLeft = new Vector3(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);

            leftHand.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

            // Resets pullDir
            pullDirLeft = Vector3.zero;
        }
        // Right arm and joystick
        if (gripRight)
        {
            // Gets joystick X- and Y-axis, clamps Y between 0 and 1
            //pullDirRight = new Vector3(-Input.GetAxis("XB-rightjoystickX_p" + playerNr), Mathf.Clamp(Input.GetAxis("XB-rightjoystickY_p" + playerNr), 0f, 1f));
            pullDirRight = new Vector3(Mathf.Clamp(-state.ThumbSticks.Right.X, -0.5f, 0.5f), Mathf.Clamp(-state.ThumbSticks.Right.Y, 0, 1f));

            // Resets pushDir
            pushDirRight = Vector3.zero;
        }
        else
        {
            // Gets direction of joystick axis
            //pushDirRight = new Vector3(Input.GetAxis("XB-rightjoystickX_p" + playerNr), -Input.GetAxis("XB-rightjoystickY_p" + playerNr), 0f);
            pushDirRight = new Vector3(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);

            rightHand.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);

            // Resets pullDir
            pullDirRight = Vector3.zero;
        }

        // Left grip controls
        if (/*Input.GetAxis("XB-leftTrigger_p" + playerNr)*/state.Triggers.Left == 1 && !gripLeft)
        {
            grabObjLeft.SetActive(true);

            gripLeft = true;
        }
        else if (/*Input.GetAxis("XB-leftTrigger_p" + playerNr)*/state.Triggers.Left == 0 && gripLeft)
        {
            grabObjLeft.SetActive(false);

            gripLeft = false;
        }
        // Right grip controls
        if (/*Input.GetAxis("XB-rightTrigger_p" + playerNr)*/state.Triggers.Right == 1 && !gripRight)
        {
            grabObjRight.SetActive(true);

            gripRight = true;
        }
        else if (/*Input.GetAxis("XB-rightTrigger_p" + playerNr)*/state.Triggers.Right == 0 && gripRight)
        {
            grabObjRight.SetActive(false);

            gripRight = false;
        }
    }


    private void FixedUpdate()
    {
        // Add push force to hands
        leftHand.AddForce(pushDirLeft * pushForce);
        rightHand.AddForce(pushDirRight * pushForce);

        // Add pull force for torso
        leftShoulder.AddForce(pullDirLeft * pullForce);
        rightShoulder.AddForce(pullDirRight * pullForce);
    }


    public void SetGamePad(int index)
    {
        playerIndex = (PlayerIndex)index;
    }
}
