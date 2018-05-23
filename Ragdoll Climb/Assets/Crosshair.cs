using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    float minAlpha = 0.4f;
    float maxAlpha = 0.8f;
    float targetAlpha;
    float currAlpha;
    float speed = 6f;

    Material mat;


    void Awake ()
    {
        mat = GetComponent<Renderer>().material;

        targetAlpha = minAlpha;
        currAlpha = maxAlpha;
    }


    void Update ()
    {
        if (Mathf.Abs(mat.color.a - targetAlpha) <= 0.01f)
        {
            if (targetAlpha == minAlpha)
                targetAlpha = maxAlpha;
            else
                targetAlpha = minAlpha;
        }

        currAlpha = Mathf.Lerp(currAlpha, targetAlpha, speed * Time.deltaTime);

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, currAlpha);

        transform.eulerAngles = Vector3.zero;
    }
}
