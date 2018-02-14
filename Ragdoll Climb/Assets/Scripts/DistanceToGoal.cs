using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToGoal : MonoBehaviour {


    public Transform goal; // "FinishArea" GameObject
    public Transform cam;  // "MainCamera" GameObject
    public Text rangeText;

    float startPos;
    float distToEnd;
    float procentToEnd;


    void Start ()
    {
        // "startPos" is the camera position
        startPos = cam.position.y;
        distToEnd = 0;
    }
	
	void Update ()
    {
        distToEnd = goal.position.y - cam.position.y;

        // Calculating the dictance by procent
        Debug.Log("Distance from goal: " + distToEnd + "(start pos: " + startPos);
        procentToEnd = (cam.position.y - startPos ) / (goal.position.y - startPos ) ;
        rangeText.text = "Finish line: " + distToEnd;

        // When it reach zero
        if(distToEnd <= 0)
        {
            rangeText.text = "Finish line: ON SIGHT";
        }
        else
        {

        }
    }
}
