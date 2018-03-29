using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCheck : MonoBehaviour
{
    [Tooltip("Is this the left hand?")]
    [SerializeField] bool left = true;

    [Tooltip("Normal damage dealt on punched player.")]
    [Range(0f, 100f)]
    [SerializeField] float damage = 20;

    [Tooltip("Damage multiplier for hittin a head.")]
    [Range(0f, 10f)]
    [SerializeField] float headDmgMult = 2f;

    [Tooltip("Force applied to bodypart that was hit.")]
    [Range(0f, 100000)]
    [SerializeField] float hitForce = 10000;

    [Tooltip("The time it takes until you can hit again after a hit.")]
    [Range(0f, 2f)]
    [SerializeField] float dmgResetDelay = 0.4f;

    [SerializeField] AudioClip punchHitSfx;

    float dmgResetTimer = 0f;

    PlayerController controller;

    AudioSource source;


	void Start ()
    {
        // Gets references
        controller = transform.root.GetComponent<PlayerController>();
        source = transform.root.GetComponent<AudioSource>();
    }


    void Update ()
    {
        dmgResetTimer += Time.deltaTime;
	}


    private void OnCollisionEnter(Collision other)
    {
        // If this hand is punching right now AND if the hit object is a player AND if damage reset timer has reached its goal
        if (((left && controller.leftPunching) || (!left && controller.rightPunching)) && other.gameObject.tag == "Player" && dmgResetTimer >= dmgResetDelay)
        {
            // More damage if head was hit
            if (other.gameObject.name.Contains("Head"))
                other.transform.root.GetComponent<HealthManager>().Damage(damage * headDmgMult);
            else
                other.transform.root.GetComponent<HealthManager>().Damage(damage);

            // Calculates direction from hand to hit object
            Vector2 heading = other.transform.position - transform.position;
            float dist = heading.magnitude;
            Vector2 dir = heading / dist;

            // Pushes hit object with direction
            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * hitForce);

            dmgResetTimer = 0f;

            source.PlayOneShot(punchHitSfx);
        }
    }
}
