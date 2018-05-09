using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepTransform : MonoBehaviour
{
    [SerializeField] bool keepPosition = false;
    [SerializeField] bool keepRotation = false;
    [SerializeField] bool keepScale = false;

    Vector3 pos;
    Vector3 scale;
    Quaternion rot;

	// Use this for initialization
	void Start ()
    {
        pos = transform.position;
        rot = transform.rotation;
        scale = transform.lossyScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (keepPosition)
            transform.position = pos;

        if (keepRotation)
            transform.rotation = rot;

        if (keepScale)
            transform.localScale = scale;
	}
}
