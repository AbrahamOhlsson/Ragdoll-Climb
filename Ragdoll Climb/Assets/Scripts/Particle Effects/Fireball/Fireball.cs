using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	[SerializeField]
	GameObject PlayerCol;

    string feedbackMessage = "";

    private void Awake()
    {
        if (gameObject.name.Contains("ball"))
            feedbackMessage = "got hit by a fireball!";
        else
            feedbackMessage = "got roasted by a dragon!";
    }

    private void OnParticleCollision(GameObject other)
	{
		PlayerCol = other.transform.root.gameObject;

		if (PlayerCol.tag == "Player")
		{
			if (PlayerCol.GetComponent<PlayerInfo>().solid)
			{
		        //Run function
		        //PlayerCol.GetComponent<PlayerStun>().Stun(stunTime);
		        PlayerCol.GetComponent<PlayerInfo>().leftHand.GetComponent<CheckGrip>().BurnHand(feedbackMessage);
		        PlayerCol.GetComponent<PlayerInfo>().rightHand.GetComponent<CheckGrip>().BurnHand(feedbackMessage);
            }
		}
	}
}
