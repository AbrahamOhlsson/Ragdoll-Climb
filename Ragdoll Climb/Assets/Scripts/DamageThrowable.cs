using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThrowable : MonoBehaviour
{
    internal int holdAmount = 0;    // Amount of hand holding this object
    internal float startZPos;       // Position this object should be at when released

    [SerializeField] private float minVelocity = 5f;    // Minimum velocity to do any damage at all
    [SerializeField] private float maxVelocity = 30f;   // Minimum velocity that grants highest possible damage
	[SerializeField] private float headDmgMult = 2f;    // Damage multiplier for hitting heads

    private float throwDamage;
    
	Rigidbody rb;


	void Start ()
	{
        startZPos = transform.position.z;

        rb = GetComponent<Rigidbody>();
	}
    

    void OnCollisionEnter(Collision other)
    {
        // If this object isn't being hold AND if a player was hit AND if the velocity was great enough
        if(holdAmount <= 0 && other.transform.tag == "Player" && rb.velocity.magnitude >= minVelocity)
		{
            // Calculates damage, based on the velocity
            throwDamage = 100 * (rb.velocity.magnitude / maxVelocity);

            // Doubles damage if head was hit
            if (other.transform.name.Contains("head") || other.transform.name.Contains("Head"))
                throwDamage *= headDmgMult;

            // Damages player
			other.transform.root.GetComponent<HealthManager>().Damage(throwDamage);
		}
    }
}
