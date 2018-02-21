using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerups : MonoBehaviour
{
	////// Add/loose mass
	public Component[] Rigidbodies;
	public List<float> startMass;
	public int pos = 0;
	[SerializeField]
	public float m_MassPercent;
	[SerializeField]
	private int MassDuration;
	///////////////////////////////////////
	public ParticleSystem AddMassParticle;
	public ParticleSystem LooseMassParticle;
	///////////////////////////////////////

	///////////////////////////////////////

	//Change player mass//
	//-------------------------------------------------------------------------------------//
	public void ChangePlayerMass()
	{

		Rigidbodies = GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rigidbodymass in Rigidbodies)
		{
			startMass.Add(rigidbodymass.mass);
			;
		}

		StartCoroutine(ChangeMass());

	}

	IEnumerator ChangeMass()
	{
		foreach (Rigidbody rigidbody in Rigidbodies)
		{
			rigidbody.mass = startMass[pos] * m_MassPercent;
			pos++;
		}
		if (m_MassPercent < 1)
		{
			LooseMassParticle.Play();
			AddMassParticle.Stop();
		}

		if (m_MassPercent > 1)
		{
			AddMassParticle.Play();
			LooseMassParticle.Stop();
		}
		pos = 0;

		yield return new WaitForSeconds(MassDuration);

		LooseMassParticle.Stop();
		AddMassParticle.Stop();

		foreach (Rigidbody rigidbodymass in Rigidbodies)
		{
			rigidbodymass.mass = startMass[pos];
			pos++;
		}
		pos = 0;
	}
	//-----------------------------------------------------------------------//
}
