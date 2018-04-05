using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThrowable : MonoBehaviour {

	Rigidbody rb;
	[SerializeField] private int throwDamage;
	[SerializeField] private int headDamage;
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}


    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player" && rb.velocity.magnitude > 3)
		{
			if(other.transform.name.Contains("head") || other.transform.name.Contains("Head"))
				other.transform.root.GetComponent<HealthManager>().Damage(headDamage);
			else
				other.transform.root.GetComponent<HealthManager>().Damage(throwDamage);
		}
		

    }
}
