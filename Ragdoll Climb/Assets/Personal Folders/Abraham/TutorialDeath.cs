using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDeath : MonoBehaviour
{
    bool deathComeing = false;

    float down = 5;

    public GameObject deathBlock;

	void Update ()
    {
		if(deathComeing == true)
        {
            deathBlock.transform.Translate(Vector3.down * down * Time.deltaTime);
        }
	}
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            deathComeing = true;
        }
    }
}
