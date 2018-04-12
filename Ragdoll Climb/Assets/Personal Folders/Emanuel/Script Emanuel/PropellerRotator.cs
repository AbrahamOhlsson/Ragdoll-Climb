using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerRotator : MonoBehaviour {

    [Range(0, 100)] [Tooltip("The speed of the propeller")]
    public float rotatingSpeed = 10;

	void Update () {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotatingSpeed));
	}
}
