using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPowerUpNonFloat : MonoBehaviour {

    /// <summary>
    /// ***THIS SCRIPT IS FOR NON-FLOATING POWER-UPS
    /// </summary>


    [Tooltip("Put the given powerUp")]
    public GameObject powerUp;


    private GameObject player;
    private float cooldown = 3;
    private bool touched;
    private Vector3 powerPos;



    void Start()
    {
        powerPos = transform.position;
        Debug.Log(powerUp.transform.position);
        Instantiate(powerUp, powerPos, transform.rotation); // Spawns the powerUp, one time
        touched = false;
    }



    //  Resets Spawn trigger
    private void OnTriggerEnter(Collider other)
    {
        if (touched == false)
        {
            player = other.transform.gameObject;
            if (player.tag == "Player")
            {
                Debug.Log("collision!");
                touched = true;
            }
        }
    }

    //  Spawn generator
    IEnumerator spawnObjects()
    {
        yield return new WaitForSeconds(cooldown);

        Debug.Log("SPAWNS");
        Debug.Log(powerUp);
        Instantiate(powerUp, powerPos, transform.rotation);
    }
}
