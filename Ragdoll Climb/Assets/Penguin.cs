using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    public enum PenguinStates { Spawned, Scout, GoToDoor, GoOut, Launched }
    public PenguinStates state = PenguinStates.Spawned;

    [SerializeField] float launchForce = 100f;

    [SerializeField] Transform body;

    [SerializeField] Transform scoutPoint;
    [SerializeField] Transform doorPoint;
    [SerializeField] Transform launchPoint;

    bool rotatedTowards = false;
    bool targetToLeft = false;

    float pointOffset = 0.3f;
    float rotOffset = 1f;

    float launchAngleX;

    Vector3 currentRot;
    Vector3 targetRot;

    Vector3 spawnPos;
    Quaternion spawnRot;

    Transform playerTarget;


    void Start()
    {
        body.eulerAngles = TargetFaceRotation(scoutPoint);

        currentRot = body.eulerAngles;

        spawnPos = body.position;
        spawnRot = body.rotation;
    }

    
    void Update()
    {
        switch (state)
        {
            case PenguinStates.Spawned:
                MoveToPoint(scoutPoint, 1f);
                if (CloseEnough(body.position, scoutPoint.position, pointOffset))
                {
                    Rotate(new Vector2(0.1f, 0.1f), 2f);

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
                    
                    Rotate(targetRot, 4f);

                    if (CloseEnough_Angle(body.eulerAngles, targetRot, rotOffset))
                        rotatedTowards = true;
                }
                else
                {
                    MoveToPoint(doorPoint, 1.5f);
                    if (CloseEnough(body.position, doorPoint.position, pointOffset))
                    {
                        Rotate(new Vector2(0, 0), 4f);

                        if (CloseEnough_Angle(body.eulerAngles, new Vector3(0, 0, -90), rotOffset))
                        {
                            rotatedTowards = false;
                            state = PenguinStates.GoOut;
                        }
                    }
                }
                break;

            case PenguinStates.GoOut:
                MoveToPoint(launchPoint, 2f);

                if (CloseEnough(body.position, launchPoint.position, pointOffset))
                {
                    targetRot = TargetFaceRotation(playerTarget);
                    Rotate(targetRot, 4f);

                    if (CloseEnough_Angle(body.eulerAngles, targetRot, rotOffset))
                    {
                        Vector3 dir = (playerTarget.position - body.position).normalized;
                        body.GetComponent<Rigidbody>().isKinematic = false;
                        body.GetComponent<Rigidbody>().AddForce(dir * launchForce);

                        launchAngleX = CalculateLaunchAngle(playerTarget);

                        state = PenguinStates.Launched;
                    }
                }
                break;

            case PenguinStates.Launched:
                Rotate(new Vector2(launchAngleX, targetRot.y), 8f);
                break;
        }
    }


    public void PrepareLaunch(Transform _playerTarget)
    {
        state = PenguinStates.GoToDoor;

        playerTarget = _playerTarget;
    }


    public void Respawn()
    {
        body.position = spawnPos;
        body.rotation = spawnRot;
        currentRot = body.eulerAngles;

        rotatedTowards = false;
        body.GetComponent<Rigidbody>().isKinematic = true;

        state = PenguinStates.Spawned;
    }


    Vector3 TargetFaceRotation(Transform target)
    {
        if (target.position.x < body.position.x)
            return new Vector3(0f, 90f, -90);
        else
            return new Vector3(0f, -90f, -90);
    }

    void MoveToPoint(Transform point, float spd)
    {
        body.position = Vector3.Lerp(body.position, point.position, spd * Time.deltaTime);
    }

    float CalculateLaunchAngle(Transform target)
    {
        Vector2 dist = target.position - body.position;
        float v = Mathf.Atan(dist.y / dist.x) * Mathf.Rad2Deg;

        if (target.position.y > body.position.y)
            return (90 - v) * -1;
        else
            return (90 + v) * -1;
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
        if (Mathf.Abs(Mathf.DeltaAngle(v1.y, v2.y)) <= offset)
            return true;
        else
            return false;
    }
}
