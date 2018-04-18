using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThrowable : MonoBehaviour
{
    internal int holdAmount = 0;
    internal float startZPos;

    [SerializeField] private float minVelocity = 5f;
    [SerializeField] private float maxVelocity = 30f;
	[SerializeField] private float headDmgMult = 2f;

    private float throwDamage;
    
	Rigidbody rb;


	void Start ()
	{
        startZPos = transform.position.z;

        rb = GetComponent<Rigidbody>();
	}
    

    void OnCollisionEnter(Collision other)
    {
        if(holdAmount <= 0 && other.transform.tag == "Player" && rb.velocity.magnitude >= minVelocity)
		{
            throwDamage = 100 * (rb.velocity.magnitude / maxVelocity);

            if (other.transform.name.Contains("head") || other.transform.name.Contains("Head"))
                throwDamage *= headDmgMult;

			other.transform.root.GetComponent<HealthManager>().Damage(throwDamage);
		}
    }
}
