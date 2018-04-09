using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpRespawnList : MonoBehaviour {


    /// <summary>
    /// ***THIS SCRIPT IS FOR FLOATING POWER-UPS LIKE (Anvil Down, Feather Up)
    /// </summary>



    public List<GameObject> firstPowerUpList;

    public List<GameObject> goodPowerUpList;


    private GameObject player;
    [Range(0, 10)] [Tooltip("Cooldown per second")] public float cooldown;
    private bool touched;
    float cooldownTime;


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
        int startInt = Random.Range(0, 2);
        cooldownTime = cooldown;

        if (startInt == 0)
        {

            Instantiate(firstPowerUpList[Random.Range(0,firstPowerUpList.Count)], posOffset, transform.rotation); // Spawns the powerUp, one time
            touched = false;


        }
        if (startInt == 1)
        {
            Instantiate(goodPowerUpList[Random.Range(0, goodPowerUpList.Count)], posOffset, transform.rotation); // Spawns the powerUp, one time
            touched = false;



        }

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
            cooldownTime -= Time.deltaTime * 1;
            if (cooldownTime <= 0)
            {
                Debug.Log("Touched = true");
                
                Instantiate(goodPowerUpList[Random.Range(0, goodPowerUpList.Count)], posOffset, transform.rotation);
                touched = false;
                cooldownTime = cooldown;
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


}
