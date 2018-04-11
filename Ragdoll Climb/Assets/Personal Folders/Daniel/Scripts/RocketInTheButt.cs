using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInTheButt : MonoBehaviour {

    public Rigidbody rb;

    private GameObject player;
    
    [SerializeField]
    float rocketTank;
    [SerializeField]
    float rocketForce;
    private float rocketFuel;


   public bool flyReady;

    // Update is called once per frame
    void Update ()
    {
		if(flyReady)
        {
            rocketFuel += Time.deltaTime * 1;
            
            rb.AddForce(transform.forward * rocketForce);

            if (rocketFuel >= rocketTank || flyReady == false)
            {
                rocketFuel = 0;
                flyReady = false;
                gameObject.SetActive(false);
            }
        }
	}

    public void startTheRocket()
    {
        player = transform.Find("Head_M").gameObject;
        rb = player.GetComponent<Rigidbody>();
        flyReady = true;
        print("The Rocket is online!");
    }
}
