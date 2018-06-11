using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballArraySequence : MonoBehaviour
{
	[SerializeField]
	ParticleSystem[] particles;
	[SerializeField]
	float startDelay = 1;
	[SerializeField]
	float interval = 1;
	[SerializeField]
	float speed = 10f;

	void Start ()
	{
		for (int i = 0; i < particles.Length; i++)
		{
			ParticleSystem.MainModule main = particles[i].main;
			ParticleSystem.EmissionModule emission = particles[i].emission;
			main.startDelay = i * startDelay;
			main.startSpeed = speed;
			emission.rateOverTime = interval;

			ParticleSystem.MainModule main2 = particles[i].transform.GetChild(0).GetComponent<ParticleSystem>().main;
			ParticleSystem.EmissionModule emission2 = particles[i].transform.GetChild(0).GetComponent<ParticleSystem>().emission;
			main2.startDelay = i * startDelay;
			main2.startSpeed = speed;
			emission2.rateOverTime = interval;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
