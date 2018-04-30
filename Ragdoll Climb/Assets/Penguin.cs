using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    internal enum PenguinStates { Spawned, Scout, GoToDoor, GoOut, Launch }
    internal PenguinStates state = PenguinStates.Spawned;

    [SerializeField] float launchForce = 100f;

    [SerializeField] Transform body;

    [SerializeField] Transform scoutPoint;
    [SerializeField] Transform doorPoint;
    [SerializeField] Transform launchPoint;

    bool rotatedTowards = false;
    bool targetToLeft = false;

    float pointOffset = 0.3f;
    float rotOffset = 1f;

    Vector3 currentRot;

    Transform playerTarget;


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
                    Rotate(new Vector2(0, 0), 2f);

                    if (CloseEnough_Angle(body.eulerAngles, new Vector3(0, 0, -90), rotOffset))
                        state = PenguinStates.Scout;
                }
                break;

            case PenguinStates.Scout:
                // Waits for Scout-script on View-object to spot the player
                break;

            case PenguinStates.GoToDoor:
                if (!rotatedTowards)
                {
                    Vector3 targetRot = TargetFaceRotation(doorPoint);
                    
                    Rotate(targetRot, 3f);

                    if (CloseEnough_Angle(body.eulerAngles, targetRot, rotOffset))
                        rotatedTowards = true;
                }
                else
                {
                    MoveToPoint(doorPoint, 1.5f);
                    if (CloseEnough(body.position, doorPoint.position, pointOffset))
                    {
                        Rotate(new Vector2(0, 0), 3f);

                        if (CloseEnough_Angle(body.eulerAngles, new Vector3(0, 0, -90), rotOffset))
                        {
                            rotatedTowards = false;
                            state = PenguinStates.GoOut;
                        }
                    }
                }
                break;

            case PenguinStates.GoOut:
                MoveToPoint(launchPoint, 1f);

                if (CloseEnough(body.position, launchPoint.position, pointOffset))
                {
                    Vector3 targetRot = TargetFaceRotation(playerTarget);
                    Rotate(targetRot, 3f);

                    if (CloseEnough_Angle(body.eulerAngles, targetRot, rotOffset))
                        state = PenguinStates.Launch;
                }
                break;

            case PenguinStates.Launch:
                Vector3 dir = (playerTarget.position - body.position).normalized;
                body.GetComponent<Rigidbody>().AddForce(dir * launchForce);
                body.GetComponent<Rigidbody>().isKinematic = false;
                break;
        }
    }


    public void PrepareLaunch(Transform _playerTarget)
    {
        state = PenguinStates.GoToDoor;

        playerTarget = _playerTarget;
    }


    Vector3 TargetFaceRotation(Transform target)
    {
        if (target.position.x < 0)
            return new Vector3(0f, 90f, -90);
        else
            return new Vector3(0f, -90f, -90);
    }

    void MoveToPoint(Transform point, float spd)
    {
        body.position = Vector3.Lerp(body.position, point.position, spd * Time.deltaTime);
    }

    void RotateTowardsPoint(Transform target)
    {
        Vector3 targetDir = target.position - transform.position;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 1f * Time.deltaTime, 0.0f);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void Rotate(Vector2 rot, float spd)
    {
        currentRot = new Vector3(Mathf.LerpAngle(currentRot.x, rot.x, spd * Time.deltaTime), Mathf.LerpAngle(currentRot.y, rot.y, spd * Time.deltaTime), body.eulerAngles.z);
        
        body.eulerAngles = currentRot;
    }

    bool CloseEnough(Vector3 v1, Vector3 v2, float offset)
    {
        if (Mathf.Abs(Vector3.Distance(v1, v2)) <= offset)
            return true;
        else
            return false;
    }
    bool CloseEnough_Angle(Vector3 v1, Vector3 v2, float offset)
    {
        print(Mathf.Abs(Mathf.DeltaAngle(v1.y, v2.y)));

        if (Mathf.Abs(Mathf.DeltaAngle(v1.y, v2.y)) <= offset)
            return true;
        else
            return false;
    }
}
