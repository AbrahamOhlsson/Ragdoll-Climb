using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfPlayerIsStunned : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && other.transform.root.GetComponent<PlayerStun>().isStunned == true)
        {
            //print("Stun");
            Destroy(gameObject);
        }
    }
}
