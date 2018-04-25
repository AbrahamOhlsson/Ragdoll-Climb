using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class CharacterSelection_SP : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float joinBlinkInterval = 0.5f;
    
    [SerializeField] GameObject nextGroup;
    [SerializeField] GameObject playerModel;
    [SerializeField] GameObject joinText;
    [SerializeField] GameObject continueButton;

    [SerializeField] Color[] colors;
    [SerializeField] GameObject[] characterModels;

    [SerializeField] WorldMenuManager menuManager;

    [SerializeField] EventSystem eventSystem;

    bool joined = false;

    int colorIndex;
    int characterIndex;

    float joinBlinkTimer = 0f;

    List<Renderer> playerRenderers;

    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    GamePadState[] states_nonSpec = new GamePadState[4];
    GamePadState[] prevStates_nonSpec = new GamePadState[4];


    private void Awake()
    {
        playerRenderers = new List<Renderer>();
        
        playerRenderers = new List<Renderer>(playerModel.GetComponentsInChildren<Renderer>());
        colorIndex = 0;

        playerModel.SetActive(false);
        continueButton.SetActive(false);
    }


    void Update()
    {
        for (int i = 0; i < states_nonSpec.Length; i++)
        {
            prevStates_nonSpec[i] = states_nonSpec[i];
            states_nonSpec[i] = GamePad.GetState((PlayerIndex)i);

            // If an unjoined player presses Start
            if (states_nonSpec[i].Buttons.Start == ButtonState.Pressed && !joined)
            {
                // Adds controller index to list of joined player indexes
                playerIndex = (PlayerIndex)i;
                
                // Removes joun instructions
                joinText.SetActive(false);

                continueButton.SetActive(true);

                // Recolors character meshes to the found available color
                for (int j = 0; j < playerRenderers.Count; j++)
                {
                    playerRenderers[j].material.color = colors[colorIndex];
                }

                playerModel.SetActive(true);

                joined = true;
            }
        }

        // Input from joined controller
        if (joined)
        {
            prevState = state;
            state = GamePad.GetState(playerIndex);

            // Rotation of character
            playerModel.transform.Rotate(new Vector3(0f, state.ThumbSticks.Right.X, 0f) * rotateSpeed * Time.deltaTime);

            // Switching of color and character
            // Up on stick or D-pad
            if ((state.ThumbSticks.Left.Y >= 0.3f && prevState.ThumbSticks.Left.Y < 0.3f) || (state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released))
                SwitchColor(true);

            // Down on stick or D-pad
            else if ((state.ThumbSticks.Left.Y <= -0.3f && prevState.ThumbSticks.Left.Y > -0.3f) || (state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released))
                SwitchColor(false);

            // Right on stick or D-pad
            else if ((state.ThumbSticks.Left.X >= 0.3f && prevState.ThumbSticks.Left.X < 0.3f) || (state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released))
                SwitchCharacter(true);

            // Left on stick or D-pad
            else if ((state.ThumbSticks.Left.X <= -0.3f && prevState.ThumbSticks.Left.X > -0.3f) || (state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released))
                SwitchCharacter(false);


            // Continues if Start is pressed
            if (state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released)
                Continue();
        }
        else
        {
            // Blinks join texts
            if (joinBlinkTimer >= joinBlinkInterval)
            {
                if (joinText.activeSelf)
                    joinText.SetActive(false);
                else
                    joinText.SetActive(true);

                joinBlinkTimer = 0;
            }
            else
                joinBlinkTimer += Time.deltaTime;
        }
    }


    // Switches color of character model
    private void SwitchColor(bool next)
    {
        // Next color, if right was pressed
        if (next)
        {
            colorIndex++;

            // Loops back index to 0 if index is beyond length of array
            if (colorIndex >= colors.Length)
                colorIndex = 0;
        }
        // Previous color, if left was pressed
        else
        {
            colorIndex--;

            // Loops to end of array if index was less than 0
            if (colorIndex < 0)
                colorIndex = colors.Length - 1;
        }

        // Recolors all renderers of character model
        for (int i = 0; i < playerRenderers.Count; i++)
        {
            playerRenderers[i].material.color = colors[colorIndex];
        }
    }


    // Switch character model
    private void SwitchCharacter(bool next)
    {
        // Selects next character in array, if right was pressed
        if (next)
        {
            characterIndex++;

            // Loops back index to 0 if index is beyond length of array
            if (characterIndex >= characterModels.Length)
                characterIndex = 0;
        }
        // Selects previous character in array, if left was pressed
        else
        {
            characterIndex--;

            // Loops to end of array if index was less than 0
            if (characterIndex < 0)
                characterIndex = characterModels.Length - 1;
        }

        // Destroys last model
        Destroy(playerModel.transform.GetChild(0).gameObject);

        // Instantiates new model
        GameObject newModel = Instantiate(characterModels[characterIndex], playerModel.transform);

        // Gets all the new meshes
        playerRenderers = new List<Renderer>(newModel.GetComponentsInChildren<Renderer>());

        // Recolors new model
        for (int i = 0; i < playerRenderers.Count; i++)
        {
            playerRenderers[i].material.color = colors[colorIndex];
        }
    }


    public void Continue()
    {
        // Stores selected colors and characters of player
        PlayerInfoSingleton.instance.colors[0] = colors[colorIndex];
        PlayerInfoSingleton.instance.characterIndex[0] = characterIndex;

        // Stores controller indexes for player
        PlayerInfoSingleton.instance.playerIndexes = new List<PlayerIndex>();
        PlayerInfoSingleton.instance.playerIndexes.Add(playerIndex);

        // We are not in debug mode
        PlayerInfoSingleton.instance.debug = false;

        // Stores amount of joined players
        PlayerInfoSingleton.instance.playerAmount = 1;

        // Opens next menu group
        menuManager.OpenMenuGroup(nextGroup);
    }


    // Resets variables in case the lobby has been open before
    public void ResetValues()
    {
        // Destroys all models
        Destroy(playerModel.transform.GetChild(0).gameObject);

        // Spawns the first model in array
        GameObject newModel = Instantiate(characterModels[0], playerModel.transform);

        // Gets all the new renderers
        playerRenderers = new List<Renderer>(newModel.GetComponentsInChildren<Renderer>());

        // Recolors all meshes white
        for (int i = 0; i < playerRenderers.Count; i++)
        {
            playerRenderers[i].material.color = Color.white;
        }

        // Resets stuff for player
        characterIndex = 0;
        playerModel.SetActive(false);
        playerModel.transform.rotation = Quaternion.Euler(Vector3.zero);
        joinText.SetActive(true);
        joined = false;
        
        eventSystem.SetSelectedGameObject(null);
    }
}