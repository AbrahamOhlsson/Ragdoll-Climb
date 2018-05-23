using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SP_LevelButton : MonoBehaviour
{
    public string world = "NULL";

    internal int levelIndex;
    internal float bestTime = Mathf.Infinity;

    [SerializeField] Image[] starImages;
    [SerializeField] Text levelIndexText;
    [SerializeField] SingleLevelSelection levelSelection;
	

    public void SetButtonValues(int _starAmount, int _levelIndex, float _bestTime)
    {
        levelIndex = _levelIndex;
        bestTime = _bestTime;

        // Makes as many stars fully visible as the amount gotten
        for (int i = 0; i < _starAmount; i++)
            starImages[i].color = new Color(starImages[i].color.r, starImages[i].color.g, starImages[i].color.b, 1);

        levelIndexText.text = levelIndex.ToString();
    }


    private void OnMouseEnter()
    {
        levelSelection.UpdateLevelInfo(this, true);
    }


    private void OnMouseExit()
    {
        levelSelection.ResetLevelInfo();
    }
}
