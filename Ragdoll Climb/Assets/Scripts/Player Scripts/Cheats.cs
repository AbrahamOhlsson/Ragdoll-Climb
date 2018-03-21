using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Cheats : MonoBehaviour
{
    [SerializeField] DebugText debugText;
    [SerializeField] bool lightWeightAvailable = true;
    [SerializeField] bool boostAvailable = true;
    [SerializeField] bool invertPullAvailable = true;
    [SerializeField] bool unlimitedStaminaAvailable = true;

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

        if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released && boostAvailable)
        {
            if (controller.ActivateBoost())
                debugText.AddText("Player " + playerNr + " activated Boost Cheat");
        }

        if (state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released && lightWeightAvailable)
        {
            if (lightWeightActive)
            {
                for (int i = 0; i < bodies.Length; i++)
                {
                    playerInfo.standardMasses[i] *= 10f;
                    bodies[i].mass = playerInfo.standardMasses[i];
                }

                lightWeightActive = false;
                debugText.AddText("Player " + playerNr + " deactivated Light Weight Cheat");
            }
            else
            {
                for (int i = 0; i < bodies.Length; i++)
                {
                    playerInfo.standardMasses[i] *= 0.1f;
                    bodies[i].mass = playerInfo.standardMasses[i];
                }

                lightWeightActive = true;
                debugText.AddText("Player " + playerNr + " activated Light Weight Cheat");
            }
        }

        if (state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released && invertPullAvailable)
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

        if (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released && unlimitedStaminaAvailable)
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
