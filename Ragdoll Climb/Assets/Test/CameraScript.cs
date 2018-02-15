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

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].transform.root.gameObject.activeSelf)
            {
                playerListYPos.Add(players[i].transform.position.y);
                playerListXPos.Add(players[i].transform.position.x);
            }
        }

        if (playerListYPos.Count > 0)
        {
            playerListYPos.Sort();

            cameraPosY = (playerListYPos[0] + playerListYPos[playerListYPos.Count - 1]) / 2;
            endLerp.transform.position = new Vector3(0f, playerListYPos[playerListYPos.Count - 1], 0f);
        }
        if (playerListXPos.Count > 0)
        {
            playerListXPos.Sort();

            cameraPosX = (playerListXPos[0] + playerListXPos[playerListXPos.Count - 1]) / 2;
        }

        if (playerListXPos.Count > 0)
        {
            playersDistMax = new Vector3(playerListXPos[playerListXPos.Count - 1], playerListYPos[playerListYPos.Count - 1]);
            playerDistMin = new Vector3(playerListXPos[0], playerListYPos[0]);

            float distance = Vector3.Distance(playerDistMin, playersDistMax);
            float zoom = distance * -cameraZoom;

            print(distance);

            zoom = Mathf.Clamp(zoom, minZPos, maxZPos);

            transform.position = new Vector3(cameraPosX, cameraPosY, zoom);
        }
    }
}
