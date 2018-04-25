using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    [SerializeField] float destroyDelay = 1f;

    float timer = 0;
    

	void Update ()
    {
        if (timer >= destroyDelay)
            Destroy(gameObject);

        timer += Time.deltaTime;
	}
}
