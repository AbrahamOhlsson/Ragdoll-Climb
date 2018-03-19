using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    [SerializeField] bool lookAtCamera;
    [SerializeField] bool lockRotation;

    Quaternion initialRotation;

	void Start ()
    {
        initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (lookAtCamera)
        {
            transform.LookAt(-Camera.main.transform.position);
        }

        if (lockRotation)
        {
            transform.rotation = initialRotation;
        }
	}
}
