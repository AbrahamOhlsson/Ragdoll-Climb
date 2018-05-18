using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SP_LevelButton : MonoBehaviour
{
    public string world = "NULL";

    internal int levelIndex;

    [SerializeField] Image[] starImages;
    [SerializeField] Text levelIndexText;
	
    public void SetButtonValues(int _starAmount, int _levelIndex, string _bestTime)
    {
        levelIndex = _levelIndex; 

        // Makes as many stars fully visible as the amount gotten
        for (int i = 0; i < _starAmount; i++)
            starImages[i].color = new Color(starImages[i].color.r, starImages[i].color.g, starImages[i].color.b, 1);

        levelIndexText.text = levelIndex.ToString();
    }
}
