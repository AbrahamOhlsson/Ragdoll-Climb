using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleLevelSelection : MonoBehaviour
{
    [SerializeField] Transform levelBtnGroup_ice;
    [SerializeField] Transform levelBtnGroup_volcano;
    [SerializeField] Transform levelBtnGroup_woods;
    [SerializeField] Transform levelBtnGroup_metal;

    SP_LevelButton[] levelButtons_ice;
    SP_LevelButton[] levelButtons_volcano;
    SP_LevelButton[] levelButtons_woods;
    SP_LevelButton[] levelButtons_metal;

    [Space]
    [SerializeField] LevelLoader loader;

    Singleton singleton;


	void Start ()
    {
        singleton = Singleton.instance;

        levelButtons_ice = levelBtnGroup_ice.GetComponentsInChildren<SP_LevelButton>();
        levelButtons_volcano = levelBtnGroup_volcano.GetComponentsInChildren<SP_LevelButton>();
        levelButtons_woods = levelBtnGroup_woods.GetComponentsInChildren<SP_LevelButton>();
        levelButtons_metal = levelBtnGroup_metal.GetComponentsInChildren<SP_LevelButton>();

        // If there is no save file yet
        if (!singleton.SavefileExists())
        {
            // Creates new level stats lists
            singleton.levelStats_ice = new List<SP_LevelStats>();
            singleton.levelStats_volcano = new List<SP_LevelStats>();
            singleton.levelStats_woods = new List<SP_LevelStats>();
            singleton.levelStats_metal = new List<SP_LevelStats>();

            // Adds as many level stats objects as there are level buttons
            for (int i = 0; i < levelButtons_ice.Length; i++)
                singleton.levelStats_ice.Add(new SP_LevelStats());
            for (int i = 0; i < levelButtons_volcano.Length; i++)
                singleton.levelStats_volcano.Add(new SP_LevelStats());
            for (int i = 0; i < levelButtons_woods.Length; i++)
                singleton.levelStats_woods.Add(new SP_LevelStats());
            for (int i = 0; i < levelButtons_metal.Length; i++)
                singleton.levelStats_metal.Add(new SP_LevelStats());
        }

        // Adds further level stats objects in lists if there are fewer of them than buttons
        if (singleton.levelStats_ice.Count < levelButtons_ice.Length)
        {
            for (int i = singleton.levelStats_ice.Count; i < levelButtons_ice.Length; i++)
                singleton.levelStats_ice.Add(new SP_LevelStats());
        }
        if (singleton.levelStats_volcano.Count < levelButtons_volcano.Length)
        {
            for (int i = singleton.levelStats_ice.Count; i < levelButtons_volcano.Length; i++)
                singleton.levelStats_volcano.Add(new SP_LevelStats());
        }
        if (singleton.levelStats_woods.Count < levelButtons_woods.Length)
        {
            for (int i = singleton.levelStats_woods.Count; i < levelButtons_woods.Length; i++)
                singleton.levelStats_woods.Add(new SP_LevelStats());
        }
        if (singleton.levelStats_woods.Count < levelButtons_metal.Length)
        {
            for (int i = singleton.levelStats_metal.Count; i < levelButtons_metal.Length; i++)
                singleton.levelStats_metal.Add(new SP_LevelStats());
        }
        
        // Sets button number, star amount and best time to be displayed
        for (int i = 0; i < levelButtons_ice.Length; i++)
            levelButtons_ice[i].SetButtonValues(singleton.levelStats_ice[i].starAmount, i + 1, singleton.levelStats_ice[i].bestTime_str);
        for (int i = 0; i < levelButtons_volcano.Length; i++)
            levelButtons_volcano[i].SetButtonValues(singleton.levelStats_volcano[i].starAmount, i + 1, singleton.levelStats_volcano[i].bestTime_str);
        for (int i = 0; i < levelButtons_woods.Length; i++)
            levelButtons_woods[i].SetButtonValues(singleton.levelStats_woods[i].starAmount, i + 1, singleton.levelStats_woods[i].bestTime_str);
        for (int i = 0; i < levelButtons_metal.Length; i++)
            levelButtons_metal[i].SetButtonValues(singleton.levelStats_metal[i].starAmount, i + 1, singleton.levelStats_metal[i].bestTime_str);
    }
    

    public void SelectSinglePlayerLevel(SP_LevelButton levelButtonScript)
    {
        // Determines name of scene that should be loaded by getting the button's world and level index values
        string levelName = "SP_level" + "_" + levelButtonScript.world + levelButtonScript.levelIndex;

        singleton.selectedLevel = levelName;
        singleton.currSpLevelIndex = levelButtonScript.levelIndex - 1;
        singleton.currSpWorld = levelButtonScript.world;
        singleton.mode = Singleton.Modes.Single;

        if (singleton.currSpWorld == "woods")
            singleton.currLevelStats = singleton.levelStats_woods[singleton.currSpLevelIndex];
        else if (singleton.currSpWorld == "ice")
            singleton.currLevelStats = singleton.levelStats_ice[singleton.currSpLevelIndex];
        else if (singleton.currSpWorld == "lava")
            singleton.currLevelStats = singleton.levelStats_volcano[singleton.currSpLevelIndex];
        else if (singleton.currSpWorld == "metal")
            singleton.currLevelStats = singleton.levelStats_metal[singleton.currSpLevelIndex];

        loader.LoadLevelAsync(levelName);
    }
}
