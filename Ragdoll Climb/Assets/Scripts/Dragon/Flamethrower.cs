using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour {

	[SerializeField]
	private int flameInterval = 10;
	private float flameWait = 0.75f;
	private Animator dragonAnim;
	public ParticleSystem flames;

	void Start ()
	{
		dragonAnim = GetComponent<Animator>();
		StartCoroutine(ShootFlames());
	}
	
	IEnumerator ShootFlames()
	{
		dragonAnim.SetTrigger("Scream");
		yield return new WaitForSeconds(flameWait);
		flames.Play();
		yield return new WaitForSeconds(flameInterval);
		StartCoroutine(ShootFlames());
	}

	public void StopFlames()
	{
		flames.Stop();
	}
}
