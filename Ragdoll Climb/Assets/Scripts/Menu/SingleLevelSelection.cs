using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleLevelSelection : MonoBehaviour
{
    [SerializeField] SP_LevelButton[] levelButtons_ice;
    [SerializeField] SP_LevelButton[] levelButtons_volcano;
    [SerializeField] SP_LevelButton[] levelButtons_woods;
    [SerializeField] LevelLoader loader;

    Singleton singleton;


	void Start ()
    {
        singleton = Singleton.instance;

        // If there is no save file yet
        if (!singleton.SavefileExists())
        {
            // Creates new level stats lists
            singleton.levelStats_ice = new List<SP_LevelStats>();
            singleton.levelStats_volcano = new List<SP_LevelStats>();
            singleton.levelStats_woods = new List<SP_LevelStats>();

            // Adds as many level stats objects as there are level buttons
            for (int i = 0; i < levelButtons_ice.Length; i++)
                singleton.levelStats_ice.Add(new SP_LevelStats());
            for (int i = 0; i < levelButtons_volcano.Length; i++)
                singleton.levelStats_volcano.Add(new SP_LevelStats());
            for (int i = 0; i < levelButtons_woods.Length; i++)
                singleton.levelStats_woods.Add(new SP_LevelStats());
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

        // Sets button number, star amount and best time to be displayed
        for (int i = 0; i < levelButtons_ice.Length; i++)
            levelButtons_ice[i].SetButtonValues(singleton.levelStats_ice[i].starAmount, i + 1, singleton.levelStats_ice[i].bestTime_str);
        for (int i = 0; i < levelButtons_volcano.Length; i++)
            levelButtons_volcano[i].SetButtonValues(singleton.levelStats_volcano[i].starAmount, i + 1, singleton.levelStats_volcano[i].bestTime_str);
        for (int i = 0; i < levelButtons_woods.Length; i++)
            levelButtons_woods[i].SetButtonValues(singleton.levelStats_woods[i].starAmount, i + 1, singleton.levelStats_woods[i].bestTime_str);
	}
    

    public void SelectSinglePlayerLevel(SP_LevelButton levelButtonScript)
    {
        // Determines name of scene that should be loaded by getting the button's world and level index values
        string levelName = "SP_level" + "_" + levelButtonScript.world + levelButtonScript.levelIndex;

        singleton.selectedLevel = levelName;
        singleton.currSpLevelIndex = levelButtonScript.levelIndex - 1;
        singleton.currSpWorld = levelButtonScript.world;
        singleton.mode = Singleton.Modes.Single;

        loader.LoadLevelAsync(levelName);
    }
}
