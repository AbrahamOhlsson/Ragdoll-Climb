using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackText : MonoBehaviour
{
    [HideInInspector] public int playerNr;
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public Vector2 goalPos;

    [SerializeField] enum Alignment { AbovePlayer, TopOfScreen }
    [SerializeField] Alignment alignment = Alignment.AbovePlayer;
    
    [Range(0f, 1f)] [SerializeField] float moveSpeed = 0.1f;
    [Range(0f, 1f)] [SerializeField] float scaleSpeed = 0.1f;
    [Range(0f, 1f)] [SerializeField] float fadeSpeed = 0.1f;

    [SerializeField] float fadeDelay = 1f;
    [SerializeField] float reqPosOffset = 1f;
    [SerializeField] float reqScaleOffset = 0.99f;
    [SerializeField] float minScale = 0.1f;

    [SerializeField] Vector3 playerOffset = new Vector2(0f, 150f);
    
    bool active = false;
    
    float fadeDelayTimer = 0f;
    
    enum States { None, Move, Fade }
    States state;

    Vector3 canvasRes;

    Text feedbackText;

    Camera cam;

    RectTransform rectTrans;


    void Start ()
    {
        feedbackText = GetComponent<Text>();

        cam = Camera.main;

        rectTrans = GetComponent<RectTransform>();

        goalPos = rectTrans.localPosition;

        canvasRes = transform.root.GetComponent<CanvasScaler>().referenceResolution;
    }


    void Update ()
    {
        switch (alignment)
        {
            case Alignment.TopOfScreen:
                // Makes the text move to the top of the screen
                if (active)
                {
                    // Lerps scale and position
                    rectTrans.localPosition = Vector3.Lerp(rectTrans.localPosition, goalPos, moveSpeed);
                    rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, new Vector3(1f, 1f, 1f), scaleSpeed);

                    // If text is close enough and big enough
                    if (Vector3.Distance(rectTrans.localPosition, goalPos) <= reqPosOffset && rectTrans.localScale.x >= reqScaleOffset)
                    {
                        // If it's time to fade away
                        if (fadeDelayTimer >= fadeDelay)
                        {
                            // Reduces alpha with lerping
                            feedbackText.color = new Color(feedbackText.color.r, feedbackText.color.g, feedbackText.color.b, Mathf.Lerp(feedbackText.color.a, 0f, fadeSpeed));

                            // If alpha is small enough
                            if (feedbackText.color.a <= 0.05f)
                            {
                                // Disables text
                                feedbackText.enabled = false;
                                active = false;
                            }
                        }

                        fadeDelayTimer += Time.deltaTime;
                    }
                }
                break;

            case Alignment.AbovePlayer:
                // Makes the text move to a point above the player
                switch (state)
                {
                    case States.Move:
                        // Lerps scale and position
                        rectTrans.localPosition = PlayerPosToCanvasPos_Offset();
                        rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, new Vector3(1f, 1f, 1f), scaleSpeed);

                        // If text is close enough and big enough
                        if (Vector3.Distance(rectTrans.localPosition, PlayerPosToCanvasPos_Offset()) <= reqPosOffset && rectTrans.localScale.x >= reqScaleOffset)
                        {
                            rectTrans.localScale = new Vector3(1f, 1f, 1f);

                            state = States.Fade;
                        }

                        break;

                    case States.Fade:
                        // Lerps position
                        rectTrans.localPosition = PlayerPosToCanvasPos_Offset();

                        // If it's time to fade
                        if (fadeDelayTimer >= fadeDelay)
                        {
                            // Reduces alpha with lerping
                            feedbackText.color = new Color(feedbackText.color.r, feedbackText.color.g, feedbackText.color.b, Mathf.Lerp(feedbackText.color.a, 0f, fadeSpeed));

                            // If alpha is small enough
                            if (feedbackText.color.a <= 0.05f)
                            {
                                // Disables text
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
                break;
        }
    }


    private Vector3 PlayerPosToCanvasPos_Offset()
    {
        //return cam.WorldToScreenPoint(playerTrans.position) - new Vector3(Screen.currentResolution.width / 2 - playerOffset.x, Screen.currentResolution.height / 2 - playerOffset.y);
        return Vector3.Scale(cam.WorldToViewportPoint(playerTrans.position), canvasRes) - canvasRes / 2 + playerOffset;
    }
    private Vector3 PlayerPosToCanvasPos()
    {
        //return cam.WorldToScreenPoint(playerTrans.position) - new Vector3(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);
        return Vector3.Scale(cam.WorldToViewportPoint(playerTrans.position), canvasRes) - canvasRes / 2;
    }


    public void Activate(string text)
    {
        feedbackText.text = "Player " + playerNr + " " + text;

        // Resets values
        transform.localScale = new Vector3(minScale, minScale, minScale);
        feedbackText.color = new Color(feedbackText.color.r, feedbackText.color.g, feedbackText.color.b, 1);
        fadeDelayTimer = 0f;
        state = States.Move;

        // Moves it to be in front of the player in canvas space
        rectTrans.localPosition = PlayerPosToCanvasPos();
        
        // Updates other texts goal positions
        transform.root.GetComponent<FeedbackTextManager>().PushList(this);
        
        feedbackText.enabled = true;

        active = true;
    }
}
