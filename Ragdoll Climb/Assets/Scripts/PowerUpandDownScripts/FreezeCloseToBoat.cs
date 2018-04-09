using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCloseToBoat : MonoBehaviour
{
    [SerializeField] float tooCloseToBottom;

    GameObject bottomObj;

	// Use this for initialization
	void Start ()
    {
        bottomObj = GameObject.FindGameObjectWithTag("BottomObj");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y < bottomObj.transform.position.y + tooCloseToBottom)
        {
            transform.root.GetComponent<FreezePlayerPowerUp>().closeToBoat = true;
        }
        if (transform.position.y > bottomObj.transform.position.y + tooCloseToBottom)
        {
            transform.root.GetComponent<FreezePlayerPowerUp>().closeToBoat = false;
        }
    }
}
