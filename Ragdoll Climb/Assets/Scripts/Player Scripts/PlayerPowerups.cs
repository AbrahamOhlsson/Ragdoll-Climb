using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerups : MonoBehaviour
{
	////// Add/loose mass
	public Rigidbody[] rigidbodies;
	public List<float> startMass;
	public int pos = 0;
	[SerializeField]
	public float m_MassPercent;
	[SerializeField]
	private int MassDuration;
	///////////////////////////////////////
	public ParticleSystem AddMassParticle;
	public ParticleSystem LooseMassParticle;
    ParticleSystem[] particleSystems;
    ///////////////////////////////////////
    ////// Teleport player
    [SerializeField] Transform rightWrist;
    [SerializeField] Transform leftWrist;
    [SerializeField] GameObject m_Root;

    Quaternion leftWristStartRot;
    Quaternion rightWristStartRot;
    ///////////////////////////////////////
    PlayerInfo playerInfo;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();

        rigidbodies = GetComponentsInChildren<Rigidbody>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            string name = rigidbodies[i].name;

            if (name.Contains("wrist") || name.Contains("Wrist"))
            {
                if (name.Contains("L"))
                    leftWrist = rigidbodies[i].transform;
                else if (name.Contains("R"))
                    rightWrist = rigidbodies[i].transform;
            }
            else if (name.Contains("Root") || name.Contains("root"))
            {
                m_Root = rigidbodies[i].gameObject;
            }
        }

        for (int i = 0; i < particleSystems.Length; i++)
        {
            string name = particleSystems[i].name;

            if (name.Contains("AddMass"))
                AddMassParticle = particleSystems[i];
            else if (name.Contains("LooseMass"))
                LooseMassParticle = particleSystems[i];
        }

        leftWristStartRot = leftWrist.localRotation;
        rightWristStartRot = rightWrist.localRotation;
    }

    //Teleport player
    //-------------------------------------------------------------------------------------//
    public void StartTeleport(Vector3 newPos)
    {
        GetComponent<PlayerInfo>().feedbackText.Activate("got teleported!");

        //Rigidbodies = GetComponentsInChildren<Rigidbody>();

        GetComponent<PlayerController>().ReleaseGrip(true, false);
        GetComponent<PlayerController>().ReleaseGrip(false, false);
        GetComponent<PlayerInfo>().DisconnectGrabbingPlayers();
        GetComponent<VibrationManager>().VibrateTimed(0.5f, 0.1f, 5);

        StartCoroutine(TheTeleporter());

        m_Root.transform.position = new Vector3(newPos.x, newPos.y, m_Root.transform.position.z);
    }

    IEnumerator TheTeleporter()
    {
        GetComponent<PlayerController>().canMove = false;

        foreach (Rigidbody rigidKinematic in rigidbodies)
        {
            rigidKinematic.isKinematic = true;
        }

        rightWrist.localRotation = rightWristStartRot;
        leftWrist.localRotation = leftWristStartRot;

        yield return new WaitForSeconds(0.1f);

        foreach (Rigidbody rigidKinematic in rigidbodies)
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
		/*Rigidbodies = GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rigidbodymass in Rigidbodies)
		{
			startMass.Add(rigidbodymass.mass);
		}

        print("i change player mass");*/

        StartCoroutine(ChangeMass());
        StopCoroutine(ChangeMass());
	}

    public void ResetPlayerMass()
    {
        LooseMassParticle.Stop();
        AddMassParticle.Stop();

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            //rigidbodymass.mass = startMass[pos];
            playerInfo.targetMasses[i] = playerInfo.standardMasses[i];
            rigidbodies[i].mass = playerInfo.targetMasses[i];
        }

        StopCoroutine(ChangeMass());
        //startMass.Clear();
    }

	IEnumerator ChangeMass()
	{
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            //rigidbody.mass = startMass[pos] * m_MassPercent;
            playerInfo.targetMasses[i] = playerInfo.standardMasses[i] * m_MassPercent;
            rigidbodies[i].mass = playerInfo.targetMasses[i];
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
        
        ResetPlayerMass();
	}
	//-----------------------------------------------------------------------//
}
