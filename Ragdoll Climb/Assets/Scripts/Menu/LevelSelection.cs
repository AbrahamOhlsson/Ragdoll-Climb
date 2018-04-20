using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] WorldMenuManager manager;
    [SerializeField] GameObject nextGroup;

    public void SelectLevel(string levelName)
    {
        PlayerInfoSingleton.instance.selectedLevel = levelName;

        if (levelName == "Tutorial")
            SceneManager.LoadScene(levelName);
        else
            manager.OpenMenuGroup(nextGroup);
    }
}
