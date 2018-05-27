using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaybladeRotator : MonoBehaviour {

    [Range(0, 100)] [Tooltip("The speed of the propeller")]
    public float spinSpeed = 10;

	void Update () {
        transform.Rotate(new Vector3(0, Time.deltaTime * spinSpeed, 0));
	}
}
