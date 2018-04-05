using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderDelete : MonoBehaviour {

	MeshCollider[] meshColls;

	void Start ()
	{
		meshColls = GetComponents<MeshCollider>();

		if(meshColls.Length > 0)
		{
			for (int i = 0; i < meshColls.Length; i++)
			{
				Destroy(meshColls[i]);
			}
		}
	}
	
}
