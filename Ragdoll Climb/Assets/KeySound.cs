using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySound : MonoBehaviour
{
    soundManager soundManager;

	// Use this for initialization
	void Start ()
    {
        soundManager = FindObjectOfType<soundManager>();
    }

    public void PlaySound()
    {
        soundManager.PlaySound("goodClimb");
    }
}
