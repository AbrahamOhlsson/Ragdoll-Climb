using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamePosEnd : MonoBehaviour
{
    public GameObject endPos;

	// Use this for initialization
	void Start ()
    {
        endPos = GameObject.Find("EndModul");
	}
	
	// Update is called once per frame
	void Update ()
    {
        endPos.transform.position = transform.position;
        Destroy(this.gameObject);
	}
}
