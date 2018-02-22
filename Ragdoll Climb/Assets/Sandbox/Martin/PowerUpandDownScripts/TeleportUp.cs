using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUp : MonoBehaviour {


    GameObject PlayerTP;
    GameObject particleSys;

    GameObject teleportPos;

    //public GameObject[] teleportPoints;
    public List<GameObject> teleportList;




    // Use this for initialization
    void Start ()
    {
        particleSys = gameObject.transform.Find("Static Black Hole").gameObject;

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        foreach(Transform child in transform)
        {
            teleportList.Add(child.gameObject);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(particleSys);

        PlayerTP = other.transform.root.gameObject;

        if(PlayerTP.tag == "Player")
        {
            GetTeleportPosition();
        }
        Destroy(gameObject);
    }

    void GetTeleportPosition()
    {
        teleportPos = teleportList [Random.Range(0, teleportList.Count-1)];
        PlayerTP.GetComponent<PlayerPowerups>().StartTeleport(teleportPos.transform.position);
    }
}
