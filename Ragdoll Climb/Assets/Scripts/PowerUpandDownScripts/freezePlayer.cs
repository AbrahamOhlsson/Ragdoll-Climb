using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezePlayer : MonoBehaviour
{
    GameObject PlayerFreeze;

    
    void OnTriggerEnter(Collider other)
    {
        PlayerFreeze = other.transform.root.gameObject;

        if (PlayerFreeze.tag == "Player")
        {
            if (PlayerFreeze.GetComponent<PlayerInfo>().solid)
            {
                TimeToFreeze();
                Destroy(gameObject);
            }
        }
        
    }


    void TimeToFreeze()
    {
        PlayerFreeze.GetComponent<FreezePlayerPowerUp>().FreezeTime();
    }
}
