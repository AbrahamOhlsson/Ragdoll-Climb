using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerMass : MonoBehaviour
{
	public Component[] Rigidbodies;
	public List<float> startMass;
	public int pos = 0;
	[SerializeField]
	private float MassValue;
	[SerializeField]
	private int MassDuration;

	void Start()
	{

		Rigidbodies = GetComponentsInChildren<Rigidbody>();
		

		foreach (Rigidbody rigidbodymass in Rigidbodies)
		{
			startMass.Add( rigidbodymass.mass) ;
            ;
        }		

		StartCoroutine(ChangeMass());
	}

	IEnumerator ChangeMass()
	{
		foreach (Rigidbody rigidbody in Rigidbodies)
		{
			rigidbody.mass = MassValue;
		}

		yield return new WaitForSeconds(MassDuration);

		foreach (Rigidbody rigidbodymass in Rigidbodies)
		{
			rigidbodymass.mass = startMass[pos];
			pos++;
		}
		pos = 0;
	}
}
