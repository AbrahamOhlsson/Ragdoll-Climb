using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class CharacterSelection_SP : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float uiColorSat = 0.2f;

    [SerializeField] string[] characterNames;

    [SerializeField] GameObject nextGroup;
    [SerializeField] GameObject playerModel;
    [SerializeField] GameObject continueButton;

    [SerializeField] Image playerSlotImg;
    [SerializeField] Image joinedPlayerImg;

    [SerializeField] Text nameText;

    [SerializeField] Color[] colors;
    [SerializeField] GameObject[] characterModels;
    
    [SerializeField] GameObject lockedCharacter;

    [SerializeField] EventSystem eventSystem;

    internal bool canSwitchCharacter = false;
    
    int colorIndex;
    int characterIndex;

    List<Renderer> playerRenderers;

    WorldMenuManager menuManager;

    internal PlayerIndex playerIndex = PlayerIndex.One;
    GamePadState state;
    GamePadState prevState;

    soundManager soundManager;

    Singleton singleton;


    private void Awake()
    {
        singleton = Singleton.instance;

        soundManager = FindObjectOfType<soundManager>();

        menuManager = transform.root.GetComponent<WorldMenuManager>();

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
            Instantiate(characterModels[characterIndex], playerModel.transform);

            // Gets all the new meshes
            playerRenderers = new List<Renderer>(playerModel.GetComponentsInChildren<Renderer>());
        }

        // Recolors new model
        for (int i = 0; i < playerRenderers.Count; i++)
        {
            playerRenderers[i].material.color = colors[colorIndex];
        }

        Color uiColor = AddHSV(colors[colorIndex], 0f, uiColorSat - 1, 1f);
        joinedPlayerImg.color = uiColor;
        playerSlotImg.color = uiColor;
        
        nameText.text = characterNames[characterIndex];
    }


    void Update()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);

        // Rotation of character
        playerModel.transform.Rotate(new Vector3(0f, state.ThumbSticks.Right.X, 0f) * rotateSpeed * Time.deltaTime);

        if (!canSwitchCharacter && characterIndex == 1)
        { }
        else
        {
            // Switching of color and character
            // Up on stick or D-pad
            if ((state.ThumbSticks.Left.Y >= 0.3f && prevState.ThumbSticks.Left.Y < 0.3f) || (state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released))
                SwitchColor(true);

            // Down on stick or D-pad
            else if ((state.ThumbSticks.Left.Y <= -0.3f && prevState.ThumbSticks.Left.Y > -0.3f) || (state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released))
                SwitchColor(false);

            // Continues if Start is pressed
            if (state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released)
                Continue();
        }

        // Right on stick or D-pad
        if ((state.ThumbSticks.Left.X >= 0.3f && prevState.ThumbSticks.Left.X < 0.3f) || (state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released))
            SwitchCharacter(true);

        // Left on stick or D-pad
        else if ((state.ThumbSticks.Left.X <= -0.3f && prevState.ThumbSticks.Left.X > -0.3f) || (state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released))
            SwitchCharacter(false);
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

        Color uiColor = AddHSV(colors[colorIndex], 0f, uiColorSat - 1, 1f);
        joinedPlayerImg.color = uiColor;
        playerSlotImg.color = uiColor;

        soundManager.PlaySound("ButtonNavigation");
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

        if (!canSwitchCharacter && characterIndex == 1)
        {
            GameObject newModel = Instantiate(lockedCharacter, playerModel.transform);
            nameText.text = "LOCKED";

            continueButton.SetActive(false);
        }
        else
        {
            // Instantiates new model
            GameObject newModel = Instantiate(characterModels[characterIndex], playerModel.transform);

            // Gets all the new meshes
            playerRenderers = new List<Renderer>(newModel.GetComponentsInChildren<Renderer>());

            // Recolors new model
            for (int i = 0; i < playerRenderers.Count; i++)
            {
                playerRenderers[i].material.color = colors[colorIndex];
            }

            nameText.text = characterNames[characterIndex];

            continueButton.SetActive(true);
        }

        soundManager.PlaySound("ButtonNavigation");
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

        soundManager.PlaySound("ButtonClick");
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

        colorIndex = 0;

        // Recolors all meshes white
        for (int i = 0; i < playerRenderers.Count; i++)
        {
            playerRenderers[i].material.color = colors[colorIndex];
        }

        Color uiColor = AddHSV(colors[colorIndex], 0f, uiColorSat - 1, 1f);
        joinedPlayerImg.color = uiColor;
        playerSlotImg.color = uiColor;

        // Resets stuff for player
        characterIndex = 0;
        playerModel.transform.localEulerAngles = Vector3.zero;

        nameText.text = characterNames[characterIndex];

        eventSystem.SetSelectedGameObject(null);
    }
}