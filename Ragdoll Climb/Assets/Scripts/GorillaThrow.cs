﻿using System.Collections;
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
    public float playerStunTime = 1;

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
            playerForce.AddForce((lerpPos - playerForce.position).normalized * 500f);
            throwTimer -= Time.deltaTime;

            if (throwTimer <= 0)
            {
                throwYDist = Random.Range(minY, maxY);
                throwXDist = Mathf.Sqrt(1 - Mathf.Pow(throwYDist, 2));

                //Throw the player to the left.
                if (left)
                {
                    for (int i = 0; i < bodyParts.Length; i++)
                    {
                        bodyParts[i].AddForce(new Vector3(-throwXDist, throwYDist, 0) * thrust);
                    }
                }
                //Throw the player to the right.
                if(!left)
                {
                    for (int i = 0; i < bodyParts.Length; i++)
                    {
                        bodyParts[i].AddForce(new Vector3(throwXDist, throwYDist, 0) * thrust);
                        
                    }
                }

                inactiveTimer = inactiveTime;

                //Apply stun effect to the player.
                playerForce.transform.root.gameObject.GetComponent<PlayerStun>().Stun(playerStunTime);

                //Stop all of Gorilla particle effects.
                smoke.Stop();
                stars.Stop();

                inactive = true;
                playerCollision = false;
                
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
        //Get the player referens.
        if (other.gameObject.CompareTag("Player"))
        {
            if(!playerCollision && !inactive)
            {
                bodyParts = other.transform.root.GetComponentsInChildren<Rigidbody>();
                smoke.Play();
                stars.Play();

                //Remove the players movement.
                other.transform.root.GetComponent<PlayerController>().canMove = false;
                other.transform.root.GetComponent<PlayerController>().ReleaseGrip(true, false);
                other.transform.root.GetComponent<PlayerController>().ReleaseGrip(false, false);

                //Looking for the bodypart "Spine1_M" and then set its posision.
                for (int i = 0; i < bodyParts.Length; i++)
                {
                    if (bodyParts[i].name == "Spine1_M")
                    {
                        playerForce = bodyParts[i];
                        lerpPos.z = playerForce.position.z;

                        break;
                    }
                }

                //Add time to the throwTimer to delay the thorw. 
                throwTimer = throwDelay;
                playerCollision = true;
            }
        }
        //If the gorilla is colliding with the bottom object remove the gorilla from the game.
        if (other.gameObject.CompareTag("BottomObj"))
        {
            if (playerCollision)
                playerForce.transform.root.GetComponent<PlayerController>().canMove = true;

            Destroy(gameObject);
        }
    }
}
