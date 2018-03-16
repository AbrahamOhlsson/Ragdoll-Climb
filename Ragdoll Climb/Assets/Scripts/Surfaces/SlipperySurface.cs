using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperySurface : MonoBehaviour {

    [SerializeField]
    GameObject rightGrabObject;
    [SerializeField]
    GameObject leftGrabObject;

    public Component[] Rigidbodies;

    bool rightWrist;
    bool leftWrist;

    [SerializeField]
    float glideForce;

    [SerializeField]
    float helpingForce;

    // Use this for initialization
    void Start ()
    {
        Rigidbodies = GetComponentsInChildren<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (rightWrist)
            Gliding(rightGrabObject);
        if (leftWrist)
            Gliding(leftGrabObject);
    }


    public void SetIsGrabbing(bool leftHand, bool grabbing)
    {
        if (leftHand)
            leftWrist = grabbing;
        else
            rightWrist = grabbing;

        print("Grabbing = " + grabbing);
    }


    void Gliding(GameObject hand)
    {
        hand.GetComponent<Rigidbody>().position = Vector3.Lerp(hand.GetComponent<Rigidbody>().position, hand.GetComponent<Rigidbody>().position - new Vector3(0f, 10f, 0f), glideForce);
        hand.GetComponent<Rigidbody>().AddForce(hand.GetComponent<Rigidbody>().position - new Vector3(0f, 1f, 0f) * helpingForce);
    }
}
