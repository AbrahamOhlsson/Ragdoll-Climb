using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class Lobby : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float joinBlinkInterval = 0.5f;
    [SerializeField] float uiColorSat = 0.2f;

    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject nextGroup;

    [SerializeField] string[] characterNames;

    [SerializeField] GameObject[] playerModels = new GameObject[4];
    //[SerializeField] GameObject[] checkBoxes = new GameObject[4];
    [SerializeField] GameObject[] checkMarkers = new GameObject[4];
    [SerializeField] GameObject[] joinTexts = new GameObject[4];
    [SerializeField] GameObject[] instructions = new GameObject[4];

    [SerializeField] Image[] joinedPlayerImg = new Image[4];
    [SerializeField] Image[] playerSlotImg = new Image[4];
    [SerializeField] Image[] readyBoxImg = new Image[4];

    [SerializeField] Text[] nameTexts = new Text[4];

    [SerializeField] Color[] colors;
    [SerializeField] GameObject[] characterModels;

    [SerializeField] WorldMenuManager menuManager;

    [SerializeField] EventSystem eventSystem;

    internal bool canSwitchCharacter = false;

    bool canControl = true;

    bool allReady = false;

    bool[] colorTaken;

    bool[] playersReady = new bool[4];

    int[] colorIndexAssigned = new int[4];
    int[] characterIndexAssigned = new int[4];

    float joinBlinkTimer = 0f;

    Quaternion[] startRotations = new Quaternion[4];

    List<Renderer>[] playerRenderers;

    List<PlayerIndex> playerIndexes = new List<PlayerIndex>();
    GamePadState[] state = new GamePadState[4];
    GamePadState[] prevState = new GamePadState[4];

    GamePadState[] states_nonSpec = new GamePadState[4];
    GamePadState[] prevStates_nonSpec = new GamePadState[4];

    Singleton singleton;


    private void Awake()
    {
        singleton = Singleton.instance;

        colorTaken = new bool[colors.Length];

        playerRenderers = new List<Renderer>[4];

        for (int i = 0; i < playerModels.Length; i++)
        {
            playerRenderers[i] = new List<Renderer>(playerModels[i].GetComponentsInChildren<Renderer>());
            colorIndexAssigned[i] = 0;
            startRotations[i] = playerModels[i].transform.rotation;
        }

        if (singleton.mode == Singleton.Modes.Multi)
        {
            ResetValues(false);

            playerIndexes = singleton.playerIndexes;

            for (int i = 0; i < playerIndexes.Count; i++)
            {
                colorIndexAssigned = singleton.colorindex;
                characterIndexAssigned[i] = singleton.characterIndex[i];
                colorTaken[colorIndexAssigned[i]] = true;

                // Destroys last model
                Destroy(playerModels[i].transform.GetChild(0).gameObject);
                
                // Instantiates new model
                GameObject newModel = Instantiate(characterModels[characterIndexAssigned[i]], playerModels[i].transform);
                
                // Gets all the new meshes
                playerRenderers[i] = new List<Renderer>(newModel.GetComponentsInChildren<Renderer>());

                // Recolors new model
                for (int j = 0; j < playerRenderers[i].Count; j++)
                {
                    playerRenderers[i][j].material.color = colors[colorIndexAssigned[i]];
                }

                Color uiColor = AddHSV(colors[colorIndexAssigned[i]], 0f, uiColorSat - 1, 1f);
                joinedPlayerImg[i].color = uiColor;
                playerSlotImg[i].color = uiColor;
                readyBoxImg[i].color = uiColor;

                nameTexts[i].text = characterNames[characterIndexAssigned[i]];

                playerModels[i].SetActive(true);
                instructions[i].SetActive(true);
                joinTexts[i].SetActive(false);
            }
        }
    }

	
    void Update ()
    {
        if (canControl)
        {
            for (int i = 0; i < states_nonSpec.Length; i++)
            {
                prevStates_nonSpec[i] = states_nonSpec[i];
                states_nonSpec[i] = GamePad.GetState((PlayerIndex)i);

                // If an unjoined player presses Start
                if (states_nonSpec[i].Buttons.Start == ButtonState.Pressed && !playerIndexes.Contains((PlayerIndex)i))
                {
                    // Adds controller index to list of joined player indexes
                    playerIndexes.Add((PlayerIndex)i);
                    int addedIndex = playerIndexes.Count - 1;

                    // In case players were ready when a new joined
                    allReady = false;
                    continueButton.SetActive(false);

                    // Swaps instructions
                    joinTexts[addedIndex].SetActive(false);
                    //checkBoxes[addedIndex].SetActive(true);
                    instructions[addedIndex].SetActive(true);

                    int colorIndex = 0;

                    // Searches for an available color
                    for (int j = 0; j < colorTaken.Length; j++)
                    {
                        if (!colorTaken[j])
                        {
                            colorIndex = j;
                            colorIndexAssigned[addedIndex] = j;
                            colorTaken[j] = true;
                            break;
                        }
                    }

                    // Recolors character meshes to the found available color
                    for (int j = 0; j < playerRenderers[addedIndex].Count; j++)
                    {
                        playerRenderers[addedIndex][j].material.color = colors[colorIndex];
                    }

                    Color uiColor = AddHSV(colors[colorIndex], 0f, uiColorSat - 1, 1f);
                    joinedPlayerImg[addedIndex].color = uiColor;
                    playerSlotImg[addedIndex].color = uiColor;
                    readyBoxImg[addedIndex].color = uiColor;

                    playerModels[addedIndex].SetActive(true);
                }
            }

            // Input from joines players
            for (int i = 0; i < playerIndexes.Count; i++)
            {
                prevState[i] = state[i];
                state[i] = GamePad.GetState(playerIndexes[i]);

                // Rotation of character
                playerModels[i].transform.Rotate(new Vector3(0f, state[i].ThumbSticks.Right.X, 0f) * rotateSpeed * Time.deltaTime);

                // Switching of color and character
                if (!playersReady[i])
                {
                    // Up on stick or D-pad
                    if ((state[i].ThumbSticks.Left.Y >= 0.3f && prevState[i].ThumbSticks.Left.Y < 0.3f) || (state[i].DPad.Up == ButtonState.Pressed && prevState[i].DPad.Up == ButtonState.Released))
                    {
                        SwitchColor(i, true);
                    }
                    // Down on stick or D-pad
                    else if ((state[i].ThumbSticks.Left.Y <= -0.3f && prevState[i].ThumbSticks.Left.Y > -0.3f) || (state[i].DPad.Down == ButtonState.Pressed && prevState[i].DPad.Down == ButtonState.Released))
                    {
                        SwitchColor(i, false);
                    }
                    // Right on stick or D-pad
                    else if ((state[i].ThumbSticks.Left.X >= 0.3f && prevState[i].ThumbSticks.Left.X < 0.3f) || (state[i].DPad.Right == ButtonState.Pressed && prevState[i].DPad.Right == ButtonState.Released))
                    {
                        SwitchCharacter(i, true);
                    }
                    // Left on stick or D-pad
                    else if ((state[i].ThumbSticks.Left.X <= -0.3f && prevState[i].ThumbSticks.Left.X > -0.3f) || (state[i].DPad.Left == ButtonState.Pressed && prevState[i].DPad.Left == ButtonState.Released))
                    {
                        SwitchCharacter(i, false);
                    }
                }

                if (state[i].Buttons.A == ButtonState.Pressed && prevState[i].Buttons.A == ButtonState.Released)
                {
                    // Unreadies player
                    if (playersReady[i])
                    {
                        playersReady[i] = false;
                        checkMarkers[i].SetActive(false);
                        instructions[i].SetActive(true);

                        // Everyone isn't ready anymore in case they were
                        allReady = false;
                        continueButton.SetActive(false);
                    }
                    // Readies player
                    else
                    {
                        playersReady[i] = true;
                        checkMarkers[i].SetActive(true);
                        instructions[i].SetActive(false);

                        // Checks if the other players are ready too
                        for (int j = 0; j < playerIndexes.Count; j++)
                        {
                            // Someone wasn't ready, abort check
                            if (!playersReady[j])
                            {
                                break;
                            }
                            // If the last player is ready and more than two has joined
                            else if (playersReady[i] && j == playerIndexes.Count - 1 && playerIndexes.Count >= 1)
                            {
                                // Since we got to the last player and he was ready, the check never got aborted which measn the otehrs were ready too
                                allReady = true;
                                continueButton.SetActive(true);
                            }
                        }
                    }
                }

                // Continues if any player presses Start when all are ready
                if (state[i].Buttons.Start == ButtonState.Pressed && prevState[i].Buttons.Start == ButtonState.Released && allReady)
                {
                    Continue();
                }
            }

            // Blinks join texts
            if (joinBlinkTimer >= joinBlinkInterval)
            {
                for (int i = 0; i < joinTexts.Length; i++)
                {
                    if (i > playerIndexes.Count - 1)
                    {
                        if (joinTexts[i].transform.GetChild(0).gameObject.activeSelf)
                            joinTexts[i].transform.GetChild(0).gameObject.SetActive(false);
                        else
                            joinTexts[i].transform.GetChild(0).gameObject.SetActive(true);
                    }
                }

                joinBlinkTimer = 0;
            }
            else
                joinBlinkTimer += Time.deltaTime;
        }
	}


    // Switches color of character model
    private void SwitchColor(int playerIndex, bool next)
    {
        // Index of previously selected color
        int initialIndex = colorIndexAssigned[playerIndex];

        // Searches for available color
        while (colorTaken[colorIndexAssigned[playerIndex]])
        {
            // Next color, if right was pressed
            if (next)
            {
                colorIndexAssigned[playerIndex]++;

                // Loops back index to 0 if index is beyond length of array
                if (colorIndexAssigned[playerIndex] >= colorTaken.Length)
                    colorIndexAssigned[playerIndex] = 0;
            }
            // Previous color, if left was pressed
            else
            {
                colorIndexAssigned[playerIndex]--;

                // Loops to end of array if index was less than 0
                if (colorIndexAssigned[playerIndex] < 0)
                    colorIndexAssigned[playerIndex] = colorTaken.Length - 1;
            }
        }

        // Recolors all renderers of character model
        for (int i = 0; i < playerRenderers[playerIndex].Count; i++)
        {
            playerRenderers[playerIndex][i].material.color = colors[colorIndexAssigned[playerIndex]];
        }

        Color uiColor = AddHSV(colors[colorIndexAssigned[playerIndex]], 0f, uiColorSat - 1, 1f);
        joinedPlayerImg[playerIndex].color = uiColor;
        playerSlotImg[playerIndex].color = uiColor;
        readyBoxImg[playerIndex].color = uiColor;

        // The new color is now taken and can't be assigned to other players
        colorTaken[colorIndexAssigned[playerIndex]] = true;

        // The previously selected color can now be assigned to other players
        colorTaken[initialIndex] = false;
    }


    // Switch character model
    private void SwitchCharacter(int playerIndex, bool next)
    {
        if (canSwitchCharacter)
        {
            // Selects next character in array, if right was pressed
            if (next)
            {
                characterIndexAssigned[playerIndex]++;

                // Loops back index to 0 if index is beyond length of array
                if (characterIndexAssigned[playerIndex] >= characterModels.Length)
                    characterIndexAssigned[playerIndex] = 0;
            }
            // Selects previous character in array, if left was pressed
            else
            {
                characterIndexAssigned[playerIndex]--;

                // Loops to end of array if index was less than 0
                if (characterIndexAssigned[playerIndex] < 0)
                    characterIndexAssigned[playerIndex] = characterModels.Length - 1;
            }

            // Destroys last model
            Destroy(playerModels[playerIndex].transform.GetChild(0).gameObject);

            // Instantiates new model
            GameObject newModel = Instantiate(characterModels[characterIndexAssigned[playerIndex]], playerModels[playerIndex].transform);

            // Gets all the new meshes
            playerRenderers[playerIndex] = new List<Renderer>(newModel.GetComponentsInChildren<Renderer>());

            // Recolors new model
            for (int i = 0; i < playerRenderers[playerIndex].Count; i++)
            {
                playerRenderers[playerIndex][i].material.color = colors[colorIndexAssigned[playerIndex]];
            }

            nameTexts[playerIndex].text = characterNames[characterIndexAssigned[playerIndex]];
        }
    }


    private Color AddHSV(Color color, float h, float s, float v)
    {
        float _h, _s, _v;
        Color.RGBToHSV(color, out _h, out _s, out _v);
        _h = Mathf.Clamp(_h + h, 0f, 1f);
        _s = Mathf.Clamp(_s + s, 0f, 1f);
        if (_s > 0f)
            _v = Mathf.Clamp(_v + v, 0f, 1f);
        return Color.HSVToRGB(_h, _s, _v);
    }


    public void Continue()
    {
        // Stores selected colors and characters of all players
        for (int i = 0; i < 4; i++)
        {
            singleton.colorindex[i] = colorIndexAssigned[i];
            singleton.colors[i] = colors[colorIndexAssigned[i]];
            singleton.characterIndex[i] = characterIndexAssigned[i];
        }

        // Stores controller indexes for each player
        singleton.playerIndexes = playerIndexes;

        // We are not in debug mode
        singleton.debug = false;

        // Stores amount of joined players
        singleton.playerAmount = playerIndexes.Count;

        // Opens next menu group
		menuManager.OpenMenuGroup(nextGroup);

        // Resets the ready values in case the players would return from next menu group
		for (int i = 0; i < playerIndexes.Count; i++)
		{
			checkMarkers[i].SetActive(false);
            instructions[i].SetActive(true);
			playersReady[i] = false;
		}
		continueButton.SetActive(false);
		allReady = false;

        canControl = false;
	}

    
    // Resets variables in case the lobby has been open before
    public void ResetValues(bool changeModel)
    {
        for (int i = 0; i < colorTaken.Length; i++)
        {
            colorTaken[i] = false;
        }

        // For every potential player
        for (int i = 0; i < 4; i++)
        {
            if (changeModel)
            {
                // Destroys all models
                Destroy(playerModels[i].transform.GetChild(0).gameObject);

                // Spawns the first model in array
                GameObject newModel = Instantiate(characterModels[0], playerModels[i].transform);

                // Gets all the new renderers
                playerRenderers[i] = new List<Renderer>(newModel.GetComponentsInChildren<Renderer>());
            }
            
            // Recolors all meshes white
            for (int j = 0; j < playerRenderers[i].Count; j++)
            {
                playerRenderers[i][j].material.color = Color.white;
            }

            joinedPlayerImg[i].color = Color.white;
            playerSlotImg[i].color = Color.white;
            readyBoxImg[i].color = Color.white;
            
            // Resets stuff for player
            characterIndexAssigned[i] = 0;
            playerModels[i].SetActive(false);
            playerModels[i].transform.rotation = startRotations[i];
            checkMarkers[i].SetActive(false);
            //checkBoxes[i].SetActive(false);
            joinTexts[i].SetActive(true);
            joinTexts[i].transform.GetChild(0).gameObject.SetActive(true);
            instructions[i].SetActive(false);
            playersReady[i] = false;

            nameTexts[i].text = characterNames[characterIndexAssigned[i]];
        }

        continueButton.SetActive(false);
        playerIndexes.Clear();
        allReady = false;

        eventSystem.SetSelectedGameObject(null);
    }

    private void OnEnable()
    {
        canControl = true;
    }
}