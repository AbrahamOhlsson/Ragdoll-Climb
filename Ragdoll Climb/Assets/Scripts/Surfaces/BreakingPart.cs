using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPart : MonoBehaviour
{
    [SerializeField] float shrinkSpeed = 0.1f;

    //float stunTime = 2f;
    float scale;

    bool falling = false;

    Rigidbody rb;


    private void Start()
    {
        scale = transform.localScale.x;

        rb = GetComponent<Rigidbody>();
    }
    

    public void Break(float force)
    {
        rb.isKinematic = false;

        float rndForceX = Random.Range(-10f, 10f);

        float rndTorqueX = Random.Range(-50, 50);
        float rndTorqueY = Random.Range(-50, 50);
        float rndTorqueZ = Random.Range(-50, 50);

        rb.AddForce(rndForceX, 0f, -force);
        rb.AddTorque(rndTorqueX, rndTorqueY, rndTorqueZ);

        falling = true;

        //StartCoroutine(Fall());
    }


    private void Update()
    {
        if (scale > 0 && falling)
        {
            scale -= shrinkSpeed * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);

            if (scale <= 0)
            {
                transform.parent.GetComponent<BreakingSurface>().ReleaseHands();
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if (falling && other.tag == "Player")
        //{
        //    other.transform.root.GetComponent<PlayerStun>().Stun(stunTime);
        //}
    }


    IEnumerator Fall()
    {
        while (scale > 0)
        {
            scale -= shrinkSpeed * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);

            if (scale <= 0)
            {
                transform.parent.GetComponent<BreakingSurface>().ReleaseHands();
                Destroy(gameObject);
            }

            yield return null;
        }
    }
}
