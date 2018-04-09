using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPowerUpNonFloat : MonoBehaviour {

    /// <summary>
    /// ***THIS SCRIPT IS FOR NON-FLOATING POWER-UPS
    /// </summary>


    [Tooltip("Put the given NON-floating powerUp")]
    public GameObject powerUp;


    private GameObject player;
    [Range (0, 10)] [Tooltip("Cooldown per second")]public float cooldown = 3;
    private bool touched;
    private Vector3 powerPos;



    void Start()
    {
        powerPos = transform.position;
        Debug.Log(powerUp.transform.position);
        Instantiate(powerUp, powerPos, transform.rotation); // Spawns the powerUp, one time
        touched = false;
    }



    //  Activates spawn
    private void Update()
    {

        if (touched == true)
        {
            cooldown -= Time.deltaTime * 1; //cooldown goes by per seconds
            if (cooldown <= 0)
            {
                Debug.Log("Touched = true");
                touched = false;
                StartCoroutine("spawnObjects");
                cooldown = 3;
            }
        }



    }

    //  Resets Spawn trigger
    private void OnTriggerEnter(Collider other)
    {
        if (touched == false)
        {
            player = other.transform.gameObject;
            if (player.tag == "Player")
            {
                Debug.Log("collision non float!");
                touched = true;
            }
        }
    }

    //  Spawn generator
    IEnumerator spawnObjects()
    {
        yield return new WaitForSeconds(cooldown);

        Debug.Log("SPAWNS Non float");
        Debug.Log(powerUp);
        Instantiate(powerUp, powerPos, transform.rotation);
    }
}
