using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpRespawnList : MonoBehaviour
{
    /// <summary>
    /// ***THIS SCRIPT IS FOR FLOATING POWER-UPS LIKE (Anvil Down, Feather Up)
    /// </summary>
    
    public List<GameObject> firstPowerUpList;

    public List<GameObject> goodPowerUpList;

    public GameObject contdownText;
    
    private GameObject player;
    [Range(0, 10)] [Tooltip("Cooldown per second")] public float cooldown;
    private bool touched;
    float cooldownTime;

    //floating 
    [SerializeField]
    bool moving;
    [SerializeField]
    private float rotateSpeed = 15.0f;
    //[SerializeField]
    private float floatLength = 0.3f;
    [SerializeField]
    private float floatSpeed = 1f;
    Vector3 startPos = new Vector3();
    Vector3 tempPos = new Vector3();
    private GameObject TEXT;
    bool firstSpawn;
    int cdInt;
    int startInt;

    void Start()
    {
        startPos = transform.position;
        startInt = Random.Range(0, 2);
        cooldownTime = cooldown;
        firstSpawn = true;

        floatLength = 0;

        cdInt = 0;

    }

    //  Activates spawn
    private void Update()
    {
        if(cdInt == 10 && firstSpawn)
        {
            if (startInt == 0)
            {
                GameObject newObj = firstPowerUpList[Random.Range(0, firstPowerUpList.Count)];
                if (newObj.GetComponent<LowerMass>() == null)
                {
                    moving = false;
                }
                else
                {
                    moving = true;
                }

                Instantiate(newObj, transform.position, transform.rotation, transform.parent); // Spawns the powerUp, one time
                touched = false;


            }
            if (startInt == 1)
            {
                GameObject newObj = goodPowerUpList[Random.Range(0, goodPowerUpList.Count)];
                if (newObj.GetComponent<LowerMass>() == null)
                {
                    moving = false;
                }
                else
                {
                    moving = true;
                }

                Instantiate(newObj, transform.position, transform.rotation, transform.parent); // Spawns the powerUp, one time
                touched = false;


            }
            firstSpawn = false;
            //print(cdInt + "nu skapas skiten ");

        }

        if (firstSpawn){
            cdInt++;
        }

        // Rotate
        transform.Rotate(new Vector3(0f, Time.deltaTime * rotateSpeed, 0f), Space.World);

        if (moving)
        {
            // Float up/down with a Sin()
            tempPos = transform.position;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatSpeed) * floatLength;

            transform.position = tempPos;
        }

        if (touched == true)
        {
            TEXT.GetComponent<TextMesh>().text = Mathf.FloorToInt(cooldownTime).ToString();
            TEXT.transform.position = transform.position; 
            cooldownTime -= Time.deltaTime * 1;

            

            if (cooldownTime <= 0)
            {
                GameObject newObj = goodPowerUpList[Random.Range(0, goodPowerUpList.Count)];
                if (newObj.GetComponent<LowerMass>() == null)
                {
                    moving = false;
                }
                else
                {
                    moving = true;
                }
                transform.position = startPos;

                Instantiate(newObj , transform.position, transform.rotation, transform.parent);
                touched = false;
                cooldownTime = cooldown;
                Destroy(TEXT);
            }
        }



    }

    // Resets Spawn trigger
    private void OnTriggerEnter(Collider other)
    {
        if (touched == false)
        {
            player = other.transform.gameObject;
            if (player.tag == "Player")
            {
                TEXT = Instantiate(contdownText);
                touched = true;
            }
        }
    }
}
