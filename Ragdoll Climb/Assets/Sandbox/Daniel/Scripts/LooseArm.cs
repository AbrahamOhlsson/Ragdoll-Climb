using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBig : MonoBehaviour {
	[SerializeField]
	GameObject PlayerCol;

	private float size;
	
	void OnTriggerEnter(Collider other)
	{
		PlayerCol = other.transform.root.gameObject;

		if (PlayerCol.tag == "Player")
		{
			StartCoroutine(DisableArm());
		}
		//Destroy(gameObject);
	}

	IEnumerator DisableArm()
	{
		PlayerCol.transform.localScale *= size;

		yield return new WaitForSeconds(3);

    }
}
