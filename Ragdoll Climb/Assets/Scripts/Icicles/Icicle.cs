using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    public float growthSpeed = 0.1f;
    public float stunTime = 2f;
    public float autoDestroyTime = 2f;
    [SerializeField] float playerDistForGrowth = 15f;
    [SerializeField] GameObject icicleDropEffect;
    [SerializeField] GameObject icicleShatterEffect;

    internal bool instantiated = false;

    enum States { Growing, Falling, Shrinking }
    States state = States.Growing;
    bool firstColl = false;

    float targetScale;
    float minScale = 0;
    float scale;

    float autoDestroyTimer;

    Transform bottomObj;

    Vector3 startPos;
    Vector3 particleOffset = new Vector3(0f, -1f, 0f);

    soundManager soundManager;

    Rigidbody rb;


	void Start ()
    {
        startPos = transform.localPosition;

        targetScale = transform.localScale.x;
        transform.localScale = new Vector3(minScale, minScale, minScale);
		scale = transform.localScale.x;

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        bottomObj = GameObject.FindGameObjectWithTag("BottomObj").transform;

		soundManager = GameObject.Find("music and sound").GetComponent<soundManager>();
	}
	
	void Update ()
    {
        switch(state)
        {
            case States.Growing:
                scale += growthSpeed * Time.deltaTime;
                transform.localScale = new Vector3(scale, scale, scale);

                if (scale >= targetScale)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    state = States.Falling;

                    Vector3 partSysPos = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                    Instantiate(icicleDropEffect, partSysPos, Quaternion.identity);
                }
                break;

            case States.Falling:
                if (autoDestroyTimer >= autoDestroyTime)
                    state = States.Shrinking;

                autoDestroyTimer += Time.deltaTime;
                break;

            case States.Shrinking:
                scale -= 2 * Time.deltaTime;
                transform.localScale = new Vector3(scale, scale, scale);

                if (scale <= 0)
                    DestroyIcicle();
                break;
        }
        
        if (transform.position.y <= bottomObj.position.y)
            Destroy(gameObject);
	}
    

    private void DestroyIcicle()
    {
        if (instantiated)
            Destroy(gameObject);
        else
        {
            scale = minScale;
            transform.localScale = new Vector3(minScale, minScale, minScale);
            transform.localPosition = startPos;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
            state = States.Growing;
            autoDestroyTimer = 0;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (!firstColl)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.collider.gameObject.GetComponent<Collider>());

            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

            firstColl = true;
        }
        else if (state == States.Falling)
        {
            if (other.gameObject.tag == "Player")
            {
                other.transform.root.GetComponent<PlayerStun>().Stun(stunTime);
                soundManager.PlaySound("icicles"); // sound on player hit
            }
            Instantiate(icicleShatterEffect, transform.position + particleOffset, Quaternion.identity);
            DestroyIcicle();
        }
        else if (state == States.Shrinking)
        {
            Instantiate(icicleShatterEffect, transform.position + particleOffset, Quaternion.identity);
            DestroyIcicle();
        }
    }
}
