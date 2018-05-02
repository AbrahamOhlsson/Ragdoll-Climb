using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class singleplayerInfo : MonoBehaviour {

    public int stars;
    public float playtime;
    public float lvlTime;
    public bool onEnd;

    public Text TimerText;
   

	// Use this for initialization
	void Start () {
        stars = 0;
        playtime = lvlTime; // ska vara time från singletonen 
        onEnd = false;
       // testSlängSEn = GameObject.Find("Timer");
        TimerText = GameObject.Find("Debug Canvas/Timer").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if (playtime.ToString().Length>4)
            TimerText.text = "Time : " + playtime.ToString().Remove(4);
        else
            TimerText.text = "Time : " + playtime.ToString();


        if (!onEnd && playtime>0)
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
