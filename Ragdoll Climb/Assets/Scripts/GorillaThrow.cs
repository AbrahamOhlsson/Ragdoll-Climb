using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorillaThrow : MonoBehaviour
{
    public float thrust = 1000;
    public float randomDire;
    public float upDire;
    public float lerpSpeed = 0.1f;
    public float throwDelay;
    public float inactiveTime;
    public bool direction;

    float throwYDist;
    float throwXDist;

    float throwTimer;
    float inactiveTimer;
    bool playerCollision = false;
    bool inactive;


    ParticleSystem smoke;
    Transform playerDirection;
    Rigidbody playerForce;
    Rigidbody[] bodyParts;

	// Use this for initialization
	void Start ()
    {
        smoke = GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update ()
    {
        if (playerCollision && !inactive)
        {
            print("Is lerping");
            playerForce.position = Vector3.Lerp(playerForce.position, smoke.transform.position, lerpSpeed);
            
            if(playerForce.position.x < smoke.transform.position.x + 1 && playerForce.position.y < smoke.transform.position.y + 1)
            {
                throwTimer -= Time.deltaTime; 
            }

            if (throwTimer <= 0)
            {   
                if(direction)
                {
                    //It is forward for the gorilla is rotated.
                    //playerForce.AddForce(new Vector3(Random.Range(randomDire, -randomDire), Random.Range(-upDire, upDire), 0) * thrust);
                    throwYDist = Random.Range(4, 8);

                    playerForce.AddForce(new Vector3(0, throwYDist, 0) * thrust);
                }
                if(!direction)
                {
                    //It is forward for the gorilla is rotated.
                    //playerForce.AddForce(new Vector3(Random.Range(randomDire, -randomDire), Random.Range(-upDire, upDire), 0) * thrust);

                    //playerForce.AddForce(new Vector3(0, 0, 0) * thrust);
                }

                inactiveTimer = inactiveTime;

                smoke.Pause();
                smoke.Clear();

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

                other.transform.root.GetComponent<PlayerController>().canMove = false;

                for (int i = 0; i < bodyParts.Length; i++)
                {
                    if (bodyParts[i].name == "Spine1_M")
                    {
                        playerForce = bodyParts[i];
                        break;
                    }
                }

                throwTimer = throwDelay;
                playerCollision = true;
            }
        }
    }
}
