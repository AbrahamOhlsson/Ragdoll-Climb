using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class singleplayerInfo : MonoBehaviour {

    internal int stars = 0;
    internal float playtime = 0f;
    internal bool onEnd = false;
    internal bool started = false;

    internal Text TimerText;
   

	// Use this for initialization
	void Start ()
    {
        TimerText = GameObject.Find("Debug Canvas/Timer").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (playtime.ToString().Length>4)
            TimerText.text = "Time : " + playtime.ToString().Remove(4);
        else
            TimerText.text = "Time : " + playtime.ToString();
        
        if (!onEnd && started)
        {
            playtime += Time.deltaTime;
        }
	}
}
