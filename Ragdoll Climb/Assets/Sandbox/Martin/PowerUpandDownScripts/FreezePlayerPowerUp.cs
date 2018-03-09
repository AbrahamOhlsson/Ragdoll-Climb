using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerPowerUp : MonoBehaviour
{
    public Component[] Rigidbodies;

    [SerializeField]
    GameObject rightGrabObject;
    [SerializeField]
    GameObject leftGrabObject;

    //////Getting the color for the sake of freeze
    [SerializeField]
    Color[] FreezeColors;
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

    public void FreezeTime()
    {

        // Gets all renderers in player
        renderers = transform.GetChild(0).GetComponentsInChildren<Renderer>();

        Rigidbodies = GetComponentsInChildren<Rigidbody>();

        StartCoroutine(freezeThePlayer());

    }

    void Update()
    {
        if (doLerp)
        {
            // Changes color of all renderers
            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].material.color = Color.Lerp(renderers[j].material.color, lerpedColor, lerpTime * Time.deltaTime);
            }

        }

        if(doLerpBack)
        {
            // Changes color of all renderers
            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].material.color = Color.Lerp(renderers[j].material.color, defColor, lerpTime * Time.deltaTime);
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
        else if(!closeToBoat)
        {
            freezeTime = 3f;
        }

        // Get player default colour
        for (int j = 0; j < renderers.Length; j++)
        {
            defColor = renderers[j].material.color;
        }
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
            //rightGrabObject.GetComponent<Rigidbody>().isKinematic = true;
            //leftGrabObject.GetComponent<Rigidbody>().isKinematic = true;
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
                renderers[j].material.color = defColor;
        }

    }

}