using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeMassCenter : MonoBehaviour
{
    Vector3[] centers;
    Vector3[] tensors;
    Rigidbody[] rbs;

    float startPosZ;

    void Awake ()
    {
        CapsuleCollider[] colls = GetComponentsInChildren<CapsuleCollider>();
        rbs = GetComponentsInChildren<Rigidbody>();
        centers = new Vector3[rbs.Length];
        tensors = new Vector3[rbs.Length];
        startPosZ = transform.position.z;

        for (int i = 0; i < centers.Length; i++)
        {
            centers[i] = rbs[i].centerOfMass;
            tensors[i] = rbs[i].inertiaTensor;

            colls[i].isTrigger = true;
        }
	}

    private void Start()
    {
        for (int i = 0; i < centers.Length; i++)
        {
            rbs[i].centerOfMass = centers[i];
            rbs[i].inertiaTensor = tensors[i];
        }
    }

    private void Update()
    {
        for (int i = 0; i < rbs.Length; i++)
            rbs[i].position = new Vector3(rbs[i].position.x, rbs[i].position.y, startPosZ);
    }
}
