using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle_NonDrop : MonoBehaviour
{
    public float growthSpeed = 0.1f;
    
    internal bool grow = false;

    float targetScale;
    float minScale = 0;
    float scale;
    

    void Start()
    {
        targetScale = transform.localScale.x;
        transform.localScale = new Vector3(minScale, minScale, minScale);
        scale = transform.localScale.x;
    }

    void Update()
    {
        if (grow)
        {
            scale += growthSpeed * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);

            if (scale >= targetScale)
                grow = false;
        }
    }
}
