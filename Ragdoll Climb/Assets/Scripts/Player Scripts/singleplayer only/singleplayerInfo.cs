using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleplayerInfo : MonoBehaviour {

    public int stars;
    public float playtime;
    public bool onEnd;

	// Use this for initialization
	void Start () {
        stars = 0;
        playtime = 100; // ska vara time från singletonen 
        onEnd = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (!onEnd)
        {
            playtime = playtime - Time.deltaTime;
        }


        if(playtime <= 0)
        {
            //gameover and restart(probobly)
            print(" no more time ");

        }

	}
}
