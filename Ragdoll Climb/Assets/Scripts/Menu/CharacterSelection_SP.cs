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

    internal PlayerIndex playerIndex = PlayerIndex.One;
    GamePadState state;
    GamePadState prevState;

    GamePadState[] states_nonSpec = new GamePadState[4];
    GamePadState[] prevStates_nonSpec = new GamePadState[4];

    Singleton singleton;


    private void Awake()
    {
        singleton = Singleton.instance;

        playerRenderers = new List<Renderer>();
        playerRenderers = new List<Renderer>(playerModel.GetComponentsInChildren<Renderer>());
        colorIndex = 0;

        if (singleton.mode == Singleton.Modes.Single)
        {
            ResetValues(false);

            playerIndex = singleton.playerIndexes[0];
            colorIndex = singleton.colorindex[0];
            characterIndex = singleton.characterIndex[0];

            // Destroys last model
            Destroy(playerModel.transform.GetChild(0).gameObject);

            // Instantiates new model
            GameObject newModel = Instantiate(characterModels[characterIndex], playerModel.transform);
            
            playerModel.SetActive(true);
            joinText.SetActive(false);
            continueButton.SetActive(true);
            joined = true;
        }

        // Gets all the new meshes
        playerRenderers = new List<Renderer>(playerModel.GetComponentsInChildren<Renderer>());

        // Recolors new model
        for (int i = 0; i < playerRenderers.Count; i++)
        {
            playerRenderers[i].material.color = colors[colorIndex];
        }
    }


    void Update()
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
        singleton.colorindex[0] = colorIndex;
        singleton.colors[0] = colors[colorIndex];
        singleton.characterIndex[0] = characterIndex;

        // Stores controller indexes for player
        singleton.playerIndexes = new List<PlayerIndex>();
        singleton.playerIndexes.Add(playerIndex);

        // We are not in debug mode
        singleton.debug = false;

        // Stores amount of joined players
        singleton.playerAmount = 1;

        // Opens next menu group
        menuManager.OpenMenuGroup(nextGroup);
    }


    // Resets variables in case the lobby has been open before
    public void ResetValues(bool changeModel)
    {
        if (changeModel)
        {
            // Destroys all models
            Destroy(playerModel.transform.GetChild(0).gameObject);

            // Spawns the first model in array
            GameObject newModel = Instantiate(characterModels[0], playerModel.transform);

            // Gets all the new renderers
            playerRenderers = new List<Renderer>(newModel.GetComponentsInChildren<Renderer>());
        }

        // Recolors all meshes white
        for (int i = 0; i < playerRenderers.Count; i++)
        {
            playerRenderers[i].material.color = colors[0];
        }

        // Resets stuff for player
        characterIndex = 0;
        playerModel.transform.localEulerAngles = Vector3.zero;
        
        eventSystem.SetSelectedGameObject(null);
    }
}