using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezePlayer : MonoBehaviour {

    GameObject PlayerFreeze;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter(Collider other)
    {
        PlayerFreeze = other.transform.root.gameObject;

        if (PlayerFreeze.tag == "Player")
        {
            TimeToFreeze();
            Destroy(gameObject);
        }
        
    }


    void TimeToFreeze()
    {
        PlayerFreeze.GetComponent<FreezePlayerPowerUp>().FreezeTime();
    }
}
