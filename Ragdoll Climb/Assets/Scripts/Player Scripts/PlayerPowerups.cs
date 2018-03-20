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
    ////// Teleport player
    [SerializeField]
    GameObject rightGrabObject;
    [SerializeField]
    GameObject leftGrabObject;
    [SerializeField]
    GameObject m_Root;
    ///////////////////////////////////////

    //Teleport player
    //-------------------------------------------------------------------------------------//
    public void StartTeleport(Vector3 newPos)
    {
        GetComponent<PlayerInfo>().feedbackText.Activate("got teleported!");

        Rigidbodies = GetComponentsInChildren<Rigidbody>();

        GetComponent<PlayerController>().ReleaseGrip(true, false);
        GetComponent<PlayerController>().ReleaseGrip(false, false);
        GetComponent<PlayerInfo>().DisconnectGrabbingPlayers();

        StartCoroutine(TheTeleporter());

        m_Root.transform.position = new Vector3(newPos.x, newPos.y, m_Root.transform.position.z);

       
    }

    IEnumerator TheTeleporter()
    {
        GetComponent<PlayerController>().canMove = false;

        foreach (Rigidbody rigidKinematic in Rigidbodies)
        {
            rigidKinematic.isKinematic = true;
        } 

        yield return new WaitForSeconds(0.1f);

        foreach (Rigidbody rigidKinematic in Rigidbodies)
        {
            rigidKinematic.isKinematic = false;
        }

        GetComponent<PlayerController>().canMove = true;
    }
    ///////////////////////////////////////
    //-------------------------------------------------------------------------------------//
    //Change player mass//
    //-------------------------------------------------------------------------------------//
    public void ChangePlayerMass()
	{
		Rigidbodies = GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rigidbodymass in Rigidbodies)
		{
			startMass.Add(rigidbodymass.mass);
		}

        print("i change player mass");

        StartCoroutine(ChangeMass());
        StopCoroutine(ChangeMass());
	}

    public void ResetPlayerMass()
    {
        LooseMassParticle.Stop();
        AddMassParticle.Stop();

        foreach (Rigidbody rigidbodymass in Rigidbodies)
        {
            rigidbodymass.mass = startMass[pos];
            pos++;
        }
        pos = 0;

        StopCoroutine(ChangeMass());
        //startMass.Clear();
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

            GetComponent<PlayerInfo>().feedbackText.Activate("got light weighted!");
        }

		if (m_MassPercent > 1)
		{
			AddMassParticle.Play();
			LooseMassParticle.Stop();

            GetComponent<PlayerInfo>().feedbackText.Activate("got heavy!");
        }
		pos = 0;

		yield return new WaitForSeconds(MassDuration);

        print("Reset  in changeMass");
        ResetPlayerMass();
	}
	//-----------------------------------------------------------------------//
}
