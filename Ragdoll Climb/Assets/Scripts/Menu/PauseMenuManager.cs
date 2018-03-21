using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainGroup;
    [SerializeField] GameObject howToPlayGroup;
    [SerializeField] GameObject optionsGroup;
    [SerializeField] GameObject quitGroup;

    [SerializeField] EventSystem eventSystem;

    bool paused = false;

    // The path of the navigation of the menu
    Stack<GameObject> groupPath = new Stack<GameObject>();

    PlayerIndex[] playerIndexes = new PlayerIndex[4];
    GamePadState[] states = new GamePadState[4];
    GamePadState[] prevStates = new GamePadState[4];


    private void Start ()
    {
        // Defaults to main group in case we forget to switch all groups in editor
        mainGroup.SetActive(true);
        howToPlayGroup.SetActive(false);
        optionsGroup.SetActive(false);
        quitGroup.SetActive(false);
        GetComponent<Canvas>().enabled = false;

        groupPath.Push(mainGroup);

        Cursor.visible = false;

        playerIndexes[0] = PlayerIndex.One;
        playerIndexes[1] = PlayerIndex.Two;
        playerIndexes[2] = PlayerIndex.Three;
        playerIndexes[3] = PlayerIndex.Four;
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
                        eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Button>().gameObject);
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
        if (paused)
        {
            paused = false;
            Time.timeScale = 1f;
            GetComponent<Canvas>().enabled = false;
            Cursor.visible = false;
        }
        else
        {
            paused = true;
            Time.timeScale = 0f;
            GetComponent<Canvas>().enabled = true;
            Cursor.visible = true;
        }
    }


    public void OpenMenuGroup(GameObject group)
    {
        groupPath.Peek().SetActive(false);
        groupPath.Push(group);
        eventSystem.SetSelectedGameObject(group.GetComponentInChildren<Button>().gameObject);
        group.SetActive(true);
    }


    public void Back()
    {
        if (groupPath.Peek() == mainGroup)
        {
            Pause();
        }
        else
        {
            groupPath.Pop().SetActive(false);
            eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Button>().gameObject);
            groupPath.Peek().SetActive(true);
        }
    }


    public void LoadMainMenu()
    {
        Pause();
        Cursor.visible = true;
        SceneManager.LoadScene("Ice Menu");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
