using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGrip : MonoBehaviour
{
    public GameObject slipperyObj;

    public bool canGrip = true;
    public bool onFire = false;

    public bool leftHand = true;
    
    // The rigidbody that is being gripped right now
    internal Rigidbody currentGripping;
    // The rigidbody that will be gripped
    internal Rigidbody currentGripable;

    internal PlayerController controller;

    [SerializeField] float fireTime = 2f;
    [SerializeField] float minFireForceInterval = 0.1f;
    [SerializeField] float maxFireForceInterval = 0.5f;
    [SerializeField] float minFireForce = 100f;
    [SerializeField] float maxFireForce = 500f;

    [SerializeField] float breakForce = 3000f;

    [SerializeField] float failsafeCheckInterval = 0.2f;

    [SerializeField] Transform grabIndicators;

    [SerializeField] ParticleSystem fireParticle;

    bool nearBottomObj = false;
    bool playingAnim = false;

    float fireTimer = 0f;
    float fireForceTimer;
    float fireforceInterval;

    float failsafeTimer = 0;
    
    Animator[] grabAnimators;

    List<Rigidbody> grabablesInReach = new List<Rigidbody>();
    
    Rigidbody rb;
    Rigidbody tempRb = new Rigidbody();
    
    
    void Start ()
    {
        controller = transform.root.GetComponent<PlayerController>();

        //Finding the player
        grabAnimators = grabIndicators.GetComponentsInChildren<Animator>();

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
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


    private void FixedUpdate()
    {
        if (onFire)
        {
            if (fireTimer >= fireTime)
            {
                onFire = false;
                fireTimer = 0;
                controller.ReleaseGrip(leftHand, false);
                fireParticle.Stop();
                fireParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
                fireParticle.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            }
            else
            {
                if (fireForceTimer >= fireforceInterval)
                {
                    float forceX = Random.Range(minFireForce, maxFireForce);
                    float forceY = Random.Range(minFireForce, maxFireForce);

                    if (Random.Range(0, 2) == 1)
                        forceX *= -1;
                    if (Random.Range(0, 2) == 1)
                        forceY *= -1;

                    rb.AddForce(new Vector2(forceX, forceY));

                    fireForceTimer = 0;
                    fireforceInterval = Random.Range(minFireForceInterval, maxFireForceInterval);
                }

                fireForceTimer += Time.deltaTime;
                fireTimer += Time.deltaTime;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BottomObj")
        {
            nearBottomObj = true;
            
            controller.ReleaseGrip(leftHand, false);
        }

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable" || other.tag == "Electric" || other.tag == "Sticky" || other.tag == "Breaking" || other.tag == "LavaWall" || other.tag == "LavaRock")
        {
            grabablesInReach.Add(other.GetComponent<Rigidbody>());
        }

        if (currentGripping != tempRb && currentGripping.tag == "LavaRock" && other.tag == "Wall")
        {
            controller.ReleaseGrip(leftHand, false);
            DetermineObjectToGrab();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BottomObj")
        {
            nearBottomObj = false;
        }

        if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable" || other.tag == "Electric" || other.tag == "Sticky" || other.tag == "Breaking" || other.tag == "LavaWall" || other.tag == "LavaRock")
        {
            grabablesInReach.Remove(other.GetComponent<Rigidbody>());
            DetermineObjectToGrab();
        }

        if (other.tag == "Slippery" && currentGripping != tempRb && currentGripping.tag == "Slippery")
        {
            controller.ReleaseGrip(leftHand, false);
            DetermineObjectToGrab();
        }
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (grabablesInReach.Count == 0 &&  failsafeTimer >= failsafeCheckInterval)
        {
            //Debug.LogWarning("TIME TO CHECK");

            if (other.tag == "Player" || other.tag == "Grabable" || other.tag == "Slippery" || other.tag == "Wall" || other.tag == "Throwable" || other.tag == "Electric" || other.tag == "Sticky" || other.tag == "Breaking" || other.tag == "LavaWall" || other.tag == "LavaRock")
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
        Rigidbody lastLava = new Rigidbody();
        Rigidbody lastBreaking = new Rigidbody();
        Rigidbody lastLavaRock = new Rigidbody();
        Rigidbody lastWall = new Rigidbody();
        
        bool foundThrowable = false;
        bool foundOther = false;
        bool foundSlippery = false;
        bool foundSticky = false;
        bool foundElectric = false;
        bool foundLava = false;
        bool foundBreaking = false;
        bool foundLavaRock = false;
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
                // If a breakable wall is found
                else if (tag == "Breaking")
                {
                    lastBreaking = grabablesInReach[i];
                    foundBreaking = true;
                }
                // If a sticky wall was found
                else if(tag == "Sticky")
                {
                    lastSticky = grabablesInReach[i];
                    foundSticky = true;
                }
                // If an electric is found
                else if (tag == "Electric")
                {
                    lastElectric = grabablesInReach[i];
                    foundElectric = true;
                }
                // If lava is found
                else if (tag == "LavaWall")
                {
                    lastLava = grabablesInReach[i];
                    foundLava = true;
                }
                // If a slippery wall is found
                else if (tag == "Slippery")
                {
                    foundSlippery = true;
                }
                // If a non-special object or player is found is found
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
                else if (tag == "LavaRock")
                {
                    lastLavaRock = grabablesInReach[i];
                    foundLavaRock = true;
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
            else if (foundBreaking)
            {
                currentGripable = lastBreaking;

                if (currentGripping == tempRb)
                    StopAnim();
            }
            else if (foundWall)
            {
                currentGripable = lastWall;

                if (currentGripping == tempRb)
                    StopAnim();
            }
            else if (foundLavaRock)
            {
                currentGripable = lastLavaRock;
                PlayGrabableAnim();
            }
            else if (foundLava)
            {
                currentGripable = lastLava;

                if (currentGripping == tempRb)
                    StopAnim();
            }


            // You cant grip anything if the bottom object is in reach.
            // This prevents the player from holding while that object goes throught the player
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
            StopAnim();
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
            if (currentGripable.tag != "Electric" && currentGripable.tag != "LavaWall" && currentGripable != tempRb)
            {
                // If a slippery wall was grabbed, the slippery child object will now move down
                if (currentGripable.tag == "Slippery")
                {
                    currentGripable.GetComponent<Rigidbody>().isKinematic = false;
                    currentGripable.transform.localPosition = Vector3.zero;

                    int squeakIndex = Random.Range(1, 4);
                    transform.root.GetComponent<playerSound>().PlaySoundRandPitch("IceSqueak" + squeakIndex);
                }
                // If a player is grabbed, that player will know it
                else if (currentGripable.tag == "Player")
                    currentGripable.transform.root.GetComponent<PlayerInfo>().AddGrabbingPlayer(transform.root.gameObject);
                else if (currentGripable.tag == "Breaking")
                    currentGripable.transform.parent.GetComponent<BreakingSurface>().AddGrabbingHand(this);

                if (currentGripable.tag == "Throwable")
                {
                    currentGripable.constraints = RigidbodyConstraints.None;
                    currentGripable.GetComponent<DamageThrowable>().holdAmount++;
                    currentGripable.useGravity = false;
                }

                gameObject.AddComponent<FixedJoint>().connectedBody = currentGripable;
                gameObject.GetComponent<FixedJoint>().enableCollision = true;
                gameObject.GetComponent<FixedJoint>().enablePreprocessing = false;

                currentGripping = currentGripable;
            }
            else
            {
                if (currentGripable.tag == "Electric")
                {
                    transform.root.GetComponent<PlayerStun>().Stun(1);
                    transform.root.GetComponent<PlayerInfo>().feedbackText.Activate("got electrified!");
                    transform.root.GetComponent<playerSound>().PlaySound("spark");
                }
                else if (currentGripable.tag == "LavaWall")
                {
                    transform.root.GetComponent<playerSound>().PlaySound("LavaTouch");
                    BurnHand("'s hand is burning!");
                }
            }
            
            StopAnim();
            PlayGrabbingAnim();
        }
    }
    

    // Disconnects connected body
    public void Disconnect()
    {
        //if (currentGripping.tag == "Throwable")
        //    Destroy(currentGripping.GetComponent<CharacterJoint>());
        //else
            Destroy(GetComponent<FixedJoint>());

        if (currentGripping != tempRb)
        {
            // If a player was grabbed, that player will know it no longer is
            if (currentGripping.tag == "Player")
                currentGripping.transform.root.GetComponent<PlayerInfo>().RemoveGrabbingPlayer(transform.root.gameObject);
            // If a slippery wall was released, the slippery child object will be non kinematic to prevent it from going down to infinity
            else if (currentGripping.tag == "Slippery")
                currentGripping.GetComponent<Rigidbody>().isKinematic = true;
            else if (currentGripping.tag == "Throwable")
                ResetThrowable();
            else if (currentGripping.tag == "Breaking")
                currentGripping.transform.parent.GetComponent<BreakingSurface>().RemoveGrabbingHand(this);
        }

        currentGripping = tempRb;

        StopAnim();
    }


    // Disconnects and throws the grabbed object with force
    public void Disconnect(Vector3 throwDir, float throwForce)
    {
        Destroy(GetComponent<FixedJoint>());

        ResetThrowable();

        // Throws object in direction of velocity with multipled force
        currentGripping.AddForce(new Vector3(currentGripping.velocity.x, currentGripping.velocity.y, 0f) * throwForce);
        
        currentGripping = tempRb;

        StopAnim();
    }


    public void BurnHand(string feedbackText)
    {
        if (!onFire)
        {
            onFire = true;
            controller.ReleaseGrip(leftHand, false);
            fireParticle.Play();
            fireParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            fireParticle.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            transform.root.GetComponent<PlayerInfo>().feedbackText.Activate(feedbackText);
            transform.root.GetComponent<VibrationManager>().VibrateTimed(0.8f, fireTime, 12);
        }
    }


    private void ResetThrowable()
    {
        DamageThrowable throwable = currentGripping.GetComponent<DamageThrowable>();

        throwable.holdAmount--;

        if (throwable.holdAmount <= 0)
        {
            currentGripping.transform.position = new Vector3(currentGripping.position.x, currentGripping.position.y, throwable.startZPos);
            currentGripping.constraints = RigidbodyConstraints.FreezePositionZ;
            currentGripping.useGravity = true;
        }
    }


    public void RemoveFromGrabables(Rigidbody rb)
    {
        if (grabablesInReach.Exists(x => x == rb))
            grabablesInReach.Remove(rb);
        else
            Debug.LogWarning("The rigidbody whose GameObject called " + rb.gameObject.name + " doesn't exist in the list grabablesInReach!");
    }
}
