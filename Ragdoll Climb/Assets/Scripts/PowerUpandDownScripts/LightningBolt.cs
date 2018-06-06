using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour {

	GameObject lightningBolt;
	GameObject blackCloud;
	GameObject root;

	private float lightningDuration;
	private float lightningWait;

	[SerializeField]private float offset;

	private bool scaleUp = false;
	private bool scaleDown = false;

	Vector3 tinyScale = new Vector3(0,0,0);
	Vector3 normalScale = new Vector3(1,1,1);

    soundManager SoundManager;
	void Start ()
	{
		SoundManager = FindObjectOfType<soundManager>();

        //lightningDuration = 0.05f;
        //lightningWait	  = 3;

        ////Get children
        //lightningBolt = gameObject.transform.GetChild(1).gameObject;
        //blackCloud = gameObject.transform.GetChild(0).gameObject;

        ////Set scale
        //gameObject.transform.localScale = tinyScale;
        //blackCloud.transform.localScale = tinyScale;

        //startLightning();

    }
	

	IEnumerator LightningStrike()
	{
		root = gameObject.transform.root.GetComponent<PlayerInfo>().rootObj;

        transform.root.GetComponent<PlayerInfo>().feedbackText.Activate("will be struck by lightning!");

		lightningDuration = 0.05f;
		lightningWait = 3;

		//Get children
		lightningBolt = gameObject.transform.GetChild(1).gameObject;
		blackCloud = gameObject.transform.GetChild(0).gameObject;

		//Set scale
		gameObject.transform.localScale = tinyScale;
		blackCloud.transform.localScale = tinyScale;


		scaleUp = true;

		yield return new WaitForSeconds(lightningWait);

		scaleUp = false;

		lightningBolt.SetActive(true);

        SoundManager.PlaySound("lightningStrike");

		yield return new WaitForSeconds(0.2f);   // w8 for the effect then stun 
		// ######   Stun  the player 
		
        if (gameObject.transform.root.GetComponent<PlayerInfo>().solid)
		    gameObject.transform.root.GetComponent<PlayerStun>().Stun(3);

		yield return new WaitForSeconds(1);
		scaleDown = true;

		yield return new WaitForSeconds(lightningDuration);

		lightningBolt.SetActive(false);

		yield return new WaitForSeconds(1);
		scaleDown = false;
		gameObject.SetActive(false);
	}

	void Update ()
	{
		transform.position = Vector3.Lerp(transform.position, new Vector3(root.transform.position.x, root.transform.position.y + offset, root.transform.position.z - 0.5f), 0.05f);
		//Scale up
		if (scaleUp)
		{
			gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, normalScale, 0.05f);
			blackCloud.transform.localScale = Vector3.Lerp(blackCloud.transform.localScale, normalScale, 0.05f);
		}
		//Scale down
		if(scaleDown)
		{
			gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, tinyScale, 0.1f);
			blackCloud.transform.localScale = Vector3.Lerp(blackCloud.transform.localScale, tinyScale, 0.1f);
		}
	}


	public void StartLightning()
	{
        StartCoroutine(LightningStrike());

        transform.position = new Vector3(root.transform.position.x, root.transform.position.y + offset, root.transform.position.z - 0.5f);
	}
}
