using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomPowerSpawn : MonoBehaviour {

    [SerializeField]
    GameObject [] powerUpDowns;

	// Use this for initialization
	void Start () {

        Instantiate(powerUpDowns[Random.Range(0, powerUpDowns.Length )],new Vector3(transform.position.x, transform.position.y, transform.position.z),Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z));
        Destroy(gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
