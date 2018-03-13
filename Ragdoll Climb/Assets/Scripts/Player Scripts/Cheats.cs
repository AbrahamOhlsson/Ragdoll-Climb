using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Cheats : MonoBehaviour
{
    [SerializeField] DebugText debugText;

    bool lightWeightActive = false;
    bool invertedPullControls = false;
    bool stamina = false;

    int playerNr;

    // Controller variables
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    PlayerController controller;
    PlayerInfo playerInfo;

    Rigidbody[] bodies;
    float bodyMasses;


	void Start ()
    {
        controller = GetComponent<PlayerController>();
        playerInfo = GetComponent<PlayerInfo>();

        playerIndex = playerInfo.playerIndex;
        playerNr = playerInfo.playerNr;

        bodies = GetComponentsInChildren<Rigidbody>();
	}
	

	void Update ()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);

		if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
		{
			if (controller.ActivateBoost())
				debugText.AddText("Player " + playerNr + " activated Boost Cheat");
		}

		if (state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released)
		{
			if (lightWeightActive)
			{
				for (int i = 0; i < bodies.Length; i++)
				{
					bodies[i].mass *= 2f;
				}

				lightWeightActive = false;
				debugText.AddText("Player " + playerNr + " deactivated Light Weight Cheat");
			}
			else
			{
				for (int i = 0; i < bodies.Length; i++)
				{
					bodies[i].mass *= 0.5f;
				}

				lightWeightActive = true;
				debugText.AddText("Player " + playerNr + " activated Light Weight Cheat");
			}
		}

		if (state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released)
        {
            if (invertedPullControls)
            {
                controller.ToggleInvertPull();
                debugText.AddText("Player " + playerNr + " reverted pull controls");
                invertedPullControls = false;
            }
            else
            {
                controller.ToggleInvertPull();
                debugText.AddText("Player " + playerNr + " inverted pull controls");
                invertedPullControls = true;
            }
        }

		if (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released)
		{
			controller.ToggleUnlimitedStamina();

			if (stamina)
			{
				debugText.AddText("Player " + playerNr + " deactivated Unlimited Stamina Cheat");
				stamina = false;
			}
			else
			{
				debugText.AddText("Player " + playerNr + " activated Unlimited Stamina Cheat");
				stamina = true;
			}
		}
	}


    public void SetGamePad(int index)
    {
        playerIndex = (PlayerIndex)index;
    }
}
