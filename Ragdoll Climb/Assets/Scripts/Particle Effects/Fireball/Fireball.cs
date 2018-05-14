using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	[SerializeField]
	GameObject PlayerCol;

	private void OnParticleCollision(GameObject other)
	{
		print(other.name);

			PlayerCol = other.transform.root.gameObject;

			if (PlayerCol.tag == "Player")
			{
				if (PlayerCol.GetComponent<PlayerInfo>().solid)
				{
				//Run function
				//PlayerCol.GetComponent<PlayerStun>().Stun(stunTime);
				PlayerCol.GetComponent<PlayerInfo>().leftHand.GetComponent<CheckGrip>().BurnHand("got roasted by a dragon!");
				PlayerCol.GetComponent<PlayerInfo>().rightHand.GetComponent<CheckGrip>().BurnHand("got roasted by a dragon!");
			}
			}
	}
}
