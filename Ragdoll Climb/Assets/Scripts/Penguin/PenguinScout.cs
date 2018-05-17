using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinScout : MonoBehaviour
{
    float minAlpha = 0.4f;
    float maxAlpha = 0.8f;
    float targetAlpha;
    float currAlpha;
    float speed = 5f;

    Penguin penguin;
    Material mat;


    void Start ()
    {
        penguin = transform.parent.GetComponent<Penguin>();
        mat = GetComponent<Renderer>().material;

        targetAlpha = minAlpha;
        currAlpha = maxAlpha;
        mat.color = new Color(1, 1, 0, 0);
    }


    public void SetColor(Vector3 rgb)
    {
        mat.color = new Color(rgb.x, rgb.y, rgb.z, currAlpha);
    }


    private void Update()
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
    }


    private void OnTriggerStay(Collider other)
    {
        if (penguin.state == Penguin.PenguinStates.Scout && other.tag == "Player")
        {
            penguin.PrepareLaunch(other.transform.root.GetComponent<PlayerInfo>().rootObj.transform);

            mat.color = new Color(1, 0, 0, currAlpha);
        }
    }
}
