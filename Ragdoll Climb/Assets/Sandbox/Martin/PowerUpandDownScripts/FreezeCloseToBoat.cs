using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCloseToBoat : MonoBehaviour {

    public GameObject Boat;

    [SerializeField]
    float tooCloseToBoat;

	// Use this for initialization
	void Start ()
    {
        //Boat = FindObjectOfType<teleportBoatCheck>().bout;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y < Boat.transform.position.y + tooCloseToBoat)
        {
            transform.root.GetComponent<FreezePlayerPowerUp>().closeToBoat = true;
        }
        if (transform.position.y > Boat.transform.position.y + tooCloseToBoat)
        {
            transform.root.GetComponent<FreezePlayerPowerUp>().closeToBoat = false;
        }
    }
}
