using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainGroup;
    [SerializeField] GameObject playerSelectGroup;
    [SerializeField] GameObject levelSelectGroup;
    [SerializeField] GameObject howToPlayGroup;
    [SerializeField] GameObject optionsGroup;
    [SerializeField] GameObject creditsGroup;

    [SerializeField] EventSystem eventSystem;

    // The path of the navigation of the menu
    Stack<GameObject> groupPath = new Stack<GameObject>();

    PlayerIndex[] playerIndexes = new PlayerIndex[4];
    GamePadState[] states = new GamePadState[4];
    GamePadState[] prevStates = new GamePadState[4];


    private void Start ()
    {
        // Defaults to main group in case we forget to switch all groups in editor
        mainGroup.SetActive(true);
        playerSelectGroup.SetActive(false);
        levelSelectGroup.SetActive(false);
        howToPlayGroup.SetActive(false);
        optionsGroup.SetActive(false);
        creditsGroup.SetActive(false);

        groupPath.Push(mainGroup);

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

            // If B is pressed and the current menu group isn't the main one
            if (states[i].Buttons.B == ButtonState.Pressed && prevStates[i].Buttons.B == ButtonState.Released && groupPath.Peek() != mainGroup)
                Back();
        }
    }


    public void OpenMenuGroup(GameObject group)
    {
        groupPath.Peek().SetActive(false);
        groupPath.Push(group);
        eventSystem.SetSelectedGameObject(group.GetComponentInChildren<Button>().gameObject);
        group.SetActive(true);

        if (group == playerSelectGroup)
            playerSelectGroup.GetComponent<Lobby>().ResetValues();
    }


    public void Back()
    {
        groupPath.Pop().SetActive(false);
        eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Button>().gameObject);
        groupPath.Peek().SetActive(true);
    }


    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
