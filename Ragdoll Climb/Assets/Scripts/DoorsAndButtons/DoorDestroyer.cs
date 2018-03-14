using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDestroyer : MonoBehaviour {
    [SerializeField]
    GameObject button;
    [SerializeField]
    GameObject door;
    GameObject player;

    Renderer renderers;

    private Color notActivated;
    private Color activated;

	// Use this for initialization
	void Start ()
    {
        notActivated = Color.red;
        activated = Color.green;
        gameObject.GetComponent<Renderer>().material.color = notActivated;

    }


    void OnTriggerEnter(Collider other)
    {
        player = other.transform.gameObject;

        if (player.tag == "Player")
        {
            Destroy(door);
            gameObject.GetComponent<Renderer>().material.color = activated;
        }

    }


    // Update is called once per frame
    void Update ()
    {
       
    }
}
