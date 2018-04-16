using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThrowable : MonoBehaviour
{
    internal bool beingHold = false;

    [SerializeField] private float reqVelocity = 5f;
	[SerializeField] private int throwDamage;
	[SerializeField] private int headDamage;
    
	Rigidbody rb;


	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}
    

    void OnCollisionEnter(Collision other)
    {
        if(!beingHold && other.transform.tag == "Player" && rb.velocity.magnitude > reqVelocity)
		{
			if(other.transform.name.Contains("head") || other.transform.name.Contains("Head"))
				other.transform.root.GetComponent<HealthManager>().Damage(headDamage);
			else
				other.transform.root.GetComponent<HealthManager>().Damage(throwDamage);
		}
    }
}
