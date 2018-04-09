using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class Lobby : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float joinBlinkInterval = 0.5f;

    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject levelSelectGroup;
    [SerializeField] GameObject[] playerModels = new GameObject[4];
    [SerializeField] GameObject[] checkBoxes = new GameObject[4];
    [SerializeField] GameObject[] checkMarkers = new GameObject[4];
    [SerializeField] GameObject[] joinTexts = new GameObject[4];

    [SerializeField] Color[] colors;
    [SerializeField] GameObject[] characterModels;

    [SerializeField] WorldMenuManager menuManager;

    [SerializeField] EventSystem eventSystem;

    bool allReady = false;

    bool[] colorTaken;

    bool[] playersReady = new bool[4];

    int[] colorIndexAssigned = new int[4];
    int[] characterIndexAssigned = new int[4];

    float joinBlinkTimer = 0f;

    List<Renderer>[] playerRenderers;

    List<PlayerIndex> playerIndexes = new List<PlayerIndex>();
    GamePadState[] state = new GamePadState[4];
    GamePadState[] prevState = new GamePadState[4];

    GamePadState[] states_nonSpec = new GamePadState[4];
    GamePadState[] prevStates_nonSpec = new GamePadState[4];


    private void Awake()
    {
        colorTaken = new bool[colors.Length];

        playerRenderers = new List<Renderer>[4];

        for (int i = 0; i < playerModels.Length; i++)
        {
            playerRenderers[i] = new List<Renderer>(playerModels[i].GetComponentsInChildren<Renderer>());
            colorIndexAssigned[i] = 0;
        }
    }

	
    void Update ()
    {
		for (int i = 0; i < states_nonSpec.Length; i++)
        {
            prevStates_nonSpec[i] = states_nonSpec[i];
            states_nonSpec[i] = GamePad.GetState((PlayerIndex)i);

            if (states_nonSpec[i].Buttons.Start == ButtonState.Pressed && !playerIndexes.Contains((PlayerIndex)i))
            {
                playerIndexes.Add((PlayerIndex)i);
                int addedIndex = playerIndexes.Count - 1;

                allReady = false;
                continueButton.SetActive(false);
                joinTexts[addedIndex].SetActive(false);
                checkBoxes[addedIndex].SetActive(true);

                int colorIndex = 0;

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

                for (int j = 0; j < playerRenderers[i].Count; j++)
                {
                    playerRenderers[addedIndex][j].material.color = colors[colorIndex];
                }

                playerModels[addedIndex].SetActive(true);
            }
        }

        for (int i = 0; i < playerIndexes.Count; i++)
        {
            prevState[i] = state[i];
            state[i] = GamePad.GetState(playerIndexes[i]);

            playerModels[i].transform.Rotate(new Vector3(0f, state[i].ThumbSticks.Right.X, 0f) * rotateSpeed * Time.deltaTime);

            if (!playersReady[i])
            {
                if ((state[i].ThumbSticks.Left.Y >= 0.3f && prevState[i].ThumbSticks.Left.Y < 0.3f) || (state[i].DPad.Up == ButtonState.Pressed && prevState[i].DPad.Up == ButtonState.Released))
                {
                    SwitchColor(i, true);
                }
                else if ((state[i].ThumbSticks.Left.Y <= -0.3f && prevState[i].ThumbSticks.Left.Y > -0.3f) || (state[i].DPad.Down == ButtonState.Pressed && prevState[i].DPad.Down == ButtonState.Released))
                {
                    SwitchColor(i, false);
                }
                else if ((state[i].ThumbSticks.Left.X >= 0.3f && prevState[i].ThumbSticks.Left.X < 0.3f) || (state[i].DPad.Right == ButtonState.Pressed && prevState[i].DPad.Right == ButtonState.Released))
                {
                    SwitchCharacter(i, true);
                }
                else if ((state[i].ThumbSticks.Left.X <= -0.3f && prevState[i].ThumbSticks.Left.X > -0.3f) || (state[i].DPad.Left == ButtonState.Pressed && prevState[i].DPad.Left == ButtonState.Released))
                {
                    SwitchCharacter(i, false);
                }
            }
            
            if (state[i].Buttons.A == ButtonState.Pressed && prevState[i].Buttons.A == ButtonState.Released)
            {
                if (playersReady[i])
                {
                    playersReady[i] = false;
                    allReady = false;
                    continueButton.SetActive(false);

                    checkMarkers[i].SetActive(false);
                }
                else
                {
                    playersReady[i] = true;

                    checkMarkers[i].SetActive(true);

                    for (int j = 0; j < playerIndexes.Count; j++)
                    {
                        if (!playersReady[j])
                        {
                            break;
                        }
                        else if (playersReady[i] && j == playerIndexes.Count - 1 && playerIndexes.Count >= 2)
                        {
                            allReady = true;
                            continueButton.SetActive(true);
                        }
                    }
                }
            }

            if (state[i].Buttons.Start == ButtonState.Pressed && prevState[i].Buttons.Start == ButtonState.Released && allReady)
            {
                Continue();
            }
        }

        if (joinBlinkTimer >= joinBlinkInterval)
        {
            for (int i = 0; i < joinTexts.Length; i++)
            {
                if (i > playerIndexes.Count - 1)
                {
                    if (joinTexts[i].activeSelf)
                        joinTexts[i].SetActive(false);
                    else
                        joinTexts[i].SetActive(true);
                }
            }

            joinBlinkTimer = 0;
        }
        else
            joinBlinkTimer += Time.deltaTime;

	}


    private void SwitchColor(int playerIndex, bool next)
    {
        int initialIndex = colorIndexAssigned[playerIndex];

        while (colorTaken[colorIndexAssigned[playerIndex]])
        {
            if (next)
            {
                colorIndexAssigned[playerIndex]++;

                if (colorIndexAssigned[playerIndex] >= colorTaken.Length)
                    colorIndexAssigned[playerIndex] = 0;
            }
            else
            {
                colorIndexAssigned[playerIndex]--;

                if (colorIndexAssigned[playerIndex] < 0)
                    colorIndexAssigned[playerIndex] = colorTaken.Length - 1;
            }
        }

        for (int i = 0; i < playerRenderers[playerIndex].Count; i++)
        {
            //print(playerRenderers[playerIndex].Count);
            playerRenderers[playerIndex][i].material.color = colors[colorIndexAssigned[playerIndex]];
        }

        colorTaken[colorIndexAssigned[playerIndex]] = true;
        colorTaken[initialIndex] = false;
    }


    private void SwitchCharacter(int playerIndex, bool next)
    {
        if (next)
        {
            characterIndexAssigned[playerIndex]++;

            if (characterIndexAssigned[playerIndex] >= characterModels.Length)
                characterIndexAssigned[playerIndex] = 0;
        }
        else
        {
            characterIndexAssigned[playerIndex]--;

            if (characterIndexAssigned[playerIndex] < 0)
                characterIndexAssigned[playerIndex] = characterModels.Length - 1;
        }

        Destroy(playerModels[playerIndex].transform.GetChild(0).gameObject);
        GameObject newModel = Instantiate(characterModels[characterIndexAssigned[playerIndex]], playerModels[playerIndex].transform);
        playerRenderers[playerIndex] = new List<Renderer>(newModel.GetComponentsInChildren<Renderer>());

        for (int i = 0; i < playerRenderers[playerIndex].Count; i++)
        {
            playerRenderers[playerIndex][i].material.color = colors[colorIndexAssigned[playerIndex]];
        }
    }


    public void Continue()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerInfoSingleton.instance.colors[i] = colors[colorIndexAssigned[i]];
            PlayerInfoSingleton.instance.characterIndex[i] = characterIndexAssigned[i];
        }

        PlayerInfoSingleton.instance.playerIndexes = playerIndexes;
        PlayerInfoSingleton.instance.debug = false;
		PlayerInfoSingleton.instance.playerAmount = playerIndexes.Count;

		menuManager.OpenMenuGroup(levelSelectGroup);

		for (int i = 0; i < 4; i++)
		{
			checkMarkers[i].SetActive(false);
			playersReady[i] = false;
		}

		continueButton.SetActive(false);
		allReady = false;
	}

    public void ResetValues()
    {
        // Resets variables in case the lobby has been open before

        for (int i = 0; i < colorTaken.Length; i++)
        {
            colorTaken[i] = false;
        }

        for (int i = 0; i < 4; i++)
        {
            Destroy(playerModels[i].transform.GetChild(0).gameObject);
            GameObject newModel = Instantiate(characterModels[0], playerModels[i].transform);
            playerRenderers[i] = new List<Renderer>(newModel.GetComponentsInChildren<Renderer>());

            for (int j = 0; j < playerRenderers[i].Count; j++)
            {
                playerRenderers[i][j].material.color = Color.white;
            }

            characterIndexAssigned[i] = 0;
            playerModels[i].SetActive(false);
            playerModels[i].transform.rotation = Quaternion.Euler(Vector3.zero);
            checkMarkers[i].SetActive(false);
            checkBoxes[i].SetActive(false);
            joinTexts[i].SetActive(true);
            playersReady[i] = false;
        }

        continueButton.SetActive(false);

        playerIndexes.Clear();

        allReady = false;

        eventSystem.SetSelectedGameObject(null);
    }
}