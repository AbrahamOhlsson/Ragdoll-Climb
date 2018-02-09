using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject[] players;
    public GameObject endLerp;    

    public float playerYPos;
    public float cameraPos;

    int topPlayer;
    int botPlayer;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        List<float> playerListYPos = new List<float>();
        
        for (int i = 0; i < players.Length; i++)
        {
            print("Players Length = " + players.Length);

            if (players[i].transform.root.gameObject.activeSelf)
                playerListYPos.Add(players[i].transform.position.y);
        }

        if (playerListYPos.Count > 0)
        {
            playerListYPos.Sort();

            cameraPos = (playerListYPos[0] + playerListYPos[playerListYPos.Count - 1]) / 2;
            endLerp.transform.position = new Vector3(0f, playerListYPos[playerListYPos.Count - 1], 0f);

            transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);
        }
    }
}
