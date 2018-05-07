using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInactiveOnStart : MonoBehaviour {



    bool firstUpdate = true;
    int timeInt;
    // Use this for initialization
    void Start()
    {
        timeInt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeInt >= 10)
        {
            if (!firstUpdate)
            {

                GetComponent<Canvas>().enabled = true;
                gameObject.SetActive(false);
                Destroy(this);
            }

           
        }
        timeInt++;

        if (firstUpdate)
        {
            GetComponent<Canvas>().enabled = false;

            firstUpdate = false;
        }
    }

}
