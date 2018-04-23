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
        PlayerInfoSingleton.instance.selectedLevel = levelName;

        if (levelName == "Tutorial")
            loader.LoadLevelAsync(levelName);
        else
            manager.OpenMenuGroup(nextGroup);
    }
}
