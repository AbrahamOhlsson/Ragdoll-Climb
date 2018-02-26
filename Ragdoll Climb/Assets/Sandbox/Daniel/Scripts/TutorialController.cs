using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;


public class TutorialController : MonoBehaviour
{

	// Controller variables
	PlayerIndex playerIndex;
	GamePadState controller;
	GamePadState prevState;

	private int spriteNumb;

	GameObject playerTutorial;

	// Use this for initialization
	void Start ()
	{
		spriteNumb = 1;

		playerTutorial = GameObject.Find("PlayerTutorial");

		playerTutorial.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ragdollTutorial" + spriteNumb);
	}
	
	// Update is called once per frame
	void Update ()
	{
		prevState = controller;
		controller = GamePad.GetState(playerIndex);

		if(controller.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
		{
			spriteNumb++;

			if(spriteNumb > 3)
			{
				spriteNumb = 1;
			}

			ChangeImage();
		}

		if(controller.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released)
		{
			SceneManager.LoadScene("Level_ConstructionSite");
		}
	}

	void ChangeImage()
	{
		playerTutorial.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ragdollTutorial" + spriteNumb);
    }
}
