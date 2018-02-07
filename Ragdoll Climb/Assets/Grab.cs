using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    bool canGripLeft = false;
    bool releasedLeft = true;
    bool releasedRight = true;

    [SerializeField] GameObject grabObjLeft;
    [SerializeField] GameObject grabObjRight;


    void Start ()
    {
		
	}
	

	void Update ()
    {
        print("Left = " + Input.GetAxis("XB-leftTrigger"));
        print("Right = " + Input.GetAxis("XB-rightTrigger"));

        if (Input.GetAxis("XB-leftTrigger") == 1 && releasedLeft)
        {
            grabObjLeft.SetActive(true);

            releasedLeft = false;
        }

        if (Input.GetAxis("XB-leftTrigger") == 0 && !releasedLeft)
        {
            grabObjLeft.SetActive(false);

            releasedLeft = true;
        }

        if (Input.GetAxis("XB-rightTrigger") == 1 && releasedRight)
        {
            grabObjRight.SetActive(true);

            releasedRight = false;
        }

        if (Input.GetAxis("XB-rightTrigger") == 0 && !releasedRight)
        {
            grabObjRight.SetActive(false);

            releasedRight = true;
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "ClimbObj")
    //    {
    //        joint = other.GetComponent<FixedJoint>();

    //        canGripLeft = true;
    //    }
    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "ClimbObj")
    //    {
    //        joint = null;

    //        canGripLeft = false;
    //    }
    //}
}
