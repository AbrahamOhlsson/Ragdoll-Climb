using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaThrow : MonoBehaviour
{
    public float thrust = 1000;
    [Range(-1f, 1f)]
    public float minY = -0.5f;
    [Range(-1f, 1f)]
    public float maxY = 1f;
    [Range(0f, 1f)]
    public float lerpSpeed = 0.1f;
    public float throwDelay;
    public float inactiveTime;
    public bool left;

    float throwYDist;
    float throwXDist;

    float throwTimer;
    float inactiveTimer;
    bool playerCollision = false;
    bool inactive;

    Vector3 lerpPos;

    ParticleSystem stars;
    ParticleSystem smoke;
    Transform playerDirection;
    Rigidbody playerForce;
    Rigidbody[] bodyParts;

	// Use this for initialization
	void Start ()
    {
        smoke = GetComponent<ParticleSystem>();
        stars = GetComponentInChildren<ParticleSystem>();

        lerpPos = new Vector3(smoke.transform.position.x - smoke.shape.position.y, smoke.transform.position.y + smoke.shape.position.z, 0f);
	}

	// Update is called once per frame
	void FixedUpdate ()
    {
        if (playerCollision && !inactive)
        {
            print("Is lerping");
            
            //playerForce.position = Vector3.Lerp(playerForce.position, lerpPos, lerpSpeed);
            playerForce.AddForce((lerpPos - playerForce.position).normalized * 500f);

            //if (playerForce.position.x < smoke.transform.position.x + 1 && playerForce.position.y < smoke.transform.position.y + 1)
            //{
                throwTimer -= Time.deltaTime; 
            //}

            if (throwTimer <= 0)
            {
                throwYDist = Random.Range(minY, maxY);
                throwXDist = Mathf.Sqrt(1 - Mathf.Pow(throwYDist, 2));

                print("X = " + throwXDist + ",  Y = " + throwYDist);

                if (left)
                {
                    //playerForce.AddForce(new Vector3(-throwXDist, throwYDist, 0) * thrust);
                    for (int i = 0; i < bodyParts.Length; i++)
                    {
                        bodyParts[i].AddForce(new Vector3(-throwXDist, throwYDist, 0) * thrust);
                    }
                }
                if(!left)
                {
                    //playerForce.AddForce(new Vector3(throwXDist, throwYDist, 0) * thrust);
                    for (int i = 0; i < bodyParts.Length; i++)
                    {
                        bodyParts[i].AddForce(new Vector3(throwXDist, throwYDist, 0) * thrust);
                    }
                }

                inactiveTimer = inactiveTime;

                smoke.Stop();
                stars.Stop();

                inactive = true;
                playerCollision = false;

                playerForce.transform.root.GetComponent<PlayerController>().canMove = true;
                playerForce = null;
            }
        }

        if (inactive)
        {
            inactiveTimer -= Time.deltaTime;

            if (inactiveTimer <= 0)
            {
                inactive = false;
            }
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!playerCollision && !inactive)
            {
                print("Player found");
                bodyParts = other.transform.root.GetComponentsInChildren<Rigidbody>();
                smoke.Play();
                stars.Play();

                other.transform.root.GetComponent<PlayerController>().canMove = false;
                other.transform.root.GetComponent<PlayerController>().ReleaseGrip(true, false);
                other.transform.root.GetComponent<PlayerController>().ReleaseGrip(false, false);

                for (int i = 0; i < bodyParts.Length; i++)
                {
                    if (bodyParts[i].name == "Spine1_M")
                    {
                        playerForce = bodyParts[i];
                        lerpPos.z = playerForce.position.z;

                        break;
                    }
                }

                throwTimer = throwDelay;
                playerCollision = true;
            }
        }
    }
}
