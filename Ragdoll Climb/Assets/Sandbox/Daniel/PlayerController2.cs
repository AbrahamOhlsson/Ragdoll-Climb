using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] float pushForce = 100f;
    //[SerializeField] float pushForceMult = 2f;
    [SerializeField] float pullForce = 50f;

    [SerializeField] Rigidbody leftHand;
    [SerializeField] Rigidbody rightHand;
    [SerializeField] Rigidbody torso;
	[SerializeField] Rigidbody leftLeg;
	[SerializeField] Rigidbody rightLeg;
	[SerializeField] GameObject grabObjLeft;
    [SerializeField] GameObject grabObjRight;

    bool gripLeft = false;
    bool gripRight = false;

    float startPushForce;
    Vector3 pullDirLeft;
    Vector3 pullDirRight;

    Vector3 pushDirLeft;
    Vector3 pushDirRight;


    void Start ()
    {
        startPushForce = pushForce;
	}
	

	void Update ()
    {

        if (!gripLeft)
        {
            pushDirLeft = new Vector3(Input.GetAxis("XB-leftjoystickX"), -Input.GetAxis("XB-leftjoystickY"), 0f);

            pullDirLeft = Vector3.zero;
        }
        else
        {
            pullDirLeft = new Vector3(-Input.GetAxis("XB-leftjoystickX"), Mathf.Clamp(Input.GetAxis("XB-leftjoystickY"), 0f, 1f));

            pushDirLeft = Vector3.zero;
        }
        if (!gripRight)
        {
            pushDirRight = new Vector3(Input.GetAxis("XB-rightjoystickX"), -Input.GetAxis("XB-rightjoystickY"), 0f);

            pullDirRight = Vector3.zero;
        }
        else
        {
            pullDirRight = new Vector3(-Input.GetAxis("XB-rightjoystickX"), Mathf.Clamp(Input.GetAxis("XB-rightjoystickY"), 0f, 1f));

            pushDirRight = Vector3.zero;
        }
        

        if (Input.GetAxis("XB-leftTrigger") == 1 && !gripLeft)
        {
            grabObjLeft.SetActive(true);

            gripLeft = true;
        }
        else if (Input.GetAxis("XB-leftTrigger") == 0 && gripLeft)
        {
            grabObjLeft.SetActive(false);

            gripLeft = false;
        }

        if (Input.GetAxis("XB-rightTrigger") == 1 && !gripRight)
        {
            grabObjRight.SetActive(true);

            gripRight = true;
        }
        else if (Input.GetAxis("XB-rightTrigger") == 0 && gripRight)
        {
            grabObjRight.SetActive(false);

            gripRight = false;
        }

        //if (gripLeft || gripRight)
        //    pushForce = startPushForce * pushForceMult;
        //else
        //    pushForce = startPushForce;
    }


    private void FixedUpdate()
    {
        leftHand.AddForce(pushDirLeft * pushForce);
        rightHand.AddForce(pushDirRight * pushForce);
        torso.AddForce(pullDirLeft * pullForce);
        torso.AddForce(pullDirRight * pullForce);

		leftHand.position = new Vector3(leftHand.position.x, leftHand.position.y, 0); //limit z
		rightHand.position = new Vector3(rightHand.position.x, rightHand.position.y, 0); //limit z
		torso.position = new Vector3(torso.position.x, torso.position.y, 0); //limit z
		leftLeg.position = new Vector3(leftLeg.position.x, leftLeg.position.y, 0); //limit z
		rightLeg.position = new Vector3(rightLeg.position.x, rightLeg.position.y, 0); //limit z


	}
}
