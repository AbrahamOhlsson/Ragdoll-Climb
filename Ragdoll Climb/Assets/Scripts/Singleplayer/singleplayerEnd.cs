using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class singleplayerEnd : MonoBehaviour
{
    GameObject Player;

    public List<SP_LevelStats> levelStats;

    string world = "";
    int levelIndex = 0;

    SP_ResultsMenu resultsMenu;

    Singleton singleton;


    private void Awake()
    {
        singleton = Singleton.instance;

        resultsMenu = GameObject.Find("SP Results Canvas").GetComponent<SP_ResultsMenu>();

        world = singleton.currSpWorld;
        levelIndex = singleton.currSpLevelIndex;

        if (world == "ice")
            levelStats = singleton.levelStats_ice;
        else if (world == "lava")
            levelStats = singleton.levelStats_volcano;
        else if (world == "woods")
            levelStats = singleton.levelStats_woods;
        else if (world == "metal")
            levelStats = singleton.levelStats_metal;
    }

    void OnTriggerEnter(Collider other)
    {
        Player = other.transform.root.gameObject;

        if (Player.tag == "Player"  )
        {
            Player.GetComponent<singleplayerInfo>().onEnd = true;

            float myTime = Player.GetComponent<singleplayerInfo>().playtime;

            print("first my time = " + myTime);
            myTime -=  myTime % 0.01f;
            print("my time = " + myTime);

            int personalBestStars = 0;
            float personalBestTime = 0;
            
            personalBestStars = levelStats[levelIndex].starAmount;
            personalBestTime = levelStats[levelIndex].bestTime_flt;

            if(Player.GetComponent<singleplayerInfo>().stars > personalBestStars)
            {
                levelStats[singleton.currSpLevelIndex].starAmount = Player.GetComponent<singleplayerInfo>().stars;
            }

            if (Player.GetComponent<singleplayerInfo>().playtime < personalBestTime)
            {
                levelStats[levelIndex].bestTime_flt = Player.GetComponent<singleplayerInfo>().playtime;
                levelStats[levelIndex].bestTime_str = (Player.GetComponent<singleplayerInfo>().playtime).ToString();
            }

            Cursor.visible = true;

            //save data and end lvl
            singleton.Save();
            //SceneManager.LoadScene("Ice Menu");
            resultsMenu.Activate(Player.GetComponent<singleplayerInfo>());

            Player.SetActive(false);
        }
    }
}
