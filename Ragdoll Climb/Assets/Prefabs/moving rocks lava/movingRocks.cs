using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingRocks : MonoBehaviour {

    bool loseGrip = false;
    [SerializeField] float fallSpeed;

    [SerializeField] Transform topObj;
    [SerializeField] GameObject botomObj;

    [SerializeField] GameObject relliseColider;

    int timeTest;

    // Use this for initialization
    void Start()
    {
        timeTest = 0;
    }

    // Update is called once per frame
    void Update()
    {
        print(tag);

        transform.position = transform.position + ((-Vector3.up * fallSpeed) * Time.deltaTime);

        if (transform.position.y > botomObj.transform.position.y)
        {
            transform.tag = "LavaRock";
            loseGrip = false;
        }


        if (transform.position.y < botomObj.transform.position.y)
        {
            // Vector3 test = new Vector3(0f, 0f, 0f);//transform.position;
            transform.tag = "Untagged";
            
            Instantiate(relliseColider,transform);

            if (timeTest == 5)
            {
                //Instantiate(relliseColider, transform);
                transform.position = transform.position - Vector3.up * ((botomObj.transform.position.y - topObj.position.y));

                timeTest = 0;
            }
            timeTest++;
        }
    }
}
