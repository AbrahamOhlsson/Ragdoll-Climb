using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using XInputDotNetPure;


public class TutorialController_new : MonoBehaviour
{
    [SerializeField] Text indexText;

	//PlayerIndex[] playerIndex;
	GamePadState[] state = new GamePadState[4];
	GamePadState[] prevState = new GamePadState[4];

	private int spriteNumb;

    GameObject playerTutorial;
    [SerializeField]GameObject tutFilmObj;


    void Awake ()
	{
		playerTutorial = GameObject.Find("PlayerTutorial");
	}

    private void Start()
    {
        tutFilmObj.SetActive(false);

        if (transform.root.name.Contains("Pause"))
            tutFilmObj.GetComponent<RawImage>().texture = tutFilmObj.GetComponent<VideoPlayer>().texture;
    }

    private void OnEnable()
    {
        spriteNumb = 1;

        ChangeImage();
    }


    void Update ()
	{
        for (int i = 0; i < state.Length; i++)
        {
            prevState[i] = state[i];
            state[i] = GamePad.GetState((PlayerIndex)i);

            if ((state[i].DPad.Right == ButtonState.Pressed && prevState[i].DPad.Right == ButtonState.Released)
                || (state[i].ThumbSticks.Left.X >= 0.5f && prevState[i].ThumbSticks.Left.X < 0.5f))
            {
                spriteNumb++;

                if (spriteNumb > 4)
                {
                    spriteNumb = 1;
                }

                ChangeImage();
            }
            else if ((state[i].DPad.Left == ButtonState.Pressed && prevState[i].DPad.Left == ButtonState.Released)
                || (state[i].ThumbSticks.Left.X <= -0.5f && prevState[i].ThumbSticks.Left.X > -0.5f))
            {
                spriteNumb--;

                if (spriteNumb <= 0)
                {
                    spriteNumb = 4;
                }

                ChangeImage();
            }
        }

	}

    

    private void LateUpdate()
    {
        if (spriteNumb != 2)
        {
            tutFilmObj.SetActive(false);

        }

        else if (spriteNumb == 2)
        {
            tutFilmObj.SetActive(true);
            tutFilmObj.GetComponent<RawImage>().texture = tutFilmObj.GetComponent<VideoPlayer>().texture;
        }
    }

    void ChangeImage()
	{
		playerTutorial.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ragdollTutorial" + spriteNumb);
        indexText.text = spriteNumb + " / 4";
    }
}
