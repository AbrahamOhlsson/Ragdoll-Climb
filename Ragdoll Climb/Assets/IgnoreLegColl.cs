using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreLegColl : MonoBehaviour
{
    [SerializeField] Collider knee;
    [SerializeField] Collider hip;


    void Start ()
    {
        Collider coll = GetComponent<Collider>();

        Physics.IgnoreCollision(coll, knee);
        Physics.IgnoreCollision(coll, hip);
    }
}
