using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseGripCollider : MonoBehaviour {

    bool firstupdate = true;

	void Start () {
        transform.localPosition = new Vector3(0f, 0f, 0f);
        print("start of ReleaseGripCollider");
	}
    private void Update()
    {
        if (firstupdate)
        {
            transform.localPosition = new Vector3(0f, 0f, 0f);
            firstupdate = false;
        }
        
    }



    private void OnCollisionEnter(Collision other)
    {
        print( "test i rock namn=  " + other.transform.root.name);
        if (other.transform.root.tag == "Player")
        {
            other.transform.root.GetComponent<PlayerController>().ReleaseGrip(false, false);
            other.transform.root.GetComponent<PlayerController>().ReleaseGrip(true, false);
            other.transform.root.GetComponent<PlayerController>().checkGripLeft.RemoveFromGrabables(transform.parent.GetComponent<Rigidbody>());
            other.transform.root.GetComponent<PlayerController>().checkGripRight.RemoveFromGrabables(transform.parent.GetComponent<Rigidbody>());
            

            print("player hit");
            
            other.transform.root.GetComponent<PlayerStun>().miniStun(0.005f);
            //Destroy(gameObject);
        }
    }
}
