using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseRope : MonoBehaviour
{
	[SerializeField]
	private Rigidbody ropeRb;
	private Rigidbody nailRb;

	[SerializeField]
	BreakingSurface breakingSurface;

	private float nailForce = -20;
	private float nailRotation = -30;


	void Start ()
	{
		nailRb = gameObject.GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
		if(breakingSurface.broken)
		{
			RopeSetFree();
			nailRb.AddForce(transform.up * nailForce);
			nailRb.AddTorque(transform.right * nailRotation);
		}
	}

	void RopeSetFree()
	{
		ropeRb.isKinematic = false;
		nailRb.isKinematic = false;
		
	}

}
