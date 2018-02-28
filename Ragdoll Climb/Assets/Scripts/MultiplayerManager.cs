using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class MultiplayerManager : MonoBehaviour
{
    // The colors each of the players should have
    [SerializeField] Color[] playerColors;

    [SerializeField] List<GameObject> players = new List<GameObject>();

    bool[] playerSpawned = new bool[4];

    // States for four XInput game pads
    GamePadState[] states = new GamePadState[4];
    

    void Start()
    {
        for (int i = 0; i < players.Count; i++)
        {
            //players[i].GetComponent<PlayerController>().enabled = true;
            players[i].GetComponent<PlayerController>().canMove = false;
        }
    }


    void Update ()
    {
        // Gets states for all game pads
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = GamePad.GetState((PlayerIndex)i);
        }

        for (int i = 0; i < players.Count; i++)
        {
            // If a player presses 'Start' and hasn't already spawned
            if (states[i].Buttons.Start == ButtonState.Pressed && !playerSpawned[i])
            {
                // Activates player and gives it the right player number
                players[i].SetActive(true);
                players[i].GetComponent<PlayerController>().playerNr = i + 1;
                players[i].GetComponent<PlayerController>().SetGamePad(i);
                players[i].GetComponent<Cheats>().SetGamePad(i);
                
                // Gets all renderers in player
                Renderer[] renderers = players[i].transform.GetChild(0).GetComponentsInChildren<Renderer>();

                // Changes color of all renderers
                for (int j = 0; j < renderers.Length; j++)
                {
                    renderers[j].material.color = playerColors[i];
                }

                playerSpawned[i] = true;
            }
        }
    }


    public void ActivatePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            //players[i].GetComponent<PlayerController>().enabled = true;
            players[i].GetComponent<PlayerController>().canMove = true;
        }
    }
}
