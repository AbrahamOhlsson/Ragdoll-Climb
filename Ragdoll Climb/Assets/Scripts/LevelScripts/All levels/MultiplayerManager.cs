using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class MultiplayerManager : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();

    [SerializeField] GameObject[] characterParts;
    
    // The colors each of the players should have
    [SerializeField] Color[] playerColors;

    bool started = false;

    bool camActivated = false;

    bool[] playerSpawned = new bool[4];

    // States for four XInput game pads
    GamePadState[] states = new GamePadState[4];

    PlayerInfoSingleton singleton;
    

    void Awake()
    {
        singleton = PlayerInfoSingleton.instance;

        if (singleton.debug)
        {
            for (int i = 0; i < players.Count; i++)
            {
                Instantiate(characterParts[i], players[i].transform);
            }
        }
        else
        {
            for (int i = 0; i < players.Count; i++)
            {
                // CHANGE THIS LATER
                Instantiate(characterParts[singleton.characterIndex[i]], players[i].transform);
            }
        }
    }


    void Update ()
    {
        // If we aren't in debug mode (we came to this level from the menu)
        if (!singleton.debug && !started)
        {
            // Resets players so they can't move
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<PlayerController>().canMove = false;
            }

            // Goes through as many players that was ready in the menu
            for (int i = 0; i < singleton.playerAmount; i++)
            {
                players[i].SetActive(true);

                // Gets renderers of the player
                Renderer[] renderers = players[i].transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();

                // Recolors the player to the one it selected in the menu
                for (int j = 0; j < renderers.Length; j++)
                {
                    renderers[j].material.color = singleton.colors[i];
                }

                TrailRenderer[] trailRenderers = players[i].GetComponentsInChildren<TrailRenderer>();
                
                for (int j = 0; j < trailRenderers.Length; j++)
                {
                    trailRenderers[j].startColor = singleton.colors[i];
                }

                // Sets which controller that should control this player
                players[i].GetComponent<PlayerController>().SetGamePad(singleton.playerIndexes[i]);
                players[i].GetComponent<PlayerInfo>().playerIndex = singleton.playerIndexes[i];

                // Stores color and player nr i Player Info
                players[i].GetComponent<PlayerInfo>().color = singleton.colors[i];
                players[i].GetComponent<PlayerInfo>().playerNr = i + 1;

                // Recolors the player's Feedback Text to match the player
                players[i].GetComponent<PlayerInfo>().feedbackText.GetComponent<Text>().color = singleton.colors[i];
                
                players[i].GetComponent<DeathManager>().SetMats();
            }

            if (!camActivated)
            {
                Camera.main.GetComponent<CameraScript>().enabled = true;
                camActivated = true;
            }

            started = true;
        }

        // If we are in debug mode (we started the level directly from the editor)
        if (singleton.debug)
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
                    players[i].GetComponent<PlayerController>().SetGamePad((PlayerIndex)i);
                    players[i].GetComponent<PlayerInfo>().playerIndex = (PlayerIndex)i;
                    players[i].GetComponent<VibrationManager>().playerIndex = (PlayerIndex)i;
                    players[i].GetComponent<Cheats>().SetGamePad(i);

                    // Gets all renderers in player
                    Renderer[] renderers = players[i].transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();

                    // Changes color of all renderers
                    for (int j = 0; j < renderers.Length; j++)
                    {
                        renderers[j].material.color = playerColors[i];
                    }

                    TrailRenderer[] trailRenderers = players[i].GetComponentsInChildren<TrailRenderer>();
                    
                    for (int j = 0; j < trailRenderers.Length; j++)
                    {
                        //trailRenderers[j].colorGradient = gradient;
                        trailRenderers[j].startColor = playerColors[i];
                    }

                    print(i + 1);
                    players[i].GetComponent<PlayerInfo>().playerNr = i + 1;
                    players[i].GetComponent<PlayerInfo>().color = playerColors[i];
                    players[i].GetComponent<PlayerInfo>().feedbackText.GetComponent<Text>().color = playerColors[i];

                    players[i].GetComponent<DeathManager>().SetMats();

                    playerSpawned[i] = true;

					singleton.playerAmount++;

                    if (!camActivated)
                    {
                        Camera.main.GetComponent<CameraScript>().enabled = true;
                        camActivated = true;
                    }
                }
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
