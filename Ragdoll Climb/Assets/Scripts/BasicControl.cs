using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicControl : MonoBehaviour {

    public float mSpeed;


    private void Start()
    {
        mSpeed = 1f;
    }

    // Update is called once per frame
    void Update ()
    {
        transform.Translate(0f, mSpeed * Input.GetAxis("Vertical") * Time.deltaTime, 0f);
	}
}
