﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerMass : MonoBehaviour {
	[SerializeField]
	GameObject PlayerCol;
	[SerializeField]
	private float MassPercent;

	//floating 
	[SerializeField]
	private float rotateSpeed = 15.0f;
	[SerializeField]
	private float floatLength = 0.3f;
	[SerializeField]
	private float floatSpeed = 1f;
	Vector3 posOffset = new Vector3();
	Vector3 tempPos = new Vector3();

    bool canColide = true; //Can only colide  

	void Start()
	{
		posOffset = transform.position;
	}

	// Rotate and float up and down
	 void Update()
	{
		// Rotate
		transform.Rotate(new Vector3(0f, Time.deltaTime * rotateSpeed, 0f), Space.World);

		// Float up/down with a Sin()
		tempPos = posOffset;
		tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatSpeed) * floatLength;

		transform.position = tempPos;
	}

	// Set player mass on trigger enter
	void OnTriggerEnter(Collider other)
	{
        if (canColide)
        {

            PlayerCol = other.transform.root.gameObject;

            if (PlayerCol.tag == "Player")
            {
                if (PlayerCol.GetComponent<PlayerInfo>().solid)
                {
                    // get script and then change value of MassPercent
                    PlayerPowerups playerpowerups = PlayerCol.transform.root.gameObject.GetComponent<PlayerPowerups>();
                    playerpowerups.m_MassPercent = MassPercent;

                    //Run function
                    PlayerCol.transform.root.gameObject.GetComponent<PlayerPowerups>().ChangePlayerMass();

                    canColide = false;
                    Destroy(gameObject);
                }
            }

            if (other.tag == "BottomObj")
            {
                canColide = false;
                Destroy(gameObject);
            }

        }
	}
}
