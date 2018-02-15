using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerMass : MonoBehaviour {
	[SerializeField]
	GameObject PlayerCol;
	[SerializeField]
	private float MassPercent;
	private float rotSpeed;

	//Float
	private float degreesPerSecond = 15.0f;
	private float amplitude = 0.3f;
	private float frequency = 1f;
	Vector3 posOffset = new Vector3();
	Vector3 tempPos = new Vector3();

	void Start()
	{
		// Rotation speed
		rotSpeed = 50;

		posOffset = transform.position;
	}

	// Rotate and float up and down
	 void Update()
	{
		// Rotatie
		transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);

		transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

		// Float up/down with a Sin()
		tempPos = posOffset;
		tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

		transform.position = tempPos;
	}

	// Set player mass on trigger enter
	void OnTriggerEnter(Collider other)
	{
		PlayerCol = other.transform.root.gameObject;

		if (PlayerCol.tag == "Player")
		{ 
		// get script and then change value of MassPercent
		GetPlayerMass getplayermass = PlayerCol.transform.root.gameObject.GetComponent<GetPlayerMass>();
		getplayermass.m_MassPercent = MassPercent;

		//Run function
		PlayerCol.transform.root.gameObject.GetComponent<GetPlayerMass>().ChangePlayerMass();
		}
		Destroy(gameObject);
	}
}
