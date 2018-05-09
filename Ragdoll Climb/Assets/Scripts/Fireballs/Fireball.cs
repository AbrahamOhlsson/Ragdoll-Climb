using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	GameObject PlayerCol;
	[SerializeField]
	private float stunTime;

	private void OnParticleCollision(GameObject other)
	{ 

			PlayerCol = other.transform.root.gameObject;

			if (PlayerCol.tag == "Player")
			{
				if (PlayerCol.GetComponent<PlayerInfo>().solid)
				{
					// get script and then change value of MassPercent
					PlayerStun playerstun = PlayerCol.transform.root.gameObject.GetComponent<PlayerStun>();

					//Run function
					PlayerCol.transform.root.gameObject.GetComponent<PlayerStun>().Stun(stunTime);
				}
			}
	}
}
