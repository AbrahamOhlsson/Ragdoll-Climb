﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SingleLevelSelection : MonoBehaviour
{
    [SerializeField] Transform levelBtnGroup_ice;
    [SerializeField] Transform levelBtnGroup_volcano;
    [SerializeField] Transform levelBtnGroup_woods;
    [SerializeField] Transform levelBtnGroup_metal;
    [Space]
    [SerializeField] Text levelNameTxt;
    [SerializeField] Text bestTimeTxt;
    [Space]
    [SerializeField] LevelLoader loader;

	[SerializeField] Image lineLeft;
	[SerializeField] Image lineRight;
	[SerializeField] Image lineTop;

	SP_LevelButton[] levelButtons_ice;
    SP_LevelButton[] levelButtons_volcano;
    SP_LevelButton[] levelButtons_woods;
    SP_LevelButton[] levelButtons_metal;

    bool mouseOverBtn = false;

	bool iceCompleted = false;
	bool volcanoCompleted = false;
	bool metalCompleted = false;

	Singleton singleton;


	void Awake ()
    {
        singleton = Singleton.instance;

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
        else
            singleton.Load();

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
        if (singleton.levelStats_woods.Count < levelButtons_metal.Length)
        {
            for (int i = singleton.levelStats_metal.Count; i < levelButtons_metal.Length; i++)
                singleton.levelStats_metal.Add(new SP_LevelStats());
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
		

        for (int i = 0; i < singleton.levelStats_woods.Count; i++)
        {
            if (singleton.levelStats_woods[i].completed)
            {
                if (i < singleton.levelStats_woods.Count - 1)
                    levelButtons_woods[i + 1].gameObject.SetActive(true);
                else
                {
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
    }


	public void CheckLevelsComplete()
	{
		//If all levels are completed
		if (iceCompleted && metalCompleted && volcanoCompleted)
		{
			StartCoroutine(DrawLines());
		}
	}

	//courotine to draw out lines on the map
	IEnumerator DrawLines()
	{
		yield return new WaitForSeconds(1);

		while (lineLeft.fillAmount < 1)
		{
			lineLeft.fillAmount  += 0.5f * Time.deltaTime;
			lineRight.fillAmount += 0.5f * Time.deltaTime;
			lineTop.fillAmount	 += 0.5f * Time.deltaTime;
			yield return null;
		}
	}

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<SP_LevelButton>())
        {
            UpdateLevelInfo(EventSystem.current.currentSelectedGameObject.GetComponent<SP_LevelButton>(), false);
        }
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
    

    public void UpdateLevelInfo(SP_LevelButton spButton, bool mouse)
    {
        if (!mouseOverBtn)
        {
            string levelName = spButton.world + " " + spButton.levelIndex;
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
