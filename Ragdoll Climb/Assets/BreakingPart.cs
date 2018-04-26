using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPart : MonoBehaviour
{
    [SerializeField] float shrinkSpeed = 0.1f;

    float stunTime = 2f;
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

        float rndX = Random.Range(-10f, 10f);

        rb.AddForce(rndX, 0f, -force);

        falling = true;

        StartCoroutine(Fall());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (falling && other.tag == "Player")
        {
            other.transform.root.GetComponent<PlayerStun>().Stun(stunTime);
        }
    }


    IEnumerator Fall()
    {
        while (scale > 0)
        {
            scale -= shrinkSpeed * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);

            if (scale <= 0)
                Destroy(gameObject);

            yield return null;
        }
    }
}
