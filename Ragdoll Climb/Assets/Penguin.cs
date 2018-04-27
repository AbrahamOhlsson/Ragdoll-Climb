using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    internal enum PenguinStates { Spawned, Scout, Prepare, Launch }
    internal PenguinStates state = PenguinStates.Spawned;

    [SerializeField] Transform body;

    [SerializeField] Transform scoutPoint;
    [SerializeField] Transform doorPoint;
    [SerializeField] Transform launchPoint;

    float pointOffset = 0.1f;
    float rotOffset = 1f;

    Vector3 currentRot;


    void Start()
    {
        currentRot = body.eulerAngles;
    }



    void Update()
    {
        switch (state)
        {
            case PenguinStates.Spawned:
                MoveToPoint(scoutPoint, 1f);
                if (CloseEnough(body.position, scoutPoint.position, pointOffset))
                {
                    Rotate(new Vector2(0, 90), 1f);

                    if (CloseEnough(body.eulerAngles, new Vector2(0, 90), 1f))
                        state = PenguinStates.Scout;
                }
                break;
        }
    }


    void MoveToPoint(Transform point, float spd)
    {
        body.position = Vector3.Lerp(body.position, point.position, spd * Time.deltaTime);
    }

    void Rotate(Vector2 rot, float spd)
    {
        currentRot = new Vector3(Mathf.LerpAngle(currentRot.x, rot.x, spd * Time.deltaTime), Mathf.LerpAngle(currentRot.y, rot.y, spd * Time.deltaTime), body.eulerAngles.z);

        body.eulerAngles = currentRot;
    }

    bool CloseEnough(Vector3 v1, Vector3 v2, float offset)
    {
        if (Vector3.Distance(v1, v2) <= offset)
            return true;
        else
            return false;
    }
}
