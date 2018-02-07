using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBoat : MonoBehaviour
{
    public GameObject boat;
    public GameObject water;
    public Transform endLerp;

    public float speed;
    float startTime;
    float journeyLength;

    float timer;


    // Use this for initialization
    void Start ()
    {
        timer = 30;
        startTime = Time.time;
        journeyLength = Vector3.Distance(boat.transform.position, endLerp.position);
	}
	
	// Update is called once per frame
	void Update ()
    {
        water.transform.position = new Vector3(0, boat.transform.position.y - 0.5f, 0);

        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if(timer < 0)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(boat.transform.position, endLerp.position, fracJourney);
        }




		
	}
}
