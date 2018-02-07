using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Translate(x, y, 0);
    }
}
