using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInfo : MonoBehaviour
{
    List<GameObject> grabbingPlayers = new List<GameObject>();


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
