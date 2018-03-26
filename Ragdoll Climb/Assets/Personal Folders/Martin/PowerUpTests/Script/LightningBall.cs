using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : MonoBehaviour {

    public Transform Player;
    public Vector3 relativeDistance = Vector3.zero;
    [SerializeField]
    float orbitDistance;
    [SerializeField]
    float OrbitDegreesPerSec;

    bool once = true;

	// Use this for initialization
	void Start ()
    {
		
	}
	
    void Orbit()
    {
        if(Player != null)
        {
            //Keep us at the last known relatve position
            transform.position = (Player.position + relativeDistance);
            transform.RotateAround(Player.position, Vector3.up, OrbitDegreesPerSec * Time.deltaTime);

            //Reset relative position after rotate
            if(once)
            {
                transform.position *= orbitDistance;
                once = false;
            }
            relativeDistance = transform.position - Player.position;
        }
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        Orbit();
        if (Player != null)
        {
            relativeDistance = transform.position - Player.position;
        }
    }
}
