using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUp : MonoBehaviour
{
    GameObject PlayerTP;
    public GameObject particleSys;

    GameObject teleportPos;

    //public GameObject[] teleportPoints;
    public List<GameObject> teleportList;




    // Use this for initialization
    void Start ()
    {
        //particleSys = gameObject.transform.Find("Static Black Hole").gameObject;
        //Destroy(particleSys);
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (other.transform.root.GetComponent<PlayerInfo>().solid)
            {
                PlayerTP = other.transform.root.gameObject;
                GetTeleportPosition();
                Destroy(gameObject);
            }
        }
        else if (other.tag == "BottomObj")
            Destroy(gameObject);
    }

    void GetTeleportPosition()
    {
        foreach (Transform child in transform)
        {
            if (child.tag != "Particle Effect")
                teleportList.Add(child.gameObject);
        }

        teleportPos = teleportList [Random.Range(0, teleportList.Count-1)];
        PlayerTP.GetComponent<PlayerPowerups>().StartTeleport(teleportPos.transform.position);
    }
}
