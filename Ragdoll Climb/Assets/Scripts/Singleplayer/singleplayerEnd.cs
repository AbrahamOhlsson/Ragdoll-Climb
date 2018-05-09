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


            int personalBestStars = FindObjectOfType<Singleton>().levelStats_ice[0].starAmount;
            float personalBestTime = FindObjectOfType<Singleton>().levelStats_ice[0].bestTime_flt;

            print("end of lvl");
            if(Player.GetComponent<singleplayerInfo>().stars > personalBestStars)
            {
                Singleton.instance.levelStats_ice[0].starAmount = Player.GetComponent<singleplayerInfo>().stars;
            }

            if (Player.GetComponent<singleplayerInfo>().lvlTime - Player.GetComponent<singleplayerInfo>().playtime < personalBestTime)
            {
                FindObjectOfType<Singleton>().levelStats_ice[0].bestTime_flt = Player.GetComponent<singleplayerInfo>().lvlTime - Player.GetComponent<singleplayerInfo>().playtime;
                FindObjectOfType<Singleton>().levelStats_ice[0].bestTime_str = (Player.GetComponent<singleplayerInfo>().lvlTime - Player.GetComponent<singleplayerInfo>().playtime).ToString();
            }

            Cursor.visible = true;

            //save data and end lvl
            Singleton.instance.Save();
            SceneManager.LoadScene("Ice Menu");
        }
    }
}
