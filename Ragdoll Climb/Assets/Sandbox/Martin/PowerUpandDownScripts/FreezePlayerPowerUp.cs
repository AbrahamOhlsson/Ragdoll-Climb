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

    bool isFrozen;
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
        renderers = gameObject.GetComponentsInChildren<Renderer>();

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
                if (renderers[j].gameObject.layer != LayerMask.NameToLayer("UI"))
                    renderers[j].material.color = Color.Lerp(renderers[j].material.color, lerpedColor, lerpTime * Time.deltaTime);
            }

        }

        if(doLerpBack)
        {
            // Changes color of all renderers
            for (int j = 0; j < renderers.Length; j++)
            {
                if (renderers[j].gameObject.layer != LayerMask.NameToLayer("UI"))
                    renderers[j].material.color = Color.Lerp(renderers[j].material.color, defColor, lerpTime * Time.deltaTime);
            }

        }
    }

    IEnumerator freezeThePlayer()
    {
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

        yield return new WaitForSeconds(3);

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
        yield return new WaitForSeconds(3);

        doLerpBack = false;
       
        // Changes color of all renderers
        for (int j = 0; j < renderers.Length; j++)
        {
            if (renderers[j].gameObject.layer != LayerMask.NameToLayer("UI"))
                renderers[j].material.color = defColor;
        }

    }

}