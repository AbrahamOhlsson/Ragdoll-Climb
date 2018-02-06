using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    bool canGripLeft = false;
    bool released = true;
    
    FixedJoint joint;


	void Start ()
    {
		
	}
	

	void Update ()
    {
        if (Input.GetAxis("XB-leftTrigger") == 1 && canGripLeft && released)
        {
            joint.connectedBody = GetComponent<Rigidbody>();

            released = false;
        }

        if (Input.GetAxis("XB-leftTrigger") == 0 && !released)
        {
            joint.connectedBody = null;

            released = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ClimbObj")
        {
            joint = other.GetComponent<FixedJoint>();

            canGripLeft = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ClimbObj")
        {
            joint = null;

            canGripLeft = false;
        }
    }
}
