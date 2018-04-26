using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleGrowthTrigger : MonoBehaviour
{
    Icicle_NonDrop icicle;


    private void Awake()
    {
        icicle = GetComponentInParent<Icicle_NonDrop>();
        transform.SetParent(transform.parent.parent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            icicle.grow = true;
            Destroy(gameObject);
        }
    }
}
