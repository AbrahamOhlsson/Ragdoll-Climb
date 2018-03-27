using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothSetActive : MonoBehaviour
{
	private Cloth cloth;
	private GameObject player;

	void Start ()
	{
		player = gameObject.transform.root.gameObject;

		cloth = GetComponent<Cloth>();
	}

	void Update()
	{
		if (player.activeSelf)
		{
			cloth.enabled = true;
			Destroy(this);
		}
	}

}
