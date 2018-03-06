using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackText : MonoBehaviour
{
    [HideInInspector] public int playerNr;
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public Vector2 goalPos;

    bool active = false;

    float minScale = 0.1f;

    float fadeDelay = 1f;
    float fadeDelayTimer = 0f;

    //float activeTime = 3f;
    //float activeTimer = 0f;

    enum States { None, Move, Fade }
    States state;


    Text feedbackText;

    Camera cam;

    RectTransform rectTrans;


    void Start ()
    {
        feedbackText = GetComponent<Text>();

        cam = Camera.main;

        rectTrans = GetComponent<RectTransform>();

        goalPos = rectTrans.localPosition;
    }


    void Update ()
    {
        //if (active)
        //{
        //    rectTrans.localPosition = Vector3.Lerp(rectTrans.localPosition, goalPos, 0.1f);
        //    rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, new Vector3(1f, 1f, 1f), 0.1f);

        //    if (Vector3.Distance(rectTrans.localPosition, goalPos) <= 3 && rectTrans.localScale.x >= 0.95f)
        //    {
        //        rectTrans.localPosition = goalPos;
        //        rectTrans.localScale = new Vector3(1f, 1f, 1f);

        //        if (fadeDelayTimer >= fadeDelay)
        //        {
        //            feedbackText.color = new Color(feedbackText.color.r, feedbackText.color.g, feedbackText.color.b, Mathf.Lerp(feedbackText.color.a, 0f, 0.1f));

        //            if (feedbackText.color.a <= 0.05f)
        //            {
        //                feedbackText.enabled = false;
        //                active = false;
        //            }
        //        }

        //        fadeDelayTimer += Time.deltaTime;
        //    }
        //}

        /*switch (state)
        {
            case States.Move:
                rectTrans.localPosition = Vector3.Lerp(rectTrans.localPosition, goalPos, 0.1f);
                rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, new Vector3(1f, 1f, 1f), 0.1f);

                if (Vector3.Distance(rectTrans.localPosition, goalPos) <= 3 && rectTrans.localScale.x >= 0.95f)
                {
                    rectTrans.localPosition = goalPos;
                    rectTrans.localScale = new Vector3(1f, 1f, 1f);

                    state = States.Fade;
                }

                break;

            case States.Fade:
                if (fadeDelayTimer >= fadeDelay)
                {
                    feedbackText.color = new Color(feedbackText.color.r, feedbackText.color.g, feedbackText.color.b, Mathf.Lerp(feedbackText.color.a, 0f, 0.1f));

                    if (feedbackText.color.a <= 0.05f)
                    {
                        feedbackText.enabled = false;
                        state = States.None;
                    }
                }
                else
                    fadeDelayTimer += Time.deltaTime;

                break;

            case States.None:
                break;
        }*/

        switch (state)
        {
            case States.Move:
                rectTrans.localPosition = Vector3.Lerp(rectTrans.localPosition, cam.WorldToScreenPoint(playerTrans.position) - new Vector3(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2 - 150f), 0.1f);
                rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, new Vector3(1f, 1f, 1f), 0.1f);

                if (Vector3.Distance(rectTrans.localPosition, cam.WorldToScreenPoint(playerTrans.position) - new Vector3(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2 - 150f)) <= 3 && rectTrans.localScale.x >= 0.95f)
                {
                    rectTrans.localScale = new Vector3(1f, 1f, 1f);

                    state = States.Fade;
                }

                break;

            case States.Fade:
                rectTrans.localPosition = Vector3.Lerp(rectTrans.localPosition, cam.WorldToScreenPoint(playerTrans.position) - new Vector3(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2 - 150f), 0.1f);

                if (fadeDelayTimer >= fadeDelay)
                {
                    feedbackText.color = new Color(feedbackText.color.r, feedbackText.color.g, feedbackText.color.b, Mathf.Lerp(feedbackText.color.a, 0f, 0.1f));

                    if (feedbackText.color.a <= 0.05f)
                    {
                        feedbackText.enabled = false;
                        state = States.None;
                    }
                }
                else
                    fadeDelayTimer += Time.deltaTime;

                break;

            case States.None:
                break;
        }
    }


    public void Activate(string text)
    {
        print("HELLO");

        feedbackText.text = "Player " + playerNr + " " + text;

        transform.localScale = new Vector3(minScale, minScale, minScale);

        rectTrans.localPosition = cam.WorldToScreenPoint(playerTrans.position) - new Vector3(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);

        feedbackText.color = new Color(feedbackText.color.r, feedbackText.color.g, feedbackText.color.b, 1);

        feedbackText.enabled = true;

        fadeDelayTimer = 0f;

        transform.root.GetComponent<FeedbackTextManager>().PushList(this);

        state = States.Move;

        active = true;
    }
}
