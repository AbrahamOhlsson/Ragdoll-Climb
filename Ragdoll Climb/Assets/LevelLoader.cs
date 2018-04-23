using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] Image progressBar;
    [SerializeField] Text progressText;

    Canvas canvas;


    private void Start()
    {
        canvas = GetComponent<Canvas>();

        progressBar.fillAmount = 0;
        progressText.text = "0%";
        canvas.enabled = false;
    }


    public void LoadLevelAsync(string levelName)
    {
        //StartCoroutine(LoadAsync(levelName));
        SceneManager.LoadScene(levelName);
    }


    IEnumerator LoadAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        //operation.allowSceneActivation = false;

        canvas.enabled = true;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            print(progress);

            progressText.text = progress * 100f + "%";
            progressBar.fillAmount = progress;

            //if (operation.progress >= 0.9f)
            //    operation.allowSceneActivation = true;
        }

        yield return null;
    }
	
}
