using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


public class PlayerInfo : MonoBehaviour
{
    // Text displaying what happens to the player
    public FeedbackText feedbackText;

    [HideInInspector] public int playerNr = 1;
    [HideInInspector] public Color color;

    // The game pad index that controls this player
    [HideInInspector] public PlayerIndex playerIndex;

    // The "Root_M" object of this player
	[HideInInspector] public GameObject rootObj;
    
    // Other players grabbing this player
    List<GameObject> grabbingPlayers = new List<GameObject>();


    private void Start()
    { 
        feedbackText.playerNr = playerNr;

        Rigidbody[] limbs = GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < limbs.Length; i++)
        {
			if (limbs[i].name == "Root_M")
			{
				feedbackText.playerTrans = limbs[i].transform;
				rootObj = limbs[i].gameObject;
			}

			break;
        }
    }


    public void AddGrabbingPlayer(GameObject grabbingPlayer)
    {
        grabbingPlayers.Add(grabbingPlayer);
    }


    public void RemoveGrabbingPlayer(GameObject grabbingPlayer)
    {
        grabbingPlayers.Remove(grabbingPlayer);
    }


    public List<GameObject> GetGrabbingPlayers()
    {
        return grabbingPlayers;
    }


    public void DisconnectGrabbingPlayers()
    {
        if (grabbingPlayers.Count > 0)
        {
            for (int i = 0; i < grabbingPlayers.Count; i++)
            {
                grabbingPlayers[i].GetComponent<PlayerController>().ReleaseGrip(true, false);
                grabbingPlayers[i].GetComponent<PlayerController>().ReleaseGrip(false, false);
            }
        }
    }
}
