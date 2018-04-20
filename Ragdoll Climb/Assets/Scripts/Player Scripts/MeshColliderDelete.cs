using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderDelete : MonoBehaviour
{
	MeshCollider[] meshColls;
    public float updateTIME;

    private void Start()
    {
        updateTIME = 0;
    }

    private void Update()
    {
        if ( updateTIME> 1)
        {
            meshColls = GetComponents<MeshCollider>();

            if (meshColls.Length > 0)
            {
                for (int i = 0; i < meshColls.Length; i++)
                {
                    Destroy(meshColls[i]);
                }

                Destroy(this);
            }
        }

        updateTIME += 1*Time.deltaTime;

    }

}
