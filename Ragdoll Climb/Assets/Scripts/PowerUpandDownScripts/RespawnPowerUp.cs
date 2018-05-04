using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPowerUp : MonoBehaviour {


    /// <summary>
    /// ***THIS SCRIPT IS FOR FLOATING POWER-UPS LIKE (Anvil Down, Feather Up)
    /// </summary>


    [Tooltip("Put the given Floating powerUp")]
    public GameObject powerUp;


    private GameObject player;
    [Range(0, 10)] [Tooltip("Cooldown per second")] public float cooldown = 3;
    private bool touched;


    //floating 
    [SerializeField]
    private float rotateSpeed = 15.0f;
    [SerializeField]
    private float floatLength = 0.3f;
    [SerializeField]
    private float floatSpeed = 1f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    void Start()
    {
        posOffset = transform.position;
        Debug.Log(powerUp.transform.position);
        Instantiate(powerUp, posOffset, transform.rotation, transform.parent); // Spawns the powerUp, one time
        touched = false;
    }



    //  Activates spawn
    private void Update()
    {
        // Rotate
        transform.Rotate(new Vector3(0f, Time.deltaTime * rotateSpeed, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatSpeed) * floatLength;

        transform.position = tempPos;

        if (touched == true)
        {
            cooldown -= Time.deltaTime * 1;
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
        Instantiate(powerUp, posOffset, transform.rotation, transform.parent);
    }

}
