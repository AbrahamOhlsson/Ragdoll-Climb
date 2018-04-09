using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrip : MonoBehaviour
{
    public GameObject slipperyObj;

    public bool canGrip = true;

    public bool leftHand = true;
    
    // The rigidbody that is being gripped right now
    internal Rigidbody currentGripping;
    // The rigidbody that will be gripped
    internal Rigidbody currentGripable;

    [SerializeField] float breakForce = 3000f;

    [SerializeField] float failsafeCheckInterval = 0.2f;

    [SerializeField] Transform grabIndicators;

    bool nearBottomObj = false;
    bool playingAnim = false;

    float failsafeTimer = 0;

    Animator[] grabAnimators;

    List<Rigidbody> grabablesInReach = new List<Rigidbody>();

    Rigidbody tempRb = new Rigidbody();
    
    PlayerController controller;


    void Start ()
    {
        controller = transform.root.GetComponent<PlayerController>();

        //Finding the player
        grabAnimators = grabIndicators.GetComponentsInChildren<Animator>();

        GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
    }
	

	void Update ()
    {
        DetermineObjectToGrab();

        if (GetComponent<FixedJoint>())
        {
            if (GetComponent<FixedJoint>().currentForce.magnitude >= breakForce || GetComponent<FixedJoint>().connectedBody == null)
            {
                if (currentGripping.tag == "Throwable")
                    controller.ReleaseGrip(leftHand, true);
                else
                    controller.ReleaseGrip(leftHand, false);
            }
        }

        failsafeTimer += Time.deltaTime;
	}
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BottomObj")
        {
            nearBottomObj = true;

            controller.ReleaseGrip(leftHand, false);
        }

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable" || other.tag == "Electric" || other.tag == "Sticky")
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

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable" || other.tag == "Electric" || other.tag == "Sticky")
        {
            grabablesInReach.Remove(other.GetComponent<Rigidbody>());
            DetermineObjectToGrab();
        }

        if (other.tag == "Slippery" && currentGripping != tempRb && currentGripping.tag == "Slippery")
        {
            controller.ReleaseGrip(leftHand, false);
        }
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (grabablesInReach.Count == 0 &&  failsafeTimer >= failsafeCheckInterval)
        {
            //Debug.LogWarning("TIME TO CHECK");

            if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable" || other.tag == "Electric" || other.tag == "Sticky")
            {
                grabablesInReach.Add(other.GetComponent<Rigidbody>());
            }
            
            failsafeTimer = 0;
        }
    }


    // This will determine what to grab if there is multiple grabable objects in reach.
    // If there is multiple objects of the same type in reach, the last one that became in reach will be selected.
    private void DetermineObjectToGrab()
    {
        Rigidbody lastThrowable = new Rigidbody();
        Rigidbody lastOther = new Rigidbody();
        Rigidbody lastSticky = new Rigidbody();
        Rigidbody lastElectric = new Rigidbody();
        Rigidbody lastWall = new Rigidbody();
        
        bool foundThrowable = false;
        bool foundOther = false;
        bool foundSlippery = false;
        bool foundSticky = false;
        bool foundElectric = false;
        bool foundWall = false;

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
                // If a sticky wall was found
                if(tag == "Sticky")
                {
                    lastSticky = grabablesInReach[i];
                    foundSticky = true;
                }
                // If Electric is found
                else if (tag == "Electric")
                {
                    lastElectric = grabablesInReach[i];
                    foundElectric = true;
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
            else if (foundElectric)
            {
                currentGripable = lastElectric;

                if (lastElectric == tempRb)
                    StopAnim();
            }
            else if (foundSticky)
            {
                currentGripable = lastSticky;

                if (currentGripping == tempRb)
                    StopAnim();
            }
            else if (foundSlippery)
            {
                currentGripable = slipperyObj.GetComponent<Rigidbody>();

                if (currentGripping == tempRb)
                    StopAnim();
            }
            else if (foundWall)
            {
                currentGripable = lastWall;

                if (currentGripping == tempRb)
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
        else if (currentGripping == tempRb)
        {
            canGrip = false;
            currentGripable = tempRb;
        }
    }


    private void PlayGrabableAnim()
    {
        if (!playingAnim && currentGripping == tempRb)
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
            if (currentGripable.tag != "Electric" && currentGripable != tempRb)
            {
                // If a slippery wall was grabbed, the slippery child object will now move down
                if (currentGripable.tag == "Slippery")
                {
                    currentGripable.GetComponent<Rigidbody>().isKinematic = false;
                    currentGripable.transform.localPosition = Vector3.zero;
                }

                gameObject.AddComponent<FixedJoint>().connectedBody = currentGripable;
                currentGripping = currentGripable;
            }
            else
            {
                transform.root.GetComponent<PlayerStun>().Stun(1);
                transform.root.GetComponent<PlayerInfo>().feedbackText.Activate("got electrified!");
            }

            // If a player is grabbed, that player will know it
            if (currentGripping != tempRb && currentGripping.tag == "Player")
                currentGripping.transform.root.GetComponent<PlayerInfo>().AddGrabbingPlayer(transform.root.gameObject);

            StopAnim();
            PlayGrabbingAnim();
        }
    }
    

    // Disconnects connected body
    public void Disconnect()
    {
        Destroy(GetComponent<FixedJoint>());
        
        if (currentGripping != tempRb)
        {
            // If a player was grabbed, that player will know it no longer is
            if (currentGripping.tag == "Player")
                currentGripping.transform.root.GetComponent<PlayerInfo>().RemoveGrabbingPlayer(transform.root.gameObject);
            // If a slippery wall was released, the slippery child object will be non kinematic to prevent it from going down to infinity
            else if (currentGripping.tag == "Slippery")
                currentGripable.GetComponent<Rigidbody>().isKinematic = true;
        }

        currentGripping = tempRb;

        StopAnim();
    }


    // Disconnects and throws the grabbed object with force
    public void Disconnect(Vector3 throwDir, float throwForce)
    {
        Destroy(GetComponent<FixedJoint>());

        currentGripping.AddForce(throwDir * throwForce);

        currentGripping = tempRb;

        StopAnim();
    }


    public void RemoveFromGrabables(Rigidbody rb)
    {
        grabablesInReach.Remove(rb);
    }

}
