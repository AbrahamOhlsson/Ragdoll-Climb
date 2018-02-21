using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrip : MonoBehaviour
{
    public bool canGrip = true;
    public Rigidbody currentGripping;
    public Rigidbody currentGripable;

    [SerializeField] bool leftHand = true;

    List<Rigidbody> grabablesInReach = new List<Rigidbody>();

    
    PlayerController controller;


    void Start ()
    {
        controller = transform.root.GetComponent<PlayerController>();
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

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable")
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

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable")
        {
            grabablesInReach.Remove(other.GetComponent<Rigidbody>());
        }
    }


    private void DetermineObjectToGrab()
    {
        Rigidbody lastThrowable = new Rigidbody();
        Rigidbody lastOther = new Rigidbody();
        Rigidbody lastSlippery = new Rigidbody();
        Rigidbody lastWall = new Rigidbody();

        bool foundThrowable = false;
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
                else if (tag == "Grabable" || tag == "Player")
                {
                    lastOther = grabablesInReach[i];
                    foundOther = true;
                }
                else if (tag == "Throwable")
                {
                    lastThrowable = grabablesInReach[i];
                    foundThrowable = true;
                }
            }
            if (foundThrowable)
                currentGripable = lastThrowable;
            else if (foundOther)
                currentGripable = lastOther;
            else if (foundSlippery)
                currentGripable = lastSlippery;
            else if (foundWall)
                currentGripable = lastWall;

            canGrip = true;
        }
        else if (currentGripping == null)
        {
            canGrip = false;
            currentGripable = null;
        }
    }


    public void Connect()
    {
        if (canGrip)
        {
            gameObject.AddComponent<FixedJoint>().connectedBody = currentGripable;
            currentGripping = currentGripable;
        }
    }


    public void Disconnect()
    {
        Destroy(GetComponent<FixedJoint>());
        currentGripping = null;
    }
}
