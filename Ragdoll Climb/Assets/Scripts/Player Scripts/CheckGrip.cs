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

    bool nearBottomObj = false;
    
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
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BottomObj")
        {
            nearBottomObj = true;

            controller.ReleaseGrip(leftHand, false);
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
            nearBottomObj = false;
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

            if (nearBottomObj)
                canGrip = false;
            else
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

            if (currentGripping != null && currentGripping.tag == "Player")
                currentGripping.transform.root.GetComponent<PlayerInfo>().AddGrabbingPlayer(transform.root.gameObject);
        }
    }


    public void Disconnect()
    {
        Destroy(GetComponent<FixedJoint>());

        if (currentGripping != null && currentGripping.tag == "Player")
            currentGripping.transform.root.GetComponent<PlayerInfo>().RemoveGrabbingPlayer(transform.root.gameObject);

        currentGripping = null;
    }

    public void Disconnect(Vector3 throwDir, float throwForce)
    {
        Destroy(GetComponent<FixedJoint>());

        currentGripping.AddForce(throwDir * throwForce);

        currentGripping = null;
    }
}
