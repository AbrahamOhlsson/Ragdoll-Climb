using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float pushForce = 100f;

    [SerializeField] Rigidbody leftHand;
    [SerializeField] Rigidbody rightHand;

    bool gripLeft = false;
    bool gripRight = false;
    bool canGripLeft = false;
    bool canGripRight = false;

    Vector3 pushDirLeft;
    Vector3 pushDirRight;


	void Start ()
    {
		
	}
	

	void Update ()
    {
        if (!gripLeft)
            pushDirLeft = new Vector3(Input.GetAxis("XB-leftjoystickX"), -Input.GetAxis("XB-leftjoystickY"), 0f);
        else
            pushDirLeft = Vector3.zero;
        if (!gripRight)
            pushDirRight = new Vector3(Input.GetAxis("XB-rightjoystickX"), -Input.GetAxis("XB-rightjoystickY"), 0f);
        else
            pushDirRight = Vector3.zero;
        
        
    }


    private void FixedUpdate()
    {
        leftHand.AddForce(pushDirLeft * pushForce);
        rightHand.AddForce(pushDirRight * pushForce);
    }
}
