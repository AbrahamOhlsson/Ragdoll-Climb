using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [SerializeField] bool x = true;
    [SerializeField] bool y = false;
    [SerializeField] bool z = false;
    [SerializeField] bool physics = false;

    [SerializeField] Vector3 offset = Vector3.zero;

    [SerializeField] Transform target;

    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update ()
    {
        if (physics)
        {
            if (x)
            {
                rb.position = new Vector3(target.position.x + offset.x, rb.position.y, rb.position.z);
            }
            if (y)
            {
                rb.position = new Vector3(rb.position.x, target.position.y + offset.y, rb.position.z);
            }
            if (z)
            {
                rb.position = new Vector3(rb.position.x, rb.position.y, target.position.z + offset.z);
            }
        }
        else
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
}
