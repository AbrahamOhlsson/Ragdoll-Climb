using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nobout : MonoBehaviour {

    public GameObject startObj;

    [SerializeField]
    float destroyTeleportationChild;

 

	// Use this for initialization
	void Start ()
    {   
        //Add manager with boat
        //startObj = manager.boat
        //remove public from gameobject startObj.
        startObj = FindObjectOfType<teleportBoatCheck>().bout;   // tabort gör inget
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < startObj.transform.position.y + destroyTeleportationChild)
        {
            Destroy(gameObject);
        }
	}
}
