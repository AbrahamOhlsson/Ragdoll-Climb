using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrip : MonoBehaviour
{
    public GameObject SlipperyCube;

    public bool canGrip = true;
    // The rigidbody that is being gripped right now
    public Rigidbody currentGripping;
    // The rigidbody that will be gripped
    public Rigidbody currentGripable;

    [SerializeField] bool leftHand = true;

    [SerializeField] Transform grabIndicators;

    bool nearBottomObj = false;
    bool playingAnim = false;

    Animator[] grabAnimators;

    List<Rigidbody> grabablesInReach = new List<Rigidbody>();


    SlipperySurface PlayerSlippery;
    PlayerController controller;


    void Start ()
    {
        controller = transform.root.GetComponent<PlayerController>();

        //Finding the player
        PlayerSlippery = transform.root.gameObject.GetComponent<SlipperySurface>();
        grabAnimators = grabIndicators.GetComponentsInChildren<Animator>();
    }
	

	void Update ()
    {
        DetermineObjectToGrab();
	}
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BottomObj")
        {
            nearBottomObj = true;

            controller.ReleaseGrip(leftHand, false);
        }

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable" || other.tag == "Electric")
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

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable" || other.tag == "Electric")
        {
            grabablesInReach.Remove(other.GetComponent<Rigidbody>());
        }

        if (other.tag == "Slippery" && currentGripping != null && currentGripping.tag == "Slippery")
        {
            controller.ReleaseGrip(leftHand, false);
        }
    }


    // This will determine what to grab if there is multiple grabable objects in reach.
    // If there is multiple objects of the same type in reach, the last one that became in reach will be selected.
    private void DetermineObjectToGrab()
    {
        Rigidbody lastThrowable = new Rigidbody();
        Rigidbody lastOther = new Rigidbody();
        Rigidbody lastSlippery = new Rigidbody();
        Rigidbody lastWall = new Rigidbody();
        Rigidbody lastElectric = new Rigidbody();
        
        bool foundThrowable = false;
        bool foundOther = false;
        bool foundSlippery = false;
        bool foundWall = false;
        bool Electric = false;

        // If there is any grabables
        if (grabablesInReach.Count > 0)
        {
            for (int i = 0; i < grabablesInReach.Count; i++)
            {
                string tag = grabablesInReach[i].tag;

                // If a normal wall is found
                if (tag == "Wall")
                {
                    lastWall = grabablesInReach[i];
                    foundWall = true;
                }
                // If Electric is found
                else if (tag == "Electric")
                {
                    lastElectric = grabablesInReach[i];
                    Electric = true;
                }
                // If a slippery wall is found
                else if (tag == "Slippery")
                {
                    foundSlippery = true;
                }
                // If a non-special object is found is found
                else if (tag == "Grabable" || tag == "Player")
                {
                    lastOther = grabablesInReach[i];
                    foundOther = true;
                }
                // If a throwable is found
                else if (tag == "Throwable")
                {
                    lastThrowable = grabablesInReach[i];
                    foundThrowable = true;
                }
               
            }

            // The type that is first checked will get first priority.
            // The type that is checked last will get least priority.
            if (foundThrowable)
            {
                currentGripable = lastThrowable;
                PlayGrabableAnim();
            }
            else if (foundOther)
            {
                currentGripable = lastOther;
                PlayGrabableAnim();
            }
            else if (Electric)
            {
                currentGripable = lastElectric;

                if (lastElectric == null)
                    StopAnim();
            }
            else if (foundSlippery)
            {
                currentGripable = SlipperyCube.GetComponent<Rigidbody>();

                if (currentGripping == null)
                    StopAnim();
            }
            else if (foundWall)
            {
                currentGripable = lastWall;

                if (currentGripping == null)
                    StopAnim();
            }
            

            // You cant grip anything if the bottom object is in reach.
            // This prevent the player from holding while that object goes throught the player
            if (nearBottomObj)
                canGrip = false;
            else
                canGrip = true;
        }
        // If nothing is no longer gripped
        else if (currentGripping == null)
        {
            canGrip = false;
            currentGripable = null;
        }
    }


    private void PlayGrabableAnim()
    {
        if (!playingAnim && currentGripping == null)
        {
            grabIndicators.gameObject.SetActive(true);

            for (int i = 0; i < grabAnimators.Length; i++)
            {
                grabAnimators[i].Play("Grab Available");
            }

            playingAnim = true;
        }
    }


    private void PlayGrabbingAnim()
    {
        if (!playingAnim)
        {
            grabIndicators.gameObject.SetActive(true);

            for (int i = 0; i < grabAnimators.Length; i++)
            {
                grabAnimators[i].Play("Grabbing");
            }

            playingAnim = true;
        }
    }


    private void StopAnim()
    {
        for (int i = 0; i < grabAnimators.Length; i++)
        {
            grabAnimators[i].StopPlayback();

            grabAnimators[i].transform.localPosition = Vector3.zero;
            grabAnimators[i].transform.localEulerAngles = Vector3.zero;

        }

        playingAnim = false;
        grabIndicators.gameObject.SetActive(false);
    }


    // Attaches the joint on this object to the rigidbody of the current targeted gripable
    public void Connect()
    {
        if (canGrip)
        {
            gameObject.AddComponent<FixedJoint>().connectedBody = currentGripable;
            currentGripping = currentGripable;


            //if (currentGripping != null && currentGripping.tag == "Slippery")
            //{
            //    PlayerSlippery.SetIsGrabbing(leftHand, true);
            //    SlipperyCube.SetActive(true);
            //    SlipperyCube.GetComponent<Rigidbody>().drag = 1f;
            //}

            if (currentGripping != null && currentGripping.tag == "Electric")
            {
                canGrip = false;
                transform.root.GetComponent<PlayerStun>().Stun(1);
                
            }

            // If a player is grabbed, that player will know it
            if (currentGripping != null && currentGripping.tag == "Player")
                currentGripping.transform.root.GetComponent<PlayerInfo>().AddGrabbingPlayer(transform.root.gameObject);

            StopAnim();
            PlayGrabbingAnim();
        }
    }


    // Disconnects connected body
    public void Disconnect()
    {
        Destroy(GetComponent<FixedJoint>());

        // If a player was grabbed, that player will know it no longer is
        if (currentGripping != null && currentGripping.tag == "Player")
            currentGripping.transform.root.GetComponent<PlayerInfo>().RemoveGrabbingPlayer(transform.root.gameObject);

        if (currentGripping != null && currentGripping.tag == "Slippery")
        {
            //PlayerSlippery.SetIsGrabbing(leftHand, false);
            //SlipperyCube.SetActive(false);
            //SlipperyCube.GetComponent<Rigidbody>().drag = Mathf.Infinity;
        }

        currentGripping = null;

        StopAnim();
    }

    // Disconnects and throws the grabbed object with force
    public void Disconnect(Vector3 throwDir, float throwForce)
    {
        Destroy(GetComponent<FixedJoint>());

        currentGripping.AddForce(throwDir * throwForce);

        currentGripping = null;

        StopAnim();
    }

}
