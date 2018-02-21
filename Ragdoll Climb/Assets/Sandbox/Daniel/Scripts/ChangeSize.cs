using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{
	[SerializeField]
	GameObject PlayerCol;

	void OnTriggerEnter(Collider other)
	{
		PlayerCol = other.transform.root.gameObject;
		PlayerPowerups playerpowerups = PlayerCol.transform.root.gameObject.GetComponent<PlayerPowerups>();

		if (PlayerCol.tag == "Player")
		{

		}
		Destroy(gameObject);
	}

	
}
