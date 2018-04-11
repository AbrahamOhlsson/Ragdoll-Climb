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
            
            rocketFuel += Time.deltaTime * 1;
            
            rb.AddForce(transform.up * (rocketForce * 35));


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
        player = transform.root.transform.Find("Character Parts (Mannequin)(Clone)/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Neck_M/NeckPart1_M/NeckPart2_M/Head_M");  //.Find("Player 1");    
        rb = player.GetComponent<Rigidbody>();
        flyReady = true;
        print("The Rocket is online! " + player);
        
        //GetComponent<PlayerController>().ReleaseGrip(true, false);
    }
}
