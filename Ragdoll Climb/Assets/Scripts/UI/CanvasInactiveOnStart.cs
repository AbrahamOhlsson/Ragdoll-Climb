using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInactiveOnStart : MonoBehaviour {



    bool firstUpdate = false;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firstUpdate)
        {
            gameObject.SetActive(false);
            Destroy(this);
        }

        firstUpdate = true;
    }

}
