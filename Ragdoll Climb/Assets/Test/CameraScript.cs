using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject[] players;

    public float test;

    public float cameraPos;


    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (players[0].transform.position.y < players[1].transform.position.y) {
            test =  players[0].transform.position.y;

            cameraPos = (((players[1].transform.position.y - players[0].transform.position.y)/2) + test);

            transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);

            Debug.Log("i första ");
        }

        else if (players[1].transform.position.y < players[0].transform.position.y) {
            test = players[1].transform.position.y;

            cameraPos = (((players[0].transform.position.y - players[1].transform.position.y) / 2) + test);

            transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);
            Debug.Log("i andra ");
        }


        else if(players[1].transform.position.y == players[0].transform.position.y)
        {
            test = players[1].transform.position.y;

            cameraPos =   test;

            transform.position = new Vector3(transform.position.x, cameraPos, transform.position.z);
        }









    }

}
