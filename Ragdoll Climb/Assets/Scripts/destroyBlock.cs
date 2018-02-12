using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBlock : MonoBehaviour {

    public GameObject ship;
    public float distance;

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (  transform.position.y < (ship.transform.position.y + distance))
        {
            Destroy(gameObject);
        }
	}
}
