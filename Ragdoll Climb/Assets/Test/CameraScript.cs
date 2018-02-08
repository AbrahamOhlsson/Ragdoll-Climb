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
        for(int i = 0; i <= 3; i++)
        {
            //Player one check
            if (players[i].transform.position.y > players[1].transform.position.y && players[i].transform.position.y > players[2].transform.position.y && players[i].transform.position.y > players[3].transform.position.y)
            {
                topPlayer = 0;
                endLerp.transform.position = players[0].transform.position;
                Debug.Log("Player1 top");
            }
            else if (players[i].transform.position.y < players[1].transform.position.y && players[i].transform.position.y < players[2].transform.position.y && players[i].transform.position.y < players[3].transform.position.y)
            {
                botPlayer = 0;
                
                Debug.Log("Player1 bot");
            }
            //Player two check
            if (players[i].transform.position.y > players[0].transform.position.y && players[i].transform.position.y > players[2].transform.position.y && players[i].transform.position.y > players[3].transform.position.y)
            {
                topPlayer = 1;
                endLerp.transform.position = players[1].transform.position;
                Debug.Log("Player2 top");
            }
            else if (players[i].transform.position.y < players[0].transform.position.y && players[i].transform.position.y < players[2].transform.position.y && players[i].transform.position.y < players[3].transform.position.y)
            {
                botPlayer = 1;
                Debug.Log("Player2 bot");
            }
            //Player three check
            if (players[i].transform.position.y > players[0].transform.position.y && players[i].transform.position.y > players[1].transform.position.y && players[i].transform.position.y > players[3].transform.position.y)
            {
                topPlayer = 2;
                endLerp.transform.position = players[2].transform.position;
                Debug.Log("Player3 top");
            }
            else if (players[i].transform.position.y < players[0].transform.position.y && players[i].transform.position.y < players[1].transform.position.y && players[i].transform.position.y < players[3].transform.position.y)
            {
                botPlayer = 2;
                Debug.Log("Player3 bot");
            }
            //Player four check
            if (players[i].transform.position.y > players[0].transform.position.y && players[i].transform.position.y > players[1].transform.position.y && players[i].transform.position.y > players[2].transform.position.y)
            {
                topPlayer = 3;
                endLerp.transform.position = players[3].transform.position;
                Debug.Log("Player4 top");
            }
            else if (players[i].transform.position.y < players[0].transform.position.y && players[i].transform.position.y < players[1].transform.position.y && players[i].transform.position.y < players[2].transform.position.y)
            {
                botPlayer = 3;
                Debug.Log("Player4 bot");
            }
        }

        //Top player position
        if (players[topPlayer].transform.position.y < players[botPlayer].transform.position.y)
        {
            playerYPos = players[topPlayer].transform.position.y;

            cameraPos = (((players[botPlayer].transform.position.y - players[topPlayer].transform.position.y) / 2) + playerYPos);

            transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);

            Debug.Log("i top ");
        }
        //Bot player position
        else if (players[botPlayer].transform.position.y < players[topPlayer].transform.position.y)
        {
            playerYPos = players[botPlayer].transform.position.y;

            cameraPos = (((players[topPlayer].transform.position.y - players[botPlayer].transform.position.y) / 2) + playerYPos);

            transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);
            Debug.Log("i bot ");
        }
        else if (players[botPlayer].transform.position.y == players[topPlayer].transform.position.y)
        {
            playerYPos = players[botPlayer].transform.position.y;

            cameraPos = playerYPos;

            transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);
        }



        //if (players[0].transform.position.y < players[1].transform.position.y)
        //{
        //    test = players[0].transform.position.y;

        //    cameraPos = (((players[1].transform.position.y - players[0].transform.position.y) / 2) + test);

        //    transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);

        //    Debug.Log("i första ");
        //}

        //else if (players[1].transform.position.y < players[0].transform.position.y)
        //{
        //    test = players[1].transform.position.y;

        //    cameraPos = (((players[0].transform.position.y - players[1].transform.position.y) / 2) + test);

        //    transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);
        //    Debug.Log("i andra ");
        //}


        //else if (players[1].transform.position.y == players[0].transform.position.y)
        //{
        //    test = players[1].transform.position.y;

        //    cameraPos = test;

        //    transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);
        //    Debug.Log("jämte");
        //}

    }

}
