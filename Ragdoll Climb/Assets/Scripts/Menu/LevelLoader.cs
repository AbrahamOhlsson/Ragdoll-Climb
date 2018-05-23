using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] Image progressBar;
    [SerializeField] Text progressText;
    [SerializeField] Text punText;
    [SerializeField] string[] puns;
    [SerializeField] float punSwitchTime = 5;
    [SerializeField] Animator[] leftPulsing;
    [SerializeField] Animator[] rightPulsing;
    [SerializeField] float[] animDelays;

    Canvas canvas;

    AsyncOperation operation;

    float punTimer = 0;

    float animTimer = 0;
    

    private void Start()
    {
        // Reset stuff
        canvas = GetComponent<Canvas>();
        progressBar.fillAmount = 0;
        progressText.text = "0%";
    }


    private void Update()
    {
        // Delays start of animations to make them flow
        for (int i = 0; i < animDelays.Length; i++)
        {
            if (animTimer >= animDelays[i])
            {
                leftPulsing[i].Play("LoadingPulse");
                rightPulsing[i].Play("LoadingPulse");
            }
        }

        animTimer += Time.deltaTime;
    }


    public void LoadLevelAsync(string levelName)
    {
        canvas.enabled = true;

        RandomizePun();

        StartCoroutine(LoadAsync(levelName));
    }


    // Loads level asyncroniously and manages loading sreen
    IEnumerator LoadAsync(string levelName)
    {
        Time.timeScale = 1f;

        yield return new WaitForSeconds(0.2f);

        operation = SceneManager.LoadSceneAsync(levelName);

        // 
        while(!operation.isDone)
        {
            // Progress goes from 0 to 1, instead of 0 to 0.9
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Sets precentage text
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            // Fills progress bar 
            progressBar.fillAmount = progress;

            // Randomizes a new pun after some time
            if (punTimer >= punSwitchTime)
                RandomizePun();
            else
                punTimer += Time.deltaTime;
            
            yield return null;
        }
    }


    // Gets a new pun and sets it to the text
    private void RandomizePun()
    {
        int index = Random.Range(0, puns.Length);

        punText.text = "Pun #" + (index + 1) + "\n" + puns[index];

        punTimer = 0;
    }
}
