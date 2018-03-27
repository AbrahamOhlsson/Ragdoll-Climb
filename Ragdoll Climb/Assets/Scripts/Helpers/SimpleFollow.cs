using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [SerializeField] bool x = true;
    [SerializeField] bool y = false;
    [SerializeField] bool z = false;

    [SerializeField] Vector3 offset = Vector3.zero;

    [SerializeField] Transform target;
    

	void Update ()
    {
        if (x)
        {
            transform.position = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);
        }
        if (y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y + offset.y, transform.position.z);
        }
        if (z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z + offset.z);
        }
	}
}
