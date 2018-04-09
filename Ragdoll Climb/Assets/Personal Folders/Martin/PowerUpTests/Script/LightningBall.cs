using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class LightningBall : MonoBehaviour {

    [SerializeField] GameObject LightningBallPrefab;
    [SerializeField] Transform lightningBall;
    [SerializeField] Transform PlayerPos;

    public Vector3 relativeDistance = Vector3.zero;

    [SerializeField]
    float orbitDistance;
    [SerializeField]
    float OrbitDegreesPerSec;
    [SerializeField]
    float ballSpeed;

    Vector3 ballDirection;

    bool once = true;
    internal bool spawnPrefab = true;
    [SerializeField] bool haveBall;
    public bool ballIsfree;

    PlayerIndex playerIndex;
    GamePadState state;

    // Use this for initialization
    void Start ()
    {
        ballIsfree = false;
    }
	
   public void Orbit()
    {
        //Keep us at the last known relatve position
        lightningBall.position = (PlayerPos.position + relativeDistance);
        lightningBall.RotateAround(PlayerPos.position, Vector3.forward, OrbitDegreesPerSec * Time.deltaTime);

        //Reset relative position after rotate
        if (once)
        {
            lightningBall.position *= orbitDistance;
            once = false;
        }
        relativeDistance = lightningBall.position - PlayerPos.position;
    }

    public void Initiation()
    {
        if (spawnPrefab == true)
        {
            spawnPrefab = false;
            lightningBall = Instantiate(LightningBallPrefab, new Vector3(0, 0, 0), Quaternion.identity).transform;
            haveBall = true;
            ballIsfree = false;
        }
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        if (ballIsfree == false)
        {
            Orbit();
            relativeDistance = lightningBall.position - PlayerPos.position;
        }


    }
    private void FixedUpdate()
    {
        state = GamePad.GetState(playerIndex);
       


        if (ballIsfree == true)
        {
            haveBall = false;
            lightningBall.Translate(ballDirection * ballSpeed * Time.deltaTime, Space.World);
        }

       

        if (state.DPad.Up == ButtonState.Pressed && haveBall == true)
        {
            Vector2 heading = lightningBall.position - PlayerPos.position;
            float distance = heading.magnitude;
            ballDirection = heading / distance;
            print(ballDirection);
            
            ballIsfree = true;
        }
    }
}
