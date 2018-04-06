using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public List<Transform> playerTrans;
    public GameObject endLerp;

    public float cameraMoveSpeed = 0.8f;
    public float playerYPos;
    public float cameraPosY;
    public float cameraPosX;
    public float cameraZoom = 1f;
    public float minZPos = -20;
    public float maxZPos = -8;

    bool doneOnce = false;

    Vector3 playersDistMax;
    Vector3 playerDistMin;

    private Vector3 position;
    

	// Update is called once per frame
	void Update ()
    {
        //if (!doneOnce)
        //{
        //    List<GameObject> players = GameObject.Find("GameManager").GetComponent<MultiplayerManager>().players;

        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        print(players[i].name);
        //        playerTrans.Add(players[i].GetComponent<PlayerInfo>().rootObj.transform);
        //    }

        //    doneOnce = true;
        //}

        if (playerTrans.Count > 0)
        {
            List<float> playerListYPos = new List<float>();
            List<float> playerListXPos = new List<float>();

            //Add players X and Y positions in two lists
            for (int i = 0; i < playerTrans.Count; i++)
            {
                if (playerTrans[i].root.gameObject.activeSelf)
                {
                    playerListYPos.Add(playerTrans[i].position.y);
                    playerListXPos.Add(playerTrans[i].position.x);
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

                transform.position = Vector3.Lerp(transform.position, new Vector3(cameraPosX, cameraPosY, zoom), cameraMoveSpeed);
            }
        }
    }
}
