using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerPowerUp : MonoBehaviour
{
    public Component[] Rigidbodies;
    
    //////Getting the color for the sake of freeze
    Renderer[] renderers;


    //////FreezeTimer Depending on how close the boat is to the player
    public bool closeToBoat;
    float freezeTime;

    public bool isFrozen;
    bool doLerp;
    bool doLerpBack;

    //////Color Lerp
    [SerializeField]
    private float lerpTime;
    [SerializeField]
    private Color lerpedColor;
    private Color defColor;
    private Color currentColor;

    private void Start()
    {
        // Gets all renderers in player
        renderers = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();

        Rigidbodies = GetComponentsInChildren<Rigidbody>();

        // Get player default colour
        defColor = GetComponent<PlayerInfo>().color;
        currentColor = defColor;
    }

    public void FreezeTime()
    {
        StartCoroutine(freezeThePlayer());
    }


	public void DeathFreezeTime()
	{
		StartCoroutine(freezeDeathThePlayer());
	}


	void Update()
    {
        if (doLerp)
        {
            currentColor = Color.Lerp(currentColor, lerpedColor, lerpTime * Time.deltaTime);

            // Changes color of all renderers
            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].material.color = new Color(currentColor.r, currentColor.g, currentColor.b, renderers[j].material.color.a);
            }

        }

        if(doLerpBack)
        {
            currentColor = Color.Lerp(currentColor, defColor, lerpTime * Time.deltaTime);

            // Changes color of all renderers
            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].material.color = new Color(currentColor.r, currentColor.g, currentColor.b, renderers[j].material.color.a);
            }

        }
    }

	IEnumerator freezeThePlayer()
	{
		GetComponent<PlayerInfo>().feedbackText.Activate("got frozen!");
		if (closeToBoat)
		{
			freezeTime = 0.01f;
		}
		else if (!closeToBoat)
		{
			freezeTime = 3f;
            GetComponent<VibrationManager>().VibrateSmoothTimed(0.2f, 3f, 5f, 5f, 5);
        }
        
        currentColor = defColor;

		isFrozen = true;

		GetComponent<PlayerController>().canMove = false;
		GetComponent<PlayerController>().ReleaseGrip(true, false);
		GetComponent<PlayerController>().ReleaseGrip(false, false);
        
		foreach (Rigidbody rigidKinematic in Rigidbodies)
		{
			rigidKinematic.isKinematic = true;
		}
		if (isFrozen == true)
		{
			doLerp = true;
		}

		yield return new WaitForSeconds(freezeTime);

		doLerp = false;

		isFrozen = false;

		foreach (Rigidbody rigidKinematic in Rigidbodies)
		{
			rigidKinematic.isKinematic = false;
		}

		GetComponent<PlayerController>().canMove = true;

		if (isFrozen == false)
		{
			doLerpBack = true;
		}

		//After freeze make sure the player get default colour back
		yield return new WaitForSeconds(freezeTime);

		doLerpBack = false;
        
		// Changes color of all renderers
		for (int j = 0; j < renderers.Length; j++)
		{
			if (renderers[j].gameObject.layer != LayerMask.NameToLayer("UI"))
                renderers[j].material.color = new Color(defColor.r, defColor.g, defColor.b, renderers[j].material.color.a);
        }

	}


	//  FOR DEATH FREEZE ##############################################################
	IEnumerator freezeDeathThePlayer()
	{
		//GetComponent<PlayerInfo>().feedbackText.Activate("got frozen!");

		if (closeToBoat)
		{
			freezeTime = 0.01f;
		}
		else if (!closeToBoat)
		{
			freezeTime = 1f;
		}
        
        currentColor = defColor;

        isFrozen = true;

		GetComponent<PlayerController>().canMove = false;
		GetComponent<PlayerController>().ReleaseGrip(true, false);
		GetComponent<PlayerController>().ReleaseGrip(false, false);

		foreach (Rigidbody rigidKinematic in Rigidbodies)
		{
			rigidKinematic.isKinematic = true;
		}
		if (isFrozen == true)
		{
			doLerp = true;
		}

		yield return new WaitForSeconds(freezeTime);

		doLerp = false;

		isFrozen = false;

		foreach (Rigidbody rigidKinematic in Rigidbodies)
		{
			rigidKinematic.isKinematic = false;
		}

		GetComponent<PlayerController>().canMove = true;

		if (isFrozen == false)
		{
			doLerpBack = true;
		}

		//After freeze make sure the player get default colour back
		yield return new WaitForSeconds(freezeTime);

		doLerpBack = false;

		// Changes color of all renderers
		for (int j = 0; j < renderers.Length; j++)
		{
            if (renderers[j].gameObject.layer != LayerMask.NameToLayer("UI"))
				renderers[j].material.color = new Color(defColor.r, defColor.g, defColor.b, renderers[j].material.color.a);
		}
	}
}