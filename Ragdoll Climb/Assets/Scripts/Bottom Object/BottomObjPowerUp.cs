using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomObjPowerUp : MonoBehaviour {

	[SerializeField] float spawnOffset = 10; 
	[SerializeField] GameObject[] powerUps;
	[SerializeField] Transform powerupParent;
	Transform lastTrans; // ändra till vector3 från transform så vi inte får error 

	int index;

	private void Start()
	{
		lastTrans = Instantiate(powerUps[index], transform.position, transform.rotation, powerupParent).AddComponent<MoveToRight>().transform;
	}

	void Update()
	{

		index = Random.Range(0, powerUps.Length);
		
		if(Vector3.Distance(lastTrans.position, transform.position) >= spawnOffset)
		{
			lastTrans = Instantiate(powerUps[index], transform.position, transform.rotation, powerupParent).AddComponent<MoveToRight>().transform;
		}
	}
}
