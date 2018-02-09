using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    //[SerializeField] GameObject playerPrefab;

    //[SerializeField] Transform[] playerInstantiatePos = new Transform[4];

    [SerializeField] Color[] playerColors;

    [SerializeField] List<GameObject> players = new List<GameObject>();

    bool[] playerSpawned = new bool[4];


    void Start ()
    {
	}


    void Update ()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetButtonDown("XB-start_p" + (i+1)) && !playerSpawned[i])
            {
                //players.Add(Instantiate(playerPrefab, playerInstantiatePos[i].position, Quaternion.identity));
                players[i].SetActive(true);

                //players[players.Count - 1].GetComponent<PlayerController>().playerNr = i+1;
                players[i].GetComponent<PlayerController>().playerNr = i + 1;

                //Renderer[] renderers = players[players.Count - 1].GetComponentsInChildren<Renderer>();
                Renderer[] renderers = players[i].GetComponentsInChildren<Renderer>();

                for (int j = 0; j < renderers.Length; j++)
                {
                    renderers[j].material.color = playerColors[i];
                }

                playerSpawned[i] = true;
            }
        }
    }
}
