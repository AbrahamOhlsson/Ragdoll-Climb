using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePD : MonoBehaviour
{
    internal bool isFrozen;

    [SerializeField] float defaultFreezeTime = 3;
    [SerializeField] float lerpTime = 3;
    [SerializeField] Color freezeColor;

    [SerializeField] float minBottomObjDist = 4f;
    Transform bottomObj;
    
    float freezeTime;

    enum States { None, Freeze, Unfreeze }
    States state = States.None;

    //////The renderers for the sake of freeze
    Renderer[] renderers;

    Rigidbody[] rbs;

    Transform rootTrans;

    Color defColor;
    Color currentColor;

    IEnumerator coroutine;


    private void Start()
    {
        // Gets all renderers in player
        renderers = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();

        rbs = GetComponentsInChildren<Rigidbody>();

        rootTrans = GetComponent<PlayerInfo>().rootObj.transform;

        bottomObj = GameObject.FindGameObjectWithTag("BottomObj").transform;
    }
    

	void Update()
    {
        switch (state)
        {
            case States.Freeze:
                currentColor = Color.Lerp(currentColor, freezeColor, lerpTime * Time.deltaTime);

                // Changes color of all renderers
                for (int j = 0; j < renderers.Length; j++)
                {
                    renderers[j].material.color = new Color(currentColor.r, currentColor.g, currentColor.b, renderers[j].material.color.a);
                }
            break;

            case States.Unfreeze:
                currentColor = Color.Lerp(currentColor, defColor, lerpTime * Time.deltaTime);

                // Changes color of all renderers
                for (int j = 0; j < renderers.Length; j++)
                {
                    renderers[j].material.color = new Color(currentColor.r, currentColor.g, currentColor.b, renderers[j].material.color.a);
                }

                if (currentColor.r >= defColor.r - 0.01f && currentColor.g >= defColor.g - 0.01f && currentColor.b >= defColor.b - 0.01f)
                {
                    for (int j = 0; j < renderers.Length; j++)
                        renderers[j].material.color = new Color(defColor.r, defColor.g, defColor.b, renderers[j].material.color.a);

                    state = States.None;
                }
            break;
        }

        if (isFrozen && rootTrans.position.y <= bottomObj.position.y + minBottomObjDist)
            ResetFreeze();
    }


    public void StartFreeze(float time, bool respawn)
    {
        if (!isFrozen)
        {
            freezeTime = time;

            isFrozen = true;

            // Get player default colour
            defColor = GetComponent<PlayerInfo>().color;
            currentColor = defColor;

            GetComponent<PlayerController>().canMove = false;
            GetComponent<PlayerController>().ReleaseGrip(true, false);
            GetComponent<PlayerController>().ReleaseGrip(false, false);

            if (!respawn)
            {
                GetComponent<PlayerInfo>().feedbackText.Activate("got frozen!");
                GetComponent<VibrationManager>().VibrateSmoothTimed(0.2f, 3f, 5f, 5f, 5);
            }

            foreach (Rigidbody rb in rbs)
            {
                rb.isKinematic = true;
            }

            state = States.Freeze;

            coroutine = FreezeTimer();
            StartCoroutine(coroutine);
        }
    }
    

    IEnumerator FreezeTimer()
	{
		yield return new WaitForSeconds(freezeTime);

        ResetFreeze();
	}


    void ResetFreeze()
    {
        state = States.Unfreeze;

        isFrozen = false;

        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }

        GetComponent<PlayerController>().canMove = true;

        StopCoroutine(coroutine);
    }
}