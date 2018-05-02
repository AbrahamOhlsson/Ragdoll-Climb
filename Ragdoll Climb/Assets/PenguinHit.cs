using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinHit : MonoBehaviour
{
    Penguin penguinScript;

	// Use this for initialization
	void Start ()
    {
        penguinScript = transform.parent.GetComponent<Penguin>();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (penguinScript.state == Penguin.PenguinStates.Launched)
        {
            if (other.transform.tag == "Player")
                other.transform.root.GetComponent<PlayerStun>().Stun(2f);
            
            penguinScript.Respawn();
        }
    }
}
