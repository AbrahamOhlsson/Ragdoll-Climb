using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezePlayer : MonoBehaviour
{
    GameObject PlayerFreeze;
    soundManager soundManager;

    private void Start()
    {

        soundManager = GameObject.Find("music and sound").GetComponent<soundManager>();
    
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerFreeze = other.transform.root.gameObject;

        if (PlayerFreeze.tag == "Player")
        {
            if (PlayerFreeze.GetComponent<PlayerInfo>().solid)
            {
                soundManager.PlaySoundRandPitch("iceCube");

                TimeToFreeze();
                Destroy(gameObject);

            }
        }
    }


    void TimeToFreeze()
    {
        PlayerFreeze.GetComponent<FreezePD>().StartFreeze(3f, false);
    }
}
