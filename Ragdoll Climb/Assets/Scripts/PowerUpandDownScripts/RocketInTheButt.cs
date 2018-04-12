using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInTheButt : MonoBehaviour {

    public Rigidbody rb;

    public Transform player;
    
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

            GameObject test = GameObject.Find("Character Parts (Sir Climb)(Clone)");

            if (test != null)
            {
                player = transform.root.transform.Find("Character Parts (Sir Climb)(Clone)/Root_M/spine/chest/neck/head");  //.Find("Player 1");    
                
                rocketFuel += Time.deltaTime * 1;
                rb = player.GetComponent<Rigidbody>();
                rb.AddForce(transform.up * (rocketForce * 35));
            }
            else if(test == null) //test.gameObject.name == "Character Parts (Mannequin)(Clone)"
            {
                player = transform.root.transform.Find("Character Parts (Mannequin)(Clone)/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Neck_M/NeckPart1_M/NeckPart2_M/Head_M");  //.Find("Player 1");    
               
                rocketFuel += Time.deltaTime * 1;
                rb = player.GetComponent<Rigidbody>();
                rb.AddForce(transform.up * (rocketForce * 35));
            }
            else
            {
                print("Not Mannequin nor Sir Climb");
            }

            if (rocketFuel >= rocketTank)
            {
                rocketFuel = 0;
                flyReady = false;
                gameObject.SetActive(false);
            }
        }
	}

    public void startTheRocket()
    {
        flyReady = true;
        print("The Rocket is online! " + player);
        //GetComponent<PlayerController>().ReleaseGrip(true, false);
    }
}
