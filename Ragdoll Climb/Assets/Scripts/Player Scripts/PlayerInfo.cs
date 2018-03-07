using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInfo : MonoBehaviour
{
    public FeedbackText feedbackText;

    [HideInInspector] public int playerNr = 1;
    [HideInInspector] public Color color;
    
    // Other players grabbing this player
    List<GameObject> grabbingPlayers = new List<GameObject>();


    private void Start()
    { 
        feedbackText.playerNr = playerNr;

        Rigidbody[] limbs = GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < limbs.Length; i++)
        {
            if (limbs[i].name == "Root_M")
                feedbackText.playerTrans = limbs[i].transform;
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
