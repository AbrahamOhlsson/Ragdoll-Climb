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
    
    bool load = false;
    bool doneOnce = false;


    private void Start()
    {
        canvas = GetComponent<Canvas>();
        
        progressBar.fillAmount = 0;
        progressText.text = "0%";
    }


    private void Update()
    {
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


    IEnumerator LoadAsync(string levelName)
    {
        yield return new WaitForSeconds(0.5f);

        operation = SceneManager.LoadSceneAsync(levelName);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
            progressBar.fillAmount = progress;

            if (punTimer >= punSwitchTime)
                RandomizePun();
            else
                punTimer += Time.deltaTime;
            
            yield return null;
        }
    }


    private void RandomizePun()
    {
        int index = Random.Range(0, puns.Length);

        punText.text = "Pun #" + (index + 1) + "\n" + puns[index];

        punTimer = 0;
    }
}
