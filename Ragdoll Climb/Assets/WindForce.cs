using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour
{
    [Range(0f, 100f)]
    public float force = 10f;

    List<Rigidbody> rbs = new List<Rigidbody>();


    private void FixedUpdate()
    {
        for (int i = 0; i < rbs.Count; i++)
        {
            rbs[i].AddForce(transform.forward * force);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        rbs.Add(other.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
        rbs.Remove(other.GetComponent<Rigidbody>());
    }
}
