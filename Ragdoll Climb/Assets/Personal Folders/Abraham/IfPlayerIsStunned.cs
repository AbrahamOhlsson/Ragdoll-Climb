using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfPlayerIsStunned : MonoBehaviour
{
    bool stunPlayer = false;

    void Update()
    {
        //print(stunPlayer);
        if (stunPlayer == true)
        {
            //print("Boom");
            Destroy(gameObject);
            stunPlayer = false; 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && other.transform.root.GetComponent<PlayerStun>().isStunned == true)
        {
            //print("Stun");
            stunPlayer = true;
        }
    }
}
