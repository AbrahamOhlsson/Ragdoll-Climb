using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBlock : MonoBehaviour
{
    [SerializeField] float distance;

    MultiplayerManager gameManager;
    GameObject bottomObj;

    Rigidbody[] rbs;


    void Start ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<MultiplayerManager>();
        bottomObj = GameObject.FindGameObjectWithTag("BottomObj");

        rbs = GetComponentsInChildren<Rigidbody>();
    }


    void Update ()
    {
        if (transform.position.y < (bottomObj.transform.position.y + distance))
        {
            // Checks if any of the players hands are gripping any of this objects and its childs rigidbodies
            for (int i = 0; i < gameManager.players.Count; i++)
            {
                CheckGrip[] hands = gameManager.players[i].GetComponentsInChildren<CheckGrip>();

                for (int j = 0; j < hands.Length; j++)
                {
                    for (int k = 0; k < rbs.Length; k++)
                    {
                        // Releases grip of grabbing hand
                        if (hands[j].currentGripping != null && hands[j].currentGripping == rbs[k])
                        {
                            gameManager.players[i].GetComponent<PlayerController>().ReleaseGrip(hands[j].leftHand, false);
                            hands[j].RemoveFromGrabables(rbs[k]);
                        }
                    }
                }
            }

            Destroy(gameObject);
        }
	}
}
