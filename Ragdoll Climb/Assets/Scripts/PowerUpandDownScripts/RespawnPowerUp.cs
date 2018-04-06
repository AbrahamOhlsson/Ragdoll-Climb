using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPowerUp : MonoBehaviour {

    public GameObject powerUp;
    private Vector3 powerPos;

    GameObject player;

    float cooldown = 3;



    bool touched;

    void Start()
    {
        powerPos = transform.position;
        Debug.Log(powerUp.transform.position);
        Instantiate(powerUp, powerPos, transform.rotation);
        touched = false;
       
    }

    private void Update()
    {
        if (touched == true)
        {
            cooldown -= Time.deltaTime * 1;

            if (cooldown <= 0)
            {
                touched = false;
                StartCoroutine("spawnObjects");
                cooldown = 3;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(touched == false)
        { 
            player = other.transform.gameObject;
            if (player.tag == "Player")
            {
                Debug.Log("collision!");
                StartCoroutine("spawnObjects");
                touched = true;
            }
        }
    }
    //StartCoroutine("spawnObjects"); << use in collision check


    IEnumerator spawnObjects()
    {

        yield return new WaitForSeconds(cooldown);

        Debug.Log("SPAWNS");
        Debug.Log(powerUp);
        Instantiate(powerUp, powerPos, transform.rotation);
        //Instantiate(powerUp, transform.position, transform.rotation);
    }

}
