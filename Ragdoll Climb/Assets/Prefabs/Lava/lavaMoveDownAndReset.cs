using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaMoveDownAndReset : MonoBehaviour {

    [SerializeField]float fallSpeed;

    [SerializeField] Transform topObj;
    [SerializeField] GameObject botomObj;

	void Update () {

        transform.position = transform.position + ((-Vector3.up * fallSpeed) * Time.deltaTime);
        
        if (transform.position.y < botomObj.transform.position.y)
        {      
           transform.position = transform.position -  Vector3.up * ((botomObj.transform.position.y - topObj.position.y));
        }
	}
}
