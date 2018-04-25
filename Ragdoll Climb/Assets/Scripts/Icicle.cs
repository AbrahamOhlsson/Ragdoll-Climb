using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    [SerializeField] float growthSpeed = 0.1f;
    [SerializeField] GameObject icicleShatterEffect;

    bool growing = true;

    float targetScale;
    float minScale = 0;
    float scale;

    Vector3 startPos;
    Vector3 particleOffset = new Vector3(0f, -1f, 0f);

    soundManager soundManager;

    Rigidbody rb;


	void Start ()
    {
        startPos = transform.position;

        targetScale = transform.localScale.x;
        transform.localScale = new Vector3(minScale, minScale, minScale);
        scale = transform.localScale.x;

        soundManager = GameObject.Find("music and sound").GetComponent<soundManager>();

        rb = GetComponent<Rigidbody>();
    }
	
	void Update ()
    {
		if (growing)
        {
            scale += growthSpeed * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);

            if (scale >= targetScale)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                growing = false;
            }
        }
	}

    private void OnCollisionEnter(Collision other)
    {
        if (!growing)
        {
            if (other.gameObject.tag == "Player")
            {
                other.transform.root.GetComponent<HealthManager>().Damage(100f);
            }

            Instantiate(icicleShatterEffect, transform.position + particleOffset, Quaternion.identity);
            soundManager.PlaySound("icicleShatter");

            scale = minScale;
            transform.localScale = new Vector3(minScale, minScale, minScale);
            transform.position = startPos;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
            growing = true;
        }
    }
}
