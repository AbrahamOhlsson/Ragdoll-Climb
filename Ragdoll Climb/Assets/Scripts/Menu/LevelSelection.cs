using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] WorldMenuManager manager;
    [SerializeField] GameObject nextGroup;

    public void SelectLevel(string levelName)
    {
        PlayerInfoSingleton.instance.selectedLevel = levelName;
        manager.OpenMenuGroup(nextGroup);
    }
}
