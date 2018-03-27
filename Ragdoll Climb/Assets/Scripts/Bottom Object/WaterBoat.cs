using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBoat : MonoBehaviour
{
    public GameObject boat;
    //public GameObject water;
    public Transform endLerp;

    Rigidbody rb;

    public float speed;
    float startTime;
    float journeyLength;
    float playerDist = 15; 


    // Use this for initialization
    void Start ()
    {
        startTime = Time.time;

        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //water.transform.position = new Vector3(0, boat.transform.position.y - 0.5f, 0);

        journeyLength = Vector3.Distance(boat.transform.position, endLerp.position);

        if (endLerp.position.y > boat.transform.position.y  + playerDist)
        {
            float distCovered = (Time.time - startTime) * speed;
            //float fracJourney = distCovered / journeyLength;
            rb.position = Vector3.Lerp(boat.transform.position, endLerp.position, speed);
        }	
	}
}
