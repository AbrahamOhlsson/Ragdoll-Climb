using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SP_ResultsMenu : MonoBehaviour
{
    [SerializeField] Text levelNameText;
    [SerializeField] Text thisTimeText;
    [SerializeField] Text bestTimeText;
    [SerializeField] Text currLastText;
    [Space]
    [SerializeField] Image timeBarImg;
    [SerializeField] Sprite recordTimeBar;
    [Space]
    [SerializeField] Image[] keys;
    [Space]
    [SerializeField] GameObject nextLvlButton;

    float bestTime;

    LevelLoader loader;
    PauseMenuManager pauseMenu;

    Singleton singleton;


    void Start ()
    {
        singleton = Singleton.instance;
        loader = GameObject.Find("Loading Screen Canvas").GetComponent<LevelLoader>();
        pauseMenu = GameObject.Find("Pause Menu").GetComponent<PauseMenuManager>();

        string levelName = singleton.currSpWorld + " " + (singleton.currSpLevelIndex + 1);
        levelNameText.text = levelName.ToUpper();

        if (singleton.currLevelStats.bestTime_flt == Mathf.Infinity)
        {
            bestTimeText.text = "None";
        }
        else
        {
            bestTime = (float)System.Math.Round(singleton.currLevelStats.bestTime_flt, 2);
            bestTimeText.text = bestTime + " sec";
        }

        if ((singleton.currSpWorld == "woods" && singleton.currSpLevelIndex == 2) || singleton.currSpLevelIndex == 9)
            nextLvlButton.SetActive(false);

        gameObject.SetActive(false);
    }
    

    public void Activate(singleplayerInfo info)
    {
        gameObject.SetActive(true);
        pauseMenu.canPause = false;

        Time.timeScale = 0;

        if (info.playtime < bestTime)
        {
            timeBarImg.sprite = recordTimeBar;
            currLastText.text = "Last Best Time";
        }

        thisTimeText.text = (float)System.Math.Round(info.playtime, 2) + " sec";

        for (int i = 0; i < info.stars; i++)
        {
            keys[i].color = Color.white;
        }

        GetComponent<Canvas>().enabled = true;

        EventSystem.current.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
    }


    public void NextLevel()
    {
        Time.timeScale = 1f;
        string levelName = "SP_level_" + singleton.currSpWorld + (singleton.currSpLevelIndex + 2);
        singleton.selectedLevel = levelName;
        singleton.currSpLevelIndex++;

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


    public void ReplayLevel()
    {
        Time.timeScale = 1f;
        loader.LoadLevelAsync("SP_level_" + singleton.currSpWorld + (singleton.currSpLevelIndex + 1));
    }


    public void SelectLevel()
    {
        Time.timeScale = 1f;
        loader.LoadLevelAsync("Ice Menu");
    }
}
