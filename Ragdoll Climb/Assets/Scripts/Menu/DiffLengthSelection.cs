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
    [SerializeField] string[] diffNames;
    [SerializeField] string[] lengthNames;


    private void Start()
    {
        UpdateDiffSlider();
        UpdateLengthSlider();
    }


    public void UpdateDiffSlider()
    {
        diffText.text = diffNames[Mathf.RoundToInt(diffSlider.value)];
        PlayerInfoSingleton.instance.levelifficulty = (PlayerInfoSingleton.Difficulties)Mathf.RoundToInt(diffSlider.value);
    }


    public void UpdateLengthSlider()
    {
        lengthText.text = lengthNames[Mathf.RoundToInt(lengthSlider.value - 1)];
        PlayerInfoSingleton.instance.levelLength = (PlayerInfoSingleton.Lengths)Mathf.RoundToInt(lengthSlider.value);
    }


    public void ResetValues()
    {
        diffSlider.value = 0;
        lengthSlider.value = 1;
    }
}
