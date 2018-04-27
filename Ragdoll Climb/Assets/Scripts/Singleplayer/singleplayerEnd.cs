using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class singleplayerEnd : MonoBehaviour {


    GameObject Player;
    
    

    void OnTriggerEnter(Collider other)
    {
        Player = other.transform.root.gameObject;

        if (Player.tag == "Player"  )
        {
            Player.GetComponent<singleplayerInfo>().onEnd = true;


            int personalBestStars = FindObjectOfType<PlayerInfoSingleton>().levelStats_ice[0].starAmount;
            float personalBestTime = FindObjectOfType<PlayerInfoSingleton>().levelStats_ice[0].bestTime_flt;

            print("end of lvl");
            if(Player.GetComponent<singleplayerInfo>().stars > personalBestStars)
            {
                PlayerInfoSingleton.instance.levelStats_ice[0].starAmount = Player.GetComponent<singleplayerInfo>().stars;
            }

            if (Player.GetComponent<singleplayerInfo>().playtime > personalBestTime)
            {
                FindObjectOfType<PlayerInfoSingleton>().levelStats_ice[0].bestTime_flt = Player.GetComponent<singleplayerInfo>().playtime;
                FindObjectOfType<PlayerInfoSingleton>().levelStats_ice[0].bestTime_str = Player.GetComponent<singleplayerInfo>().playtime.ToString();
            }

            //save data and end lvl
            PlayerInfoSingleton.instance.Save();
            SceneManager.LoadScene("Ice Menu");
        }
    }
}
