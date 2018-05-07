using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportBoatCheck : MonoBehaviour
{

    public GameObject bottomObj;

    // Use this for initialization
    void Start ()
    {
		bottomObj = GameObject.Find("Bottom Object");
	}


}
