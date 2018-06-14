using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyProgression : MonoBehaviour
{
    [SerializeField] Text keyAmountText;
    
    [SerializeField] Image lineLeft;
    [SerializeField] Image lineRight;
    [SerializeField] Image lineTop;
    [SerializeField] Button finalLevelButton;

    [SerializeField] Lobby lobby;
    [SerializeField] CharacterSelection_SP characterSelection;

    int keyAmount;

    float imgAlpha = 0;

    Singleton singleton;


	void Start ()
    {
        singleton = Singleton.instance;

        CountKeys();

        if (singleton.levelStats_castle[0].completed)
        {
            lobby.canSwitchCharacter = true;
            characterSelection.canSwitchCharacter = true;
        }

        keyAmountText.text = keyAmount + " / 99";
    }


    public void CheckLevelsComplete()
    {
        singleton = Singleton.instance;

        CountKeys();

        if (!singleton.seenFinalLevelUnlock)
        {
            lineLeft.fillAmount = 0;
            lineRight.fillAmount = 0;
            lineTop.fillAmount = 0;

            Color btnColor = finalLevelButton.GetComponent<Image>().color;

            finalLevelButton.gameObject.SetActive(false);
            finalLevelButton.interactable = false;
            finalLevelButton.GetComponent<Image>().color = new Color(btnColor.r, btnColor.g, btnColor.b, 0);

            print(keyAmount);

            // If all keys are collected
            if (keyAmount == 99)
            {
                print("All Keys");

                StartCoroutine(DrawLines());
                singleton.seenFinalLevelUnlock = true;
                singleton.Save();
            }
        }
    }


    void CountKeys()
    {
        keyAmount = 0;

        for (int i = 0; i < singleton.levelStats_woods.Count; i++)
            keyAmount += singleton.levelStats_woods[i].starAmount;
        for (int i = 0; i < singleton.levelStats_ice.Count; i++)
            keyAmount += singleton.levelStats_ice[i].starAmount;
        for (int i = 0; i < singleton.levelStats_volcano.Count; i++)
            keyAmount += singleton.levelStats_volcano[i].starAmount;
        for (int i = 0; i < singleton.levelStats_metal.Count; i++)
            keyAmount += singleton.levelStats_metal[i].starAmount;
    }

    //courotine to draw out lines on the map
    IEnumerator DrawLines()
    {
        finalLevelButton.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        while (lineLeft.fillAmount < 1)
        {
            lineLeft.fillAmount += 0.5f * Time.deltaTime;
            lineRight.fillAmount += 0.5f * Time.deltaTime;
            lineTop.fillAmount += 0.5f * Time.deltaTime;
            imgAlpha += 0.5f * Time.deltaTime;

            Color btnColor = finalLevelButton.GetComponent<Image>().color;
            finalLevelButton.GetComponent<Image>().color = new Color(btnColor.r, btnColor.g, btnColor.b, imgAlpha);
            yield return null;
        }

        
        finalLevelButton.interactable = true;
        yield return null;
    }
}
