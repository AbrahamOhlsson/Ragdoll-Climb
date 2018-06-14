using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SingleLevelSelection : MonoBehaviour
{
    // Parent objects containing level buttons
    [SerializeField] Transform levelBtnGroup_ice;
    [SerializeField] Transform levelBtnGroup_volcano;
    [SerializeField] Transform levelBtnGroup_woods;
    [SerializeField] Transform levelBtnGroup_metal;
    [Space]
    [SerializeField] Text levelNameTxt;
    [SerializeField] Text bestTimeTxt;
    [Space]
    [SerializeField] LevelLoader loader;
    [Space]
    [SerializeField] Lobby lobby;
    [SerializeField] CharacterSelection_SP characterSelection;

    SP_LevelButton[] levelButtons_ice;
    SP_LevelButton[] levelButtons_volcano;
    SP_LevelButton[] levelButtons_woods;
    SP_LevelButton[] levelButtons_metal;
    [SerializeField] SP_LevelButton[] levelButtons_castle;

    bool mouseOverBtn = false;

	bool iceCompleted = false;
	bool volcanoCompleted = false;
	bool metalCompleted = false;

	Singleton singleton;


	void Awake ()
    {
        singleton = Singleton.instance;

        // Gets buttons
        levelButtons_ice = levelBtnGroup_ice.GetComponentsInChildren<SP_LevelButton>(true);
        levelButtons_volcano = levelBtnGroup_volcano.GetComponentsInChildren<SP_LevelButton>(true);
        levelButtons_woods = levelBtnGroup_woods.GetComponentsInChildren<SP_LevelButton>(true);
        levelButtons_metal = levelBtnGroup_metal.GetComponentsInChildren<SP_LevelButton>(true);

        // If there is no save file yet
        if (!singleton.SavefileExists())
        {
            // Creates new level stats lists
            singleton.levelStats_ice = new List<SP_LevelStats>();
            singleton.levelStats_volcano = new List<SP_LevelStats>();
            singleton.levelStats_woods = new List<SP_LevelStats>();
            singleton.levelStats_metal = new List<SP_LevelStats>();
            singleton.levelStats_castle = new List<SP_LevelStats>();

            // Adds as many level stats objects as there are level buttons
            for (int i = 0; i < levelButtons_ice.Length; i++)
                singleton.levelStats_ice.Add(new SP_LevelStats());
            for (int i = 0; i < levelButtons_volcano.Length; i++)
                singleton.levelStats_volcano.Add(new SP_LevelStats());
            for (int i = 0; i < levelButtons_woods.Length; i++)
                singleton.levelStats_woods.Add(new SP_LevelStats());
            for (int i = 0; i < levelButtons_metal.Length; i++)
                singleton.levelStats_metal.Add(new SP_LevelStats());
            for (int i = 0; i < levelButtons_castle.Length; i++)
                singleton.levelStats_castle.Add(new SP_LevelStats());

        }
        else
            singleton.Load();

        if (singleton.levelStats_castle == null)
            singleton.levelStats_castle = new List<SP_LevelStats>();

        // Adds further level stats objects in lists if there are fewer of them than buttons
        if (singleton.levelStats_ice.Count < levelButtons_ice.Length)
        {
            for (int i = singleton.levelStats_ice.Count; i < levelButtons_ice.Length; i++)
                singleton.levelStats_ice.Add(new SP_LevelStats());
        }
        if (singleton.levelStats_volcano.Count < levelButtons_volcano.Length)
        {
            for (int i = singleton.levelStats_volcano.Count; i < levelButtons_volcano.Length; i++)
                singleton.levelStats_volcano.Add(new SP_LevelStats());
        }
        if (singleton.levelStats_woods.Count < levelButtons_woods.Length)
        {
            for (int i = singleton.levelStats_woods.Count; i < levelButtons_woods.Length; i++)
                singleton.levelStats_woods.Add(new SP_LevelStats());
        }
        if (singleton.levelStats_metal.Count < levelButtons_metal.Length)
        {
            for (int i = singleton.levelStats_metal.Count; i < levelButtons_metal.Length; i++)
                singleton.levelStats_metal.Add(new SP_LevelStats());
        }
        if (singleton.levelStats_castle.Count < levelButtons_castle.Length)
        {
            for (int i = singleton.levelStats_castle.Count; i < levelButtons_castle.Length; i++)
                singleton.levelStats_castle.Add(new SP_LevelStats());
        }

        // Sets button number, star amount and best time to be displayed
        for (int i = 0; i < levelButtons_ice.Length; i++)
            levelButtons_ice[i].SetButtonValues(singleton.levelStats_ice[i].starAmount, i + 1, singleton.levelStats_ice[i].bestTime_flt);
        for (int i = 0; i < levelButtons_volcano.Length; i++)
            levelButtons_volcano[i].SetButtonValues(singleton.levelStats_volcano[i].starAmount, i + 1, singleton.levelStats_volcano[i].bestTime_flt);
        for (int i = 0; i < levelButtons_woods.Length; i++)
            levelButtons_woods[i].SetButtonValues(singleton.levelStats_woods[i].starAmount, i + 1, singleton.levelStats_woods[i].bestTime_flt);
        for (int i = 0; i < levelButtons_metal.Length; i++)
            levelButtons_metal[i].SetButtonValues(singleton.levelStats_metal[i].starAmount, i + 1, singleton.levelStats_metal[i].bestTime_flt);
        for (int i = 0; i < levelButtons_castle.Length; i++)
            levelButtons_castle[i].SetButtonValues(singleton.levelStats_metal[i].starAmount, i + 1, singleton.levelStats_metal[i].bestTime_flt);
        
        // Goes through each button
        for (int i = 0; i < singleton.levelStats_woods.Count; i++)
        {
            // If level is completed
            if (singleton.levelStats_woods[i].completed)
            {
                // Unlocks next level if the level isn't the last one in the group
                if (i < singleton.levelStats_woods.Count - 1)
                    levelButtons_woods[i + 1].gameObject.SetActive(true);
                else
                {
                    // Activates the first level of ice, lava and metal if all woods are completed
                    levelButtons_ice[0].gameObject.SetActive(true);
                    levelButtons_metal[0].gameObject.SetActive(true);
                    levelButtons_volcano[0].gameObject.SetActive(true);
                }
            }
            else
                break;
        }
        for (int i = 0; i < singleton.levelStats_ice.Count; i++)
        {
            if (singleton.levelStats_ice[i].completed)
            {
                if (i < singleton.levelStats_ice.Count - 1)
                    levelButtons_ice[i + 1].gameObject.SetActive(true);
                else
                    iceCompleted = true;
            }
            else
                break;
        }
        for (int i = 0; i < singleton.levelStats_volcano.Count; i++)
        {
            if (singleton.levelStats_volcano[i].completed)
            {
                if (i < singleton.levelStats_volcano.Count - 1)
                    levelButtons_volcano[i + 1].gameObject.SetActive(true);
                else
                    volcanoCompleted = true;
            }
            else
                break;
        }
        for (int i = 0; i < singleton.levelStats_metal.Count; i++)
        {
            if (singleton.levelStats_metal[i].completed)
            {
                if (i < singleton.levelStats_metal.Count - 1)
                    levelButtons_metal[i + 1].gameObject.SetActive(true);
                else
                    metalCompleted = true;
            }
            else
                break;
        }
        // Unlocks Mannequin character if castle level is completed
        for (int i = 0; i < singleton.levelStats_castle.Count; i++)
        {
            if (singleton.levelStats_castle[i].completed)
            {
                if (i == singleton.levelStats_castle.Count - 1)
                {
                    lobby.canSwitchCharacter = true;
                    characterSelection.canSwitchCharacter = true;
                }
            }
            else
                break;
        }
    }
    

    private void Update()
    {
        // Showcases level name and time record on selected button
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<SP_LevelButton>())
        {
            UpdateLevelInfo(EventSystem.current.currentSelectedGameObject.GetComponent<SP_LevelButton>(), false);
        }
    }


    public void SelectSinglePlayerLevel(SP_LevelButton levelButtonScript)
    {
        // Determines name of scene that should be loaded by getting the button's world and level index values
        string levelName = "SP_level" + "_" + levelButtonScript.world + levelButtonScript.levelIndex;

        // Gives singleton valuable information
        singleton.selectedLevel = levelName;
        singleton.currSpLevelIndex = levelButtonScript.levelIndex - 1;
        singleton.currSpWorld = levelButtonScript.world;
        singleton.mode = Singleton.Modes.Single;
        
        // Lets singleton know which world is being played
        if (singleton.currSpWorld == "woods")
            singleton.currLevelStats = singleton.levelStats_woods[singleton.currSpLevelIndex];
        else if (singleton.currSpWorld == "ice")
            singleton.currLevelStats = singleton.levelStats_ice[singleton.currSpLevelIndex];
        else if (singleton.currSpWorld == "lava")
            singleton.currLevelStats = singleton.levelStats_volcano[singleton.currSpLevelIndex];
        else if (singleton.currSpWorld == "metal")
            singleton.currLevelStats = singleton.levelStats_metal[singleton.currSpLevelIndex];
        else if (singleton.currSpWorld == "castle")
            singleton.currLevelStats = singleton.levelStats_castle[singleton.currSpLevelIndex];

        // Loads level
        loader.LoadLevelAsync(levelName);
    }
    

    public void UpdateLevelInfo(SP_LevelButton spButton, bool mouse)
    {
        if (!mouseOverBtn)
        {
            string levelName;

            if (spButton.world != "castle")
                levelName = spButton.world + " " + spButton.levelIndex;
            else
                levelName = "castle";

            levelNameTxt.text = levelName.ToUpper();

            if (spButton.bestTime == Mathf.Infinity)
                bestTimeTxt.text = "No Best Time Yet";
            else
                bestTimeTxt.text = "Best Time: " + spButton.bestTime + " sec";

            if (mouse)
                mouseOverBtn = true;
        }
    }


    public void ResetLevelInfo()
    {
        mouseOverBtn = false;

        levelNameTxt.text = "";
        bestTimeTxt.text = "";
    }
}
