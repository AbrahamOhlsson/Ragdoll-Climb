using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBoat : MonoBehaviour
{
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

        journeyLength = Vector3.Distance(rb.position, endLerp.position);

        Vector3 targetPos = new Vector3(rb.position.x, endLerp.position.y, rb.position.z);

        if (endLerp.position.y > rb.position.y  + playerDist)
        {
            float distCovered = (Time.time - startTime) * speed;
            //float fracJourney = distCovered / journeyLength;
            rb.position = Vector3.Lerp(rb.position, targetPos, speed);
        }	
	}
}
