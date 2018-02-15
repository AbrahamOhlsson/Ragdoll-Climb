using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject[] players;
    public GameObject endLerp;    

    public float playerYPos;
    public float cameraPosY;
    public float cameraPosX;
    public float cameraZoom = 1f;
    public float minZPos = -20;
    public float maxZPos = -8;

    Vector3 playersDistMax;
    Vector3 playerDistMin;

    private Vector3 position;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        List<float> playerListYPos = new List<float>();
        List<float> playerListXPos = new List<float>();

        //Add players X and Y positions in two lists
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].transform.root.gameObject.activeSelf)
            {
                playerListYPos.Add(players[i].transform.position.y);
                playerListXPos.Add(players[i].transform.position.x);
            }
        }

        //Look at what players Y position is 
        if (playerListYPos.Count > 0)
        {
            playerListYPos.Sort();

            cameraPosY = (playerListYPos[0] + playerListYPos[playerListYPos.Count - 1]) / 2;
            endLerp.transform.position = new Vector3(0f, playerListYPos[playerListYPos.Count - 1], 0f);
        }

        //Look at what players X position is
        if (playerListXPos.Count > 0)
        {
            playerListXPos.Sort();

            cameraPosX = (playerListXPos[0] + playerListXPos[playerListXPos.Count - 1]) / 2;
        }
        
        //Using X and Y position to get new Z position
        if (playerListXPos.Count > 0)
        {
            playersDistMax = new Vector3(playerListXPos[playerListXPos.Count - 1], playerListYPos[playerListYPos.Count - 1]);
            playerDistMin = new Vector3(playerListXPos[0], playerListYPos[0]);

            float distance = Vector3.Distance(playerDistMin, playersDistMax);
            float zoom = distance * -cameraZoom;

            zoom = Mathf.Clamp(zoom, minZPos, maxZPos);

            transform.position = new Vector3(cameraPosX, cameraPosY, zoom);
        }
    }
}
