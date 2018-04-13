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
    public GameObject test;

   public bool flyReady;

    // LUDVIG TEST
   bool firstTime = true;
   public  List<GameObject> LudvigTestList;

    // Update is called once per frame
    private void FixedUpdate()
    {
		if(flyReady)
        {

            if (firstTime)
            {
                // SKA BARA KÖRA EN GÅNG FFS    ¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤¤
                foreach (Transform child in transform.root.transform)
                {

                    
                    if (child.gameObject.name == "Character Parts (Sir Climb)(Clone)")
                    {
                        print("it's Character Parts (Sir Climb)(Clone)");
                        //test = child.gameObject;
                        test = child.gameObject.transform.Find("Root_M/spine/chest/neck/head").gameObject;
                    }

                    if (child.gameObject.name == "Character Parts (Mannequin)(Clone)")
                    {
                        print("it's Character Parts (Mannequin)(Clone)");
                        test = child.gameObject.transform.Find("Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Neck_M/NeckPart1_M/NeckPart2_M/Head_M").gameObject;

                    }

                    //if (LudvigTestList.Count != 0)
                    //{
                    //    return; //break; 
                    //}
                }

                rb = test.GetComponent<Rigidbody>(); // player.GetComponent<Rigidbody>();

                firstTime = false;
                print("first Time false");

            }
            
              
                rocketFuel += Time.deltaTime * 1;

                rb.AddForce(transform.up * rocketForce);
          
            

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
