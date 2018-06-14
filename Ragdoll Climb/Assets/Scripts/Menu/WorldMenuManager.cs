using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class WorldMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainGroup;
    [SerializeField] GameObject playerSelectGroup;
    [SerializeField] GameObject spSelectGroup;
    [SerializeField] GameObject levelSelectGroup;
    [SerializeField] GameObject spLevelSelectGroup;
    [SerializeField] GameObject howToPlayGroup;
    [SerializeField] GameObject diffLengthGroup;
    [SerializeField] GameObject optionsGroup;
    [SerializeField] GameObject creditsGroup;

    [SerializeField] GameObject spButton;

    [SerializeField] float cameraSpeed = 0.5f;
    [SerializeField] GameObject mainCamera;

    [SerializeField] LevelLoader levelLoader;

    [SerializeField] EventSystem eventSystem;

    bool moving = false;

    // The path of the navigation of the menu
    public Stack<MenuGroup> groupPath = new Stack<MenuGroup>();

    [SerializeField] GameObject lastGroup;

    PlayerIndex[] playerIndexes = new PlayerIndex[4];
    GamePadState[] states = new GamePadState[4];
    GamePadState[] prevStates = new GamePadState[4];

    Singleton singleton;

    //[SerializeField] soundManager SoundManager;

    private void Start()
    {
        singleton = Singleton.instance;
        singleton.Load();

        // Deactivates all groups in case we forget to switch all groups in editor
        mainGroup.SetActive(false);
        playerSelectGroup.SetActive(false);
        levelSelectGroup.SetActive(false);
        spLevelSelectGroup.SetActive(false);
        spSelectGroup.SetActive(false);
        howToPlayGroup.SetActive(false);
        diffLengthGroup.SetActive(false);
        optionsGroup.SetActive(false);
        creditsGroup.SetActive(false);

        MenuGroup firstGroup = new MenuGroup(mainGroup, mainGroup.GetComponentInChildren<Selectable>().gameObject);
        groupPath.Push(firstGroup);

        switch (singleton.mode)
        {
            case Singleton.Modes.Single:
                MenuGroup group_spSelect = new MenuGroup(spSelectGroup, spSelectGroup.GetComponentInChildren<Selectable>().gameObject);
                MenuGroup group_spLevel = new MenuGroup(spLevelSelectGroup, spLevelSelectGroup.GetComponentInChildren<Selectable>().gameObject);
                groupPath.Push(group_spSelect);
                groupPath.Push(group_spLevel);
				GetComponent<KeyProgression>().CheckLevelsComplete();
                break;

            case Singleton.Modes.Multi:
                MenuGroup group_mpSelect = new MenuGroup(playerSelectGroup, playerSelectGroup.GetComponentInChildren<Selectable>().gameObject);
                groupPath.Push(group_mpSelect);
                break;
        }
        
        playerIndexes[0] = PlayerIndex.One;
        playerIndexes[1] = PlayerIndex.Two;
        playerIndexes[2] = PlayerIndex.Three;
        playerIndexes[3] = PlayerIndex.Four;
        
        groupPath.Peek().groupObj.SetActive(true);
        mainCamera.transform.position = groupPath.Peek().groupObj.transform.GetChild(0).transform.position;
        mainCamera.transform.rotation = groupPath.Peek().groupObj.transform.GetChild(0).transform.rotation;

        if (groupPath.Peek().groupObj != spSelectGroup && groupPath.Peek().groupObj != playerSelectGroup)
            eventSystem.SetSelectedGameObject(groupPath.Peek().highlightedBtn);
    }   


    private void Update()
    {
        // Checks input from all four controllers
        for (int i = 0; i < playerIndexes.Length; i++)
        {
            prevStates[i] = states[i];
            states[i] = GamePad.GetState(playerIndexes[i]);

            // If B is pressed and the current menu group isn't the main one
            if (states[i].Buttons.B == ButtonState.Pressed && prevStates[i].Buttons.B == ButtonState.Released && groupPath.Peek().groupObj != mainGroup && !moving)
                Back();


            //// If A is pressed play a sound 
            //if (states[i].Buttons.A == ButtonState.Pressed && prevStates[i].Buttons.A == ButtonState.Released && !moving)
            //{
            //    SoundManager.PlaySoundRandPitch("goodClimb");
            //    print("sound test !!!!!!!!!!!!!");
            //}


            if (eventSystem.currentSelectedGameObject == null && groupPath.Peek().groupObj != playerSelectGroup && groupPath.Peek().groupObj != spSelectGroup)
            {
                if (((states[i].DPad.Down == ButtonState.Pressed && prevStates[i].DPad.Down == ButtonState.Released) || (states[i].DPad.Up == ButtonState.Pressed && prevStates[i].DPad.Up == ButtonState.Released) || (states[i].DPad.Left == ButtonState.Pressed && prevStates[i].DPad.Left == ButtonState.Released) || (states[i].DPad.Right == ButtonState.Pressed && prevStates[i].DPad.Right == ButtonState.Released) || (states[i].Buttons.A == ButtonState.Pressed && prevStates[i].Buttons.A == ButtonState.Released) || (states[i].ThumbSticks.Left.Y > 0f || states[i].ThumbSticks.Left.Y < 0f && prevStates[i].ThumbSticks.Left.Y == 0f)))
                {
                    eventSystem.SetSelectedGameObject(groupPath.Peek().highlightedBtn);
                }
            }

            if (groupPath.Peek().groupObj == mainGroup && eventSystem.currentSelectedGameObject == spButton)
            {
                if (states[i].Buttons.A == ButtonState.Pressed && prevStates[i].Buttons.A == ButtonState.Released)
                {
                    spSelectGroup.GetComponent<CharacterSelection_SP>().playerIndex = playerIndexes[i];
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    spSelectGroup.GetComponent<CharacterSelection_SP>().playerIndex = PlayerIndex.One;
                }
            }
        }
        
        Vector3 camGoalPos = groupPath.Peek().groupObj.transform.GetChild(0).transform.position;
        Quaternion camGoalRot = groupPath.Peek().groupObj.transform.GetChild(0).transform.rotation;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, camGoalPos, cameraSpeed * Time.deltaTime);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, camGoalRot, cameraSpeed * Time.deltaTime);

        if (Vector3.Distance(mainCamera.transform.position, camGoalPos) <= 1f && moving)
        {
            lastGroup.SetActive(false);
            eventSystem.enabled = true;
            moving = false;

            if (groupPath.Peek().groupObj == playerSelectGroup || groupPath.Peek().groupObj == spSelectGroup)
                eventSystem.SetSelectedGameObject(null);
            else
                eventSystem.SetSelectedGameObject(groupPath.Peek().highlightedBtn);
        }

        if (Input.GetKeyDown("b") && eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(groupPath.Peek().highlightedBtn);
        }
    }


    public void OpenMenuGroup(GameObject group)
    {
        StartCoroutine(OpenMenuGroup_co(group));
    }


    public void Back()
    {
        if (groupPath.Peek().groupObj == optionsGroup)
            optionsGroup.GetComponent<Options>().ResetOptions();

        //groupPath.Pop().SetActive(false);
        //eventSystem.SetSelectedGameObject(groupPath.Peek().GetComponentInChildren<Button>().gameObject);
        eventSystem.enabled = false;
        lastGroup = groupPath.Peek().groupObj;
        groupPath.Pop();
        groupPath.Peek().groupObj.SetActive(true);
        moving = true;

        eventSystem.SetSelectedGameObject(null);// TEST!!!!!
    }


    public void LoadLevel()
    {
        levelLoader.LoadLevelAsync(singleton.selectedLevel);
        gameObject.SetActive(false);
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    IEnumerator OpenMenuGroup_co(GameObject group)
    {
        yield return new WaitForEndOfFrame();

        groupPath.Peek().highlightedBtn = eventSystem.currentSelectedGameObject;
        eventSystem.SetSelectedGameObject(null);
        eventSystem.enabled = false;
        lastGroup = groupPath.Peek().groupObj;

        MenuGroup newGroup = new MenuGroup(group, group.GetComponentInChildren<Selectable>().gameObject);
        groupPath.Push(newGroup);
        group.SetActive(true);

        moving = true;

        if (group == playerSelectGroup)
            playerSelectGroup.GetComponent<Lobby>().ResetValues(true);
        else if (group == spSelectGroup)
            spSelectGroup.GetComponent<CharacterSelection_SP>().ResetValues(true);
        else if (group == diffLengthGroup)
            diffLengthGroup.GetComponent<DiffLengthSelection>().ResetValues();
		else if (group == spLevelSelectGroup)
			GetComponent<KeyProgression>().CheckLevelsComplete();
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
