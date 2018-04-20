using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inactiveOnStart : MonoBehaviour {

    bool firstUpdate = false;
	// Use this for initialization

	void Start () {
       gameObject.SetActive(false);
       Destroy(this);

    }

}
