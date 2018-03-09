using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class commentatorManager : MonoBehaviour {

    [SerializeField]
    List <GameObject> players;
    [Space]
    [SerializeField]
    AudioClip[] testClip;
    [Space]

    [SerializeField]
    float TimeBetweenLines = 0;
    float CdTime=0;

    [Space]
    [SerializeField]
    AudioSource audioSource;
    

    // Use this for initialization
    void Start () {
        // add players 
        // if (GameObject.Find("playercontroller") != null)
        //{

        for (int i = 0; i != 4; i++)/*GameObject.Find("playercontroller").size*/
        {
            print("Player " + (i+ 1)); 
            players.Add( GameObject.Find("Player " + (i + 1)));

            if(GameObject.Find("Player "+ (i + 1)) == null) { print("null dhakdhkhadka"); }
        }



        //}

	}
	
	// Update is called once per frame
	void Update () {
        CdTime += Time.deltaTime;

        if (CdTime == TimeBetweenLines)
        {
            PlayLine();

            CdTime = 0;

            CdTime -= audioSource.clip.length;
        }

	}

    void PlayLine()
    {
      //play a line  
      

    
    }



}
