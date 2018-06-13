using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class PauseMenuManager : MonoBehaviour
{
    internal bool canPause = true;

    [SerializeField] GameObject mainGroup;
    [SerializeField] GameObject howToPlayGroup;
    [SerializeField] GameObject optionsGroup;
    [SerializeField] GameObject quitGroup;

    [SerializeField] EventSystem eventSystem;

    bool paused = false;

    // The path of the navigation of the menu
    Stack<MenuGroup> groupPath = new Stack<MenuGroup>();

    LevelLoader loadingScreen;

    PlayerIndex[] playerIndexes = new PlayerIndex[4];
    GamePadState[] states = new GamePadState[4];
    GamePadState[] prevStates = new GamePadState[4];

    VibrationManager[] vibrationManagers;


    private void Start ()
    {
        // Defaults to main group in case we forget to switch all groups in editor
        mainGroup.SetActive(false);
        howToPlayGroup.SetActive(false);
        optionsGroup.SetActive(false);
        quitGroup.SetActive(false);
        GetComponent<Canvas>().enabled = false;

        //MenuGroup firstGroup = new MenuGroup(mainGroup, mainGroup.GetComponentInChildren<Button>().gameObject);
        //groupPath.Push(firstGroup);

        Cursor.visible = false;

        playerIndexes[0] = PlayerIndex.One;
        playerIndexes[1] = PlayerIndex.Two;
        playerIndexes[2] = PlayerIndex.Three;
        playerIndexes[3] = PlayerIndex.Four;

        loadingScreen = GameObject.Find("Loading Screen Canvas").GetComponent<LevelLoader>();

        vibrationManagers = FindObjectsOfType<VibrationManager>();
    }


    private void Update()
    {
        // Checks input from all four controllers
        for (int i = 0; i < playerIndexes.Length; i++)
        {
            prevStates[i] = states[i];
            states[i] = GamePad.GetState(playerIndexes[i]);

            if (paused)
            {
                // If B is pressed and the current menu group isn't the main one
                if (states[i].Buttons.B == ButtonState.Pressed && prevStates[i].Buttons.B == ButtonState.Released)
                    Back();

                // If no button is highlighted
                if (eventSystem.currentSelectedGameObject == null)
                {
                    // Highlights first found button if input is received fron a controller
                    if (((states[i].DPad.Down == ButtonState.Pressed && prevStates[i].DPad.Down == ButtonState.Released) || (states[i].DPad.Up == ButtonState.Pressed && prevStates[i].DPad.Up == ButtonState.Released) || (states[i].DPad.Left == ButtonState.Pressed && prevStates[i].DPad.Left == ButtonState.Released) || (states[i].DPad.Right == ButtonState.Pressed && prevStates[i].DPad.Right == ButtonState.Released) || (states[i].Buttons.A == ButtonState.Pressed && prevStates[i].Buttons.A == ButtonState.Released) || (states[i].ThumbSticks.Left.Y > 0f || states[i].ThumbSticks.Left.Y < 0f && prevStates[i].ThumbSticks.Left.Y == 0f)))
                    {
                        eventSystem.SetSelectedGameObject(groupPath.Peek().highlightedBtn);
                    }
                }
            }

            // Pauses/unpauses if start is pressed on any controller
            if ((states[i].Buttons.Start == ButtonState.Pressed && prevStates[i].Buttons.Start == ButtonState.Released))
                Pause();
        }

        // Pauses/unpauses if Escaped is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }


    public void Pause()
    {
        if (canPause)
        {
            if (paused)
            {
                // Resets groups
                mainGroup.SetActive(false);
                howToPlayGroup.SetActive(false);
                optionsGroup.SetActive(false);
                quitGroup.SetActive(false);

                groupPath.Clear();

                // No button selected
                eventSystem.SetSelectedGameObject(null);

                paused = false;
                Time.timeScale = 1f;
                GetComponent<Canvas>().enabled = false;
                Cursor.visible = false;
            }
            else
            {
                // Opens first menu group
                MenuGroup firstGroup = new MenuGroup(mainGroup, mainGroup.GetComponentInChildren<Selectable>().gameObject);
                groupPath.Push(firstGroup);
                mainGroup.SetActive(true);

                // Highlights first button in group
                eventSystem.SetSelectedGameObject(groupPath.Peek().highlightedBtn);

                foreach (VibrationManager vibManager in vibrationManagers)
                    vibManager.StopVibration(10000);

                paused = true;
                Time.timeScale = 0f;
                GetComponent<Canvas>().enabled = true;
                Cursor.visible = true;
            }
        }
    }


    public void OpenMenuGroup(GameObject group)
    {
        groupPath.Peek().highlightedBtn = eventSystem.currentSelectedGameObject;
        groupPath.Peek().groupObj.SetActive(false);

        MenuGroup newGroup = new MenuGroup(group, group.GetComponentInChildren<Selectable>().gameObject);
        groupPath.Push(newGroup);
        group.SetActive(true);

        eventSystem.SetSelectedGameObject(newGroup.highlightedBtn);
    }


    public void Back()
    {
        if (groupPath.Peek().groupObj == optionsGroup)
            GetComponent<Options>().ResetOptions();

        if (groupPath.Peek().groupObj == mainGroup)
        {
            Pause();
        }
        else
        {
            groupPath.Pop().groupObj.SetActive(false);
            groupPath.Peek().groupObj.SetActive(true);
            eventSystem.SetSelectedGameObject(groupPath.Peek().highlightedBtn);
        }
    }


    public void LoadMainMenu()
    {
        MultiplayerManager manager = FindObjectOfType<MultiplayerManager>();

        foreach (GameObject player in manager.players)
            player.SetActive(false);

        Time.timeScale = 1f;
        canPause = false;
        loadingScreen.LoadLevelAsync("Castle Menu");
    }


    public void RestartLevel()
    {
        loadingScreen.LoadLevelAsync(SceneManager.GetActiveScene().name);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public class MenuGroup
    {
        public GameObject groupObj;
        public GameObject highlightedBtn;

        public MenuGroup(GameObject groupObj, GameObject highlightedBtn)
        {
            this.groupObj = groupObj;
            this.highlightedBtn = highlightedBtn;
        }
    }
}
