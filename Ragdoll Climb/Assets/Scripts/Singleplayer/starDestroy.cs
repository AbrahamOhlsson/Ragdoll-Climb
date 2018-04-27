using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starDestroy : MonoBehaviour {

    GameObject Player;
    bool canGiveStar;

    private void Start()
    {
        canGiveStar = true;    
    }

    void OnTriggerEnter(Collider other)
    {
        Player = other.transform.root.gameObject;

        if (Player.tag == "Player"&& canGiveStar)
        {
            canGiveStar = false;
            Player.transform.GetComponent<singleplayerInfo>().stars++;
            Destroy(gameObject);
            
        }
    }
}
