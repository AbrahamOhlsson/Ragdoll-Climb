using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] WorldMenuManager manager;
    [SerializeField] GameObject nextGroup;
    [SerializeField] LevelLoader loader;

    public void SelectLevel(string levelName)
    {
        Singleton.instance.selectedLevel = levelName;
        Singleton.instance.mode = Singleton.Modes.Multi;

        if (levelName == "Tutorial")
        {
            if (Singleton.instance.playerAmount > 1)
                loader.LoadLevelAsync(levelName);
            else
                loader.LoadLevelAsync("Single" + levelName);
        }
        else
            manager.OpenMenuGroup(nextGroup);
    }
}
