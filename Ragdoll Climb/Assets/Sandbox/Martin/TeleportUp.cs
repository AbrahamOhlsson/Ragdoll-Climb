using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUp : MonoBehaviour {

    [SerializeField] GameObject PlayerCol;

    /////// For the players position and the teleportation position
    [SerializeField] int playerPos;
    [SerializeField] int teleportPos;

    ///////When activated, time left until teleportation happens.
    [SerializeField] float timeToTeleport;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //void OnTriggerEnter(Collider other)
    //{
    //    PlayerCol = other.transform.root.gameObject;

    //    if(PlayerCol.tag == "Player")
    //    {
            
    //    }
    //    Destroy(gameObjec);

    //    startTeleporting();
    //}

    //void startTeleporting()
    //{


    //}
}
