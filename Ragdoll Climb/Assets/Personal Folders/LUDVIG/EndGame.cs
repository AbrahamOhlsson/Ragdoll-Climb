using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
	void Update () {
		
        //if (Input.GetKeyDown("escape"))
        //{
        //    Application.Quit();
        //}

        if (Input.GetKeyDown("r"))
        {
            if (PlayerInfoSingleton.instance.debug)
                PlayerInfoSingleton.instance.playerAmount = 0;

            Scene thisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(thisScene.buildIndex);
        }

    }
}
