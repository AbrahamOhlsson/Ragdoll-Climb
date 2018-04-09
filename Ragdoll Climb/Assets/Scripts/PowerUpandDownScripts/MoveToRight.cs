
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRight : MonoBehaviour {

	int moveX = 3;

	[SerializeField]float timer = 33;

	void Update ()
	{
		transform.position = new Vector3(transform.position.x + moveX * Time.deltaTime, transform.position.y, transform.position.z);

		timer -= Time.deltaTime;

		if(timer < 0)
		{
			Destroy(gameObject);
		}
	}
}
