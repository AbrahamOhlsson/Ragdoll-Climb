using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrip : MonoBehaviour
{
    public bool canGrip = true;

    [SerializeField] bool leftHand = true;

    List<Rigidbody> grabablesInReach = new List<Rigidbody>();

    Rigidbody currentGripable;

    FixedJoint joint;

    PlayerController controller;


    void Start ()
    {
        controller = transform.root.GetComponent<PlayerController>();
        joint = GetComponent<FixedJoint>();
	}
	

	void Update ()
    {
        DetermineObjectToGrab();
	}


    private void OnTriggerStay(Collider other)
    {
        //if (other.tag == "Player" || other.tag == "Grabable")
        //{
        //    currentGripable = other.GetComponent<Rigidbody>();
        //}
        //else if (other.tag == "Slippery")
        //{
        //    currentGripable = other.GetComponent<Rigidbody>();
        //}
        //else if (other.tag == "Wall")
        //{
        //    currentGripable = other.GetComponent<Rigidbody>();
        //}
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BottomObj")
        {
            canGrip = false;

            controller.ReleaseGrip(leftHand);
        }

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall")
        {
            grabablesInReach.Add(other.GetComponent<Rigidbody>());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BottomObj")
        {
            canGrip = true;
        }

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall")
        {
            grabablesInReach.Remove(other.GetComponent<Rigidbody>());
        }
    }


    private void DetermineObjectToGrab()
    {
        Rigidbody lastOther = new Rigidbody();
        Rigidbody lastSlippery = new Rigidbody();
        Rigidbody lastWall = new Rigidbody();

        bool foundOther = false;
        bool foundSlippery = false;
        bool foundWall = false;

        if (grabablesInReach.Count > 0)
        {
            for (int i = 0; i < grabablesInReach.Count; i++)
            {
                string tag = grabablesInReach[i].tag;

                if (tag == "Wall")
                {
                    lastWall = grabablesInReach[i];
                    foundWall = true;
                }
                else if (tag == "Slippery")
                {
                    lastSlippery = grabablesInReach[i];
                    foundSlippery = true;
                }
                else if (tag == "Other")
                {
                    lastOther = grabablesInReach[i];
                    foundOther = true;
                }
            }

            if (foundOther)
                currentGripable = lastOther;
            else if (foundSlippery)
                currentGripable = lastSlippery;
            else if (foundWall)
                currentGripable = lastWall;

            canGrip = true;
        }
        else
        {
            canGrip = false;

            GetComponent<FixedJoint>().connectedBody = null;
        }
    }


    public void Connect()
    {
        if (canGrip)
            joint.connectedBody = currentGripable;
    }


    public void Disconnect()
    {
        joint.connectedBody = null;
    }
}
