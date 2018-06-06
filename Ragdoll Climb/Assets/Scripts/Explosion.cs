using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float maxForce = 5000f;

    [SerializeField] float maxDmg;

    bool exploding = true;

    float radius;

    SphereCollider coll;


	void Awake ()
    {
        coll = GetComponent<SphereCollider>();

        radius = coll.radius;

        coll.radius = 0;

        GameObject.Find("music and sound").GetComponent<soundManager>().PlaySoundRandPitch("Explosion");
    }


    void Update ()
    {
        coll.radius = Mathf.Lerp(coll.radius, radius, 0.5f);

        if (coll.radius >= radius - 0.001f)
            exploding = false;
	}


    private void OnTriggerEnter(Collider other)
    {
        if (exploding && other.gameObject.GetComponent<Rigidbody>())
        {
            if (other.tag == "Player" && other.transform.root.GetComponent<PlayerInfo>().solid)
                other.transform.root.GetComponent<PlayerStun>().Stun(2.5f);

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            float dist = Vector3.Distance(rb.position, transform.position);
            Vector3 dir = (rb.position - transform.position).normalized;
            dir.z = 0;

            rb.AddForce(dir * dist * maxForce);

            //rb.AddExplosionForce(maxForce, transform.position, radius, 0f);
        }
    }
}
