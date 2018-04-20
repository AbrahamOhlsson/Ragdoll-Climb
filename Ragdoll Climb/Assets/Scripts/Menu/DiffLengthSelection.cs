using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiffLengthSelection : MonoBehaviour
{
    [SerializeField] WorldMenuManager manager;
    [SerializeField] Slider diffSlider;
    [SerializeField] Slider lengthSlider;
    [SerializeField] Text diffText;
    [SerializeField] Text lengthText;
    [SerializeField] GameObject[] blocks;
    [SerializeField] GameObject[] characterImages;
    //[SerializeField] Sprite[] characterSprites;
    [SerializeField] string[] diffNames;
    [SerializeField] string[] lengthNames;

    PlayerInfoSingleton singleton;


    private void Awake()
    {
        singleton = PlayerInfoSingleton.instance;
    }


    private void OnEnable()
    {
        UpdateDiffSlider();
        UpdateLengthSlider();

        for (int i = 0; i < characterImages.Length; i++)
        {
            characterImages[i].SetActive(i < singleton.playerAmount);
            //characterImages[i].GetComponent<Image>().sprite = characterSprites[singleton.characterIndex[i]];
            characterImages[i].GetComponent<Image>().color = singleton.colors[i];
        }
    }


    public void UpdateDiffSlider()
    {
        diffText.text = diffNames[Mathf.RoundToInt(diffSlider.value)];
        singleton.levelDifficulty = (PlayerInfoSingleton.Difficulties)Mathf.RoundToInt(diffSlider.value);
    }


    public void UpdateLengthSlider()
    {
        lengthText.text = lengthNames[Mathf.RoundToInt(lengthSlider.value - 1)];

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].gameObject.SetActive(i <= lengthSlider.value - 1);
        }

        singleton.levelLength = (PlayerInfoSingleton.Lengths)Mathf.RoundToInt(lengthSlider.value);
    }


    public void ResetValues()
    {
        diffSlider.value = 0;
        lengthSlider.value = 1;
    }
}
