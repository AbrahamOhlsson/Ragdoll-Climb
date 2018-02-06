using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownStart : MonoBehaviour {


    string countdownText = "3";
    bool endOfCountdown = false;
    public int myTime= 0;
    public float cdTime = 0;
    public Font f;
    public int fontSize;


    void Update()
    {
        if (!endOfCountdown)
        {
            cdTime += Time.deltaTime * 1;
        }

        if (cdTime > 1)
        {
            myTime++;
            cdTime = 0;
        }


        if (endOfCountdown) { }


        if(myTime == 1)
        {
            countdownText = "2";
            Debug.Log("in my time == 1");
        }

        else if (myTime == 2)
        {
           countdownText = "1";

        }

        else if (myTime == 3)
        {
            countdownText = "CLIMB!!!";

        }

        else if (myTime == 4)
        {
            countdownText = "";
            endOfCountdown = true;

            Destroy(this);  // Remove this script(change?)
        }

        //Debug.Log(myTime);

    }
    
    void OnGUI()
    {
        if (countdownText != "" )
        {
            GUI.skin.font = f;
            GUI.skin.label.fontSize = fontSize;
            GUI.color = Color.black;
            GUI.skin.font = f;
            GUI.Label(new Rect((Screen.width - 10) / 2, (Screen.height - 200) / 2, 100, 30), countdownText); // keep the text size at (18p) for this code
            //GUI.Label(new Rect((Screen.width - 100) / 2, (Screen.height - 30) / 2, 100, 30), countdownText);

        }
    }
    
 
      
    
    
}
