using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTSLÄNGSEN : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("up"))
        {
            Debug.Log("UP");

            FindObjectOfType<musicAndSoundManager>().PlaySound("applaus");

        }

        if (Input.GetKeyDown("down"))
        {
            Debug.Log("DOWN");

           FindObjectOfType<feedbackManager>().PlaySound("ice",feedbackSound.player.Blue);

        }
    }
}
