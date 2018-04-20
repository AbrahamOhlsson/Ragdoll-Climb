using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAndDoor : MonoBehaviour {

    //Transform door;
    [SerializeField] Transform button;
    [SerializeField] Transform buttonStop;

    private GameObject player;

    List<Vector3> doorDestinations = new List<Vector3>();
    List<Vector3> reversedDoorDestinations = new List<Vector3>();

    [SerializeField] public List<Transform> doorList;


    float sinkSpeed;
    float lerpTime = 1;
    float currentLerpTime;
    bool pressed;
    bool cantBePressed;

    enum closeDoor{openDoor, closeDoor};
    [SerializeField] closeDoor doorEnum;

    private Color Activated;
    Renderer rend;

    // Use this for initialization
    void Start ()
    {
        pressed = false;
        cantBePressed = false;
        //getDoorChildren();
        rend = button.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
  
        if (pressed == true)
        {
            currentLerpTime += Time.deltaTime;

            if ( currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            sinkSpeed = currentLerpTime / lerpTime;

            //The button lerp
            button.transform.position = Vector3.Lerp(transform.position, buttonStop.position, sinkSpeed * 1.02f);

            ////Makes the button green.
            //button.gameObject.GetComponent<Renderer>().material.color = Color.green; //Normal color
            //button.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green); //Emission color
            rend.material.color = Color.green; //Normal color
            rend.material.SetColor("_EmissionColor", Color.green); //Emission color


            //Lerps the doors
            for (int i = 0; i < doorList.Count; i++)
            {
                if (doorEnum == closeDoor.openDoor)
                {
                    doorList[i].transform.position = Vector3.Lerp(doorList[i].transform.position, doorDestinations[i], sinkSpeed);
                }
                else if(doorEnum == closeDoor.closeDoor)
                {
                    doorList[i].transform.position = Vector3.Lerp(doorList[i].transform.position, reversedDoorDestinations[i], sinkSpeed);
                }
            }
        }
    }

    void getDoorChildren()
    {
        foreach (Transform childs in transform)
        {
            if (childs.tag == "Door")
            {
                doorList.Add(childs.transform);
            }
        }

        for (int i = 0; i < doorList.Count; i++)
        {
            doorDestinations.Add(new Vector3(doorList[i].transform.position.x, doorList[i].transform.position.y, doorList[i].transform.position.z + 1f));

            reversedDoorDestinations.Add(new Vector3(doorList[i].transform.position.x, doorList[i].transform.position.y, doorList[i].transform.position.z - 1f));
            //moves the door
            //childs.transform.position = Vector3.Lerp(childs.transform.position, doorDestination, sinkSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.transform.gameObject;

        if (cantBePressed == false)
        {
            if (player.tag == "Player")
            {
                cantBePressed = true;
                pressed = true;
                getDoorChildren();
            }
        }
    }
}
