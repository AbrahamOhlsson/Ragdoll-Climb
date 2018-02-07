using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour {





    private string winnerText;




    void OnTriggerEnter(Collider winner)
    {
        Debug.Log("Trigger enter");
        if (winner.gameObject.tag == "playerOne")
        {
            Debug.Log("Blue player wins!!");
            winnerText = "Blue player Wins";
        }

        else if (winner.gameObject.tag == "playerTwo")
        {

            Debug.Log("Red player wins");
            winnerText = "Red player Wins";
        }


    }





    void OnGUI()
    {
  
            GUI.color = Color.black;
            GUI.Label(new Rect((Screen.width - 10) / 2, (Screen.height - 200) / 2, 100, 30), winnerText); // keep the text size at (18p) for this code
            //GUI.Label(new Rect((Screen.width - 100) / 2, (Screen.height - 30) / 2, 100, 30), countdownText);


        


    }





}
