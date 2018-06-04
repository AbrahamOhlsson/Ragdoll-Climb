using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireWorkColour : MonoBehaviour {

	private Image image;
	public Color[] Fcolours;
	private Color Fcolor;
	Animator anim;

	bool firstOnEnable = true;
	Vector3 startPos;
	Vector3 startScale;


	void Start ()
	{
		anim = GetComponent<Animator>();
		image = GetComponent<Image>();

		startPos = transform.position;
		startScale = transform.localScale;
	}


	private void OnEnable()
	{
		if (!firstOnEnable)
		{
			if (Fcolours == null || Fcolours.Length < 2)
			{
				Debug.Log("Need to setup colors array in inspector");
			}

			RandomizeThings();

			StartCoroutine(DelayAnim());
		}
		else
			firstOnEnable = false;
	}


	IEnumerator DelayAnim()
	{
		float delay = Random.Range(0.5f, 2f);

		yield return new WaitForSeconds(delay);

		anim.Play("FireWorks");

		yield return new WaitForSeconds(1f);

		RandomizeThings();

		anim.Play("FireWorks");
	}


	private void RandomizeThings()
	{
		image.color = Fcolours[Random.Range(0, Fcolours.Length)];

		float randScale = Random.Range(0.5f, 1.5f);

		//transform.localScale = startScale * randScale;

		float randPosX = Random.Range(-50, 50);
		float randPosY = Random.Range(-50, 50);

		transform.position = startPos + new Vector3(randPosX, randPosY, 0);
	}
}
