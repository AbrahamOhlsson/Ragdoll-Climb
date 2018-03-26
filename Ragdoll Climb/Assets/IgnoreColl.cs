using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColl : MonoBehaviour
{
    [SerializeField] Collider[] colls;


    void Start ()
    {
        Collider coll = GetComponent<Collider>();
        
        for (int i = 0; i < colls.Length; i++)
        {
            Physics.IgnoreCollision(coll, colls[i]);
        }
    }
}
