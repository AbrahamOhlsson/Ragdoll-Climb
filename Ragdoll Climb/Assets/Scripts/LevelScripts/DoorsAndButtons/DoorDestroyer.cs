using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDestroyer : MonoBehaviour {
    [SerializeField]
    GameObject button;
    [SerializeField]
    GameObject door;
    [SerializeField]
    GameObject player;

    Renderer renderers;

    private Color notActivated;
    private Color activated;

	// Use this for initialization
	void Start ()
    {
        notActivated = Color.red;
        activated = Color.green;

        //Makes the Door Red
        door.gameObject.GetComponent<Renderer>().material.color = Color.red;

        //Makes the button grey-ish before it activates.
        gameObject.GetComponent<Renderer>().material.color = notActivated;
    }


    void OnTriggerEnter(Collider other)
    {
        player = other.transform.gameObject;

        if (player.tag == "Player")
        {
            Destroy(door);

            //Switches the color to green when activated.
            gameObject.GetComponent<Renderer>().material.color = activated;
        }
    }
}