using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAndDoor : MonoBehaviour {

    [SerializeField]
    Transform button;
    [SerializeField]
    Transform buttonStop;

    private GameObject player;

    [SerializeField]
    GameObject door;
    
    float sinkSpeed;
    float lerpTime = 1;
    float currentLerpTime;
    bool pressed;

    private Color Activated;

	// Use this for initialization
	void Start ()
    {
        pressed = false;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        Renderer rend = button.GetComponent<Renderer>();

        if (pressed == true)
        {
            currentLerpTime += Time.deltaTime;

            if ( currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            sinkSpeed = currentLerpTime / lerpTime;

            button.transform.position = Vector3.Lerp(transform.position, buttonStop.position, sinkSpeed * 1.02f);

            if (sinkSpeed == 1)
            {
                ////Makes the button green.
                button.gameObject.GetComponent<Renderer>().material.color = Color.green; //Normal color
                button.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green); //Emission color
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.transform.gameObject;
        if(player.tag == "Player")
        {
            pressed = true;
        }

    }
}
