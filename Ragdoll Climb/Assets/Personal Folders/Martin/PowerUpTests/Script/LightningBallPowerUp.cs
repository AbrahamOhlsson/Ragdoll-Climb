using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBallPowerUp : MonoBehaviour {

    GameObject Player;

    void OnTriggerEnter(Collider other)
    {
       Player = other.transform.root.gameObject;

        if (Player.tag == "Player")
        {
            StartOrbit();
            Destroy(gameObject);
        }

    }

    void StartOrbit()
    {
        Player.GetComponent<LightningBall>().Initiation();
    }
}
