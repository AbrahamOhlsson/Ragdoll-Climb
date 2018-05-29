using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStartColour : MonoBehaviour {


	private Renderer rend;
	[SerializeField]
	private Renderer CRend;

	public Color[] colors;

	void Start()
	{
		rend = GetComponent<Renderer>();

		if (colors == null || colors.Length < 2)
		{
			Debug.Log("Need to setup colors array in inspector");
		}

		rend.material.color = colors[Random.Range(0, colors.Length)];
		CRend.material.color = rend.material.color;

	}

}