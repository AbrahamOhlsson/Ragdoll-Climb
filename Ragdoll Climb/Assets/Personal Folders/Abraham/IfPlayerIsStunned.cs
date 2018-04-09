using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfPlayerIsStunned : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && other.transform.root.GetComponent<PlayerStun>().isStunned == true)
        {
            Destroy(gameObject);
        }
    }
}
