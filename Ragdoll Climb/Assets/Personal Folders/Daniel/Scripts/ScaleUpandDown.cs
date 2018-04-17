using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpandDown : MonoBehaviour
 {

	Vector3 upScale = new Vector3(1.2f, 1.2f, 1);

	float scaleSize = 0.07f;
	float scaleTIme = 0.05f;

	void Update()
	{
		transform.localScale = new Vector3(Mathf.PingPong (Time.time * scaleTIme, scaleSize), transform.localScale.y, Mathf.PingPong(Time.time * scaleTIme, scaleSize));
	}
}

