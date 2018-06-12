using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    playerSound soundManager;

    float dmgResetTimer = 0f;

    PlayerController controller;
    
    public AudioMixerGroup myMixGroup;

    public AudioMixer mixer;
     
   
    void Start ()
    {
       // mixer = Resources.Load("TEST") as AudioMixer;
       
        controller = transform.root.GetComponent<PlayerController>();
        //print(mixer.name + " test namn");
        //source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
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

            int hitIndex = Random.Range(1, 7);
            transform.root.GetComponent<playerSound>().PlaySoundRandPitch("PunchHit" + hitIndex);
        }
    }
}
