using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class singleplayerInfo : MonoBehaviour {

    public int stars = 0;
    public float playtime = 0f;
    public bool onEnd = false;

    public Text TimerText;
   

	// Use this for initialization
	void Start ()
    {
        TimerText = GameObject.Find("Debug Canvas/Timer").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if (playtime.ToString().Length>4)
            TimerText.text = "Time : " + playtime.ToString().Remove(4);
        else
            TimerText.text = "Time : " + playtime.ToString();


        if (!onEnd)
        {
            playtime += Time.deltaTime;
        }
	}
}
