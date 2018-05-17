using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetZ : MonoBehaviour
{
    Rigidbody[] rbs;
    float[] startZ;

	void Start ()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        startZ = new float[rbs.Length];
        
        for (int i = 0; i < rbs.Length; i++)
        {
            startZ[i] = rbs[i].transform.position.z;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].transform.position = new Vector3(rbs[i].transform.position.x, rbs[i].transform.position.y, startZ[i]);
        }
    }
}
