using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndFloat : MonoBehaviour {

	//floating 
	[SerializeField]
	private float rotateSpeed = 15.0f;
	[SerializeField]
	private float floatLength = 0.3f;
	[SerializeField]
	private float floatSpeed = 1f;
	float startPosY;
	float tempPosY;


	void Start()
	{
		startPosY = transform.localPosition.y;
	}

	// Rotate and float up and down
	void Update()
	{
		// Rotate
		transform.Rotate(new Vector3(0f, Time.deltaTime * rotateSpeed, 0f), Space.World);

		// Float up/down with a Sin()
		tempPosY = startPosY;
		tempPosY += Mathf.Sin(Time.fixedTime * Mathf.PI * floatSpeed) * floatLength;

		transform.localPosition = new Vector3(transform.localPosition.x, tempPosY, transform.localPosition.z);
	}
}
