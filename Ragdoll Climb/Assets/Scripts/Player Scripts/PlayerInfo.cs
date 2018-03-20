using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


public class PlayerInfo : MonoBehaviour
{
    // Text displaying what happens to the player
    public FeedbackText feedbackText;

    // If the player has collision with other players
    internal bool solid = true;

    internal int playerNr = 1;
    internal Color color;

    // The game pad index that controls this player
    internal PlayerIndex playerIndex;

    // The "Root_M" object of this player
    internal GameObject rootObj;

    // The masses of the rigidbodies from the very start
    internal List<float> standardMasses;

    // The mass values that the rigidbodies should have right now
    internal List<float> targetMasses;

    // Other players grabbing this player
    List<GameObject> grabbingPlayers = new List<GameObject>();


    private void Start()
    { 
        feedbackText.playerNr = playerNr;

        Rigidbody[] limbs = GetComponentsInChildren<Rigidbody>();

        standardMasses = new List<float>(limbs.Length);
        targetMasses = new List<float>(limbs.Length);

        for (int i = 0; i < limbs.Length; i++)
        {
			if (limbs[i].name == "Root_M")
			{
				feedbackText.playerTrans = limbs[i].transform;
				rootObj = limbs[i].gameObject;
			}

            standardMasses.Add(limbs[i].mass);
        }

        targetMasses = standardMasses;
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
