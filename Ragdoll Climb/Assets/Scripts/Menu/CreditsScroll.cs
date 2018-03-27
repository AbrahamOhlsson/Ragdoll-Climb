using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroll : MonoBehaviour {

	[SerializeField] private float scrollSpeed = 10; 
	private Vector3 originPos;

	void Start()
	{
		originPos = transform.localPosition;
	}

	void Update ()
	{
		transform.Translate(new Vector3(0,scrollSpeed) * Time.deltaTime);

		if (transform.localPosition.y >= 400)
		{
			transform.localPosition = originPos;
		}
	}
}
