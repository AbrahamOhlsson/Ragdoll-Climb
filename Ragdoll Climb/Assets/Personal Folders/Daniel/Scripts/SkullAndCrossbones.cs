using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAndCrossbones : MonoBehaviour {

	Transform trans;

	private bool ScaleUp;
	private bool Go;

	void Start ()
	{
		ShowDeath();
	}


	public void ShowDeath()
	{
		trans = GetComponent<Transform>();

		trans.localScale = new Vector3(0.01f, 0.01f, 0.01f);

		ScaleUp = true;
		Go = true;
	}


	void Update ()
	{
		if (ScaleUp)
		{
			trans.localScale = Vector3.Lerp(trans.localScale, new Vector3(0.1f,0.1f,0.1f), 0.1f);
		}

		if(Go)
		{
			StartCoroutine(Wait());

			Go = false;
		}

		if (ScaleUp == false)
		{
			trans.localScale = Vector3.Lerp(trans.localScale, new Vector3(0.01f,0.01f,0.01f) , 0.1f);
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(3);

		ScaleUp = false;

		yield return new WaitForSeconds(1);

		Destroy(gameObject);
	}
}
