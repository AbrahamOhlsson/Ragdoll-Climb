using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour
{
    [Range(0f, 100f)]
    public float force = 10f;

    [Range(0f, 1f)]
    public float armForceMult = 0.1f;

    List<Rigidbody> rbs = new List<Rigidbody>();


    private void FixedUpdate()
    {
        for (int i = 0; i < rbs.Count; i++)
        {
            if (rbs[i].gameObject.layer == 8)
            {
                rbs[i].AddForce(transform.forward * force * armForceMult);
            }
            else
            {
                rbs[i].AddForce(transform.forward * force);
                print(rbs[i].transform.root.name + "HAHHAHAHAHAHAHA");
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() && (other.tag == "Player" || other.tag == "Throwable") && !rbs.Exists(x => x == other.GetComponent<Rigidbody>()))
            rbs.Add(other.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.GetComponent<Rigidbody>() && (other.tag == "Player" || other.tag == "Throwable") && !rbs.Exists(x => x == other.GetComponent<Rigidbody>()))
			rbs.Remove(other.GetComponent<Rigidbody>());
    }
}
