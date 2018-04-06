using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToGoal : MonoBehaviour
{    
    public Text rangeText;
    public List<GameObject> players; // max 4 players(Root_M)
    public GameObject highestPlayer;
    public Rigidbody highestLimb;
    
    private Rigidbody[] limbs;
    private Transform target; 
    private float distToEnd;

    
    void Start ()
    {
        // "startPos" is the camera position
        //startPos = target.position.y;
        distToEnd = 0;
        
		for(int i= 1; i<5; i++)
		{ 
		    GameObject playerTemp = GameObject.Find("Player "+ i);

			if (playerTemp != null)
			{
				GameObject playerRoot = playerTemp.GetComponent<PlayerInfo>().rootObj;

				if (playerRoot != null)
				{
					players.Add(playerRoot);
				}
			}
		}

        rangeText = GameObject.Find("RangeText").GetComponent<Text>();

		highestPlayer = players[0];
	}
	
	void Update ()
    {
		if (limbs == null)
		{
			limbs = highestPlayer.GetComponentsInChildren<Rigidbody>();

			highestLimb = limbs[0];
		}

		
        // Checks everytime among the players who is the highest y-position(First in the race) 
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].transform.position.y > highestPlayer.transform.position.y)
            {
                highestPlayer = players[i];
            }
        }

        // Gets all the rigidbody components from the highest player; and becomes "limps" varible.
        limbs = highestPlayer.GetComponentsInChildren<Rigidbody>();

        // Checks everytime of the "higestplayer" limbs wich is the highest y-position(First in the race) 
        for (int i = 0; i < limbs.Length; i++)
        {
			if (limbs[i].transform.position.y > highestLimb.transform.position.y && limbs[i].tag == "Player")
            {
                highestLimb = limbs[i];
			}
        }

        // "highestLimp" transform becomes "target" 
        target = highestLimb.transform;

        // (54)Checks "target" y-position value; Becomes "distToEnd". - (55) removes the last decimal of "rangeText".
        distToEnd = transform.position.y - target.position.y;
        distToEnd = Mathf.Floor(distToEnd);

        // ****LOST GENARATION****
        // Calculating the dictance by percent
        //Debug.Log("Distance from goal: " + distToEnd + "(start pos: " + startPos);
        //procentToEnd = (target.position.y - startPos ) / (goal.position.y - startPos ) ;
        // ****LOST GENERATION **END**

        // Shows the remaining distance of the highest player(First in the race)
        rangeText.text = "Goal Distance: " + distToEnd.ToString("F0") /*+ " units"*/;

        // When it reach Finish Line
        if(distToEnd <= 0)
        {
            rangeText.text = "GOAL";
        }
    }
}
