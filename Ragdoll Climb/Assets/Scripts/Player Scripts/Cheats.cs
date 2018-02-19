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

    // Controller variables
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    PlayerController controller;

    Rigidbody[] bodies;
    float bodyMasses;


	void Start ()
    {
        controller = GetComponent<PlayerController>();

        bodies = GetComponentsInChildren<Rigidbody>();
	}
	

	void Update ()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);

        if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
        {
            if (controller.ActivateBoost())
                debugText.AddText("Player " + playerIndex + " activated Boost Cheat");
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
                debugText.AddText("Player " + playerIndex + " deactivated Light Weight Cheat");
            }
            else
            {
                for (int i = 0; i < bodies.Length; i++)
                {
                    bodies[i].mass *= 0.5f;
                }

                lightWeightActive = true;
                debugText.AddText("Player " + playerIndex + " activated Light Weight Cheat");
            }
        }

        if (state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released)
        {
            if (invertedPullControls)
            {
                controller.ToggleInvertPull();
                debugText.AddText("Player " + playerIndex + " reverted pull controls");
                invertedPullControls = false;
            }
            else
            {
                controller.ToggleInvertPull();
                debugText.AddText("Player " + playerIndex + " inverted pull controls");
                invertedPullControls = true;
            }
        }

        if (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released)
        {
            if (stamina)
            {
                controller.ToggleUnlimitedStamina();
                debugText.AddText("Player " + playerIndex + " deactivated Unlimited Stamina Cheat");
                stamina = false;
            }

            controller.ToggleUnlimitedStamina();
            debugText.AddText("Player " + playerIndex + " activated Unlimited Stamina Cheat");
            stamina = true;
        }
    }


    public void SetGamePad(int index)
    {
        playerIndex = (PlayerIndex)index;
    }
}
