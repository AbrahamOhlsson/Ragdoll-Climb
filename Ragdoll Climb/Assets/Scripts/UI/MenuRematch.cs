using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;
using UnityEngine.EventSystems;


public class MenuRematch : MonoBehaviour
{
	PlayerIndex[] playerIndexes = new PlayerIndex[4];
	GamePadState[] states = new GamePadState[4];
	GamePadState[] prevStates = new GamePadState[4];

	[SerializeField] EventSystem eventSystem;

	public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

	void Start()
	{
		playerIndexes[0] = PlayerIndex.One;
		playerIndexes[1] = PlayerIndex.Two;
		playerIndexes[2] = PlayerIndex.Three;
		playerIndexes[3] = PlayerIndex.Four;
	}

	void Update()
	{
		// Checks input from all four controllers
		for (int i = 0; i < playerIndexes.Length; i++)
		{
			prevStates[i] = states[i];
			states[i] = GamePad.GetState(playerIndexes[i]);

			if (eventSystem.currentSelectedGameObject == null)
			{
				// Highlights first found button if input is received fron a controller
				if (((states[i].DPad.Down == ButtonState.Pressed && prevStates[i].DPad.Down == ButtonState.Released) || (states[i].DPad.Up == ButtonState.Pressed && prevStates[i].DPad.Up == ButtonState.Released) || (states[i].DPad.Left == ButtonState.Pressed && prevStates[i].DPad.Left == ButtonState.Released) || (states[i].DPad.Right == ButtonState.Pressed && prevStates[i].DPad.Right == ButtonState.Released) || (states[i].Buttons.A == ButtonState.Pressed && prevStates[i].Buttons.A == ButtonState.Released) || (states[i].ThumbSticks.Left.Y > 0f || states[i].ThumbSticks.Left.Y < 0f && prevStates[i].ThumbSticks.Left.Y == 0f)))
				{
					eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
				}
			}
		}
	}

    public void QuitGame()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("Ice Menu");
    }
}
