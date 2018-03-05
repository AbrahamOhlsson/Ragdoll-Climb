using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBlock : MonoBehaviour
{
    public MultiplayerManager gameManager;
    public GameObject ship;
    public float distance;

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y < (ship.transform.position.y + distance))
        {
            for (int i = 0; i < gameManager.players.Count; i++)
            {
                CheckGrip[] hands = gameManager.players[i].GetComponentsInChildren<CheckGrip>();

                for (int j = 0; j < hands.Length; j++)
                {
                    if (hands[j].currentGripping != null && hands[j].currentGripping.gameObject == gameObject)
                        gameManager.players[i].GetComponent<PlayerController>().ReleaseGrip(hands[j].leftHand, false);
                }
            }

            Destroy(gameObject);
        }
	}
}
