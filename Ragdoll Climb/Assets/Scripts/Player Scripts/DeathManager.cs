using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [SerializeField] float spawnPointSearchRad = 20f;
    [SerializeField] float ghostTime = 3f;

    [SerializeField] Transform bottomObj;
    [SerializeField] MultiplayerManager manager;
	[SerializeField] GameObject SkullAndCrossbones;

    //bool diedRecently = false;

    // Layer mask for 14 that prevents ignorance of that layer when using this mask in overlapping sphere
    int layerMask = 1 << 14;

    // Transform of the "Root_M" object of this player
    Transform rootTrans;

    Transform[] transforms;
    Vector3[] originPos;
    Quaternion[] originRot;

    Rigidbody[] rbs;
    
    PlayerInfo playerInfo;

    // LUDVIG FIX 
    bool firstUpdate;

    // death Grunts
    public Sound[] deathGrunts;
    [HideInInspector] public List<AudioSource> GruntsList;

    void Awake()
    {
        foreach (Sound i in deathGrunts)
        {
            i.source = gameObject.AddComponent<AudioSource>();
            i.source.clip = i.clip;

            i.source.volume = i.volume;
            i.source.pitch = i.pitch;
            i.source.playOnAwake = false;

            GruntsList.Add(i.source);
        }
    }


    void Start ()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        
        playerInfo = GetComponent<PlayerInfo>();

        rootTrans = playerInfo.rootObj.transform;

        transforms = GetComponentsInChildren<Transform>();

        originPos = new Vector3[transforms.Length];
        originRot = new Quaternion[transforms.Length];

        for (int i = 0; i < transforms.Length; i++)
        {
            originPos[i] = transforms[i].localPosition;
            originRot[i] = transforms[i].localRotation;
        }
    }


    private void Update()
    {
        // Death if below the bottom object
        if (rootTrans.position.y < bottomObj.position.y)
            Death();

        // Death if ragdoll explodes
        foreach (Rigidbody i in rbs)
        {
            if (i.tag != "Slippery")
            {
                //Check distance between regidbodies and root_M
                if (Vector3.Distance(i.transform.position, rootTrans.position) > 3)
                    Death();
            }
        }
    }


    // Kills the player
    public void Death()
    {
        //Spawn visual feedback
        Instantiate(SkullAndCrossbones, rootTrans.position, Quaternion.Euler(90, -180, 0));

        //diedRecently = true;
            
        GetComponent<PlayerStun>().UnStun();
        GetComponent<VibrationManager>().VibrateTimed(1f, 1f, 20);
            
        // Gets all spawnpoints within a radius
        Collider[] spawnPoints = Physics.OverlapSphere(rootTrans.position, spawnPointSearchRad, layerMask, QueryTriggerInteraction.Collide);

        // Will be used to store the minimum distance to any spawn point
        float minDist = Mathf.Infinity;

        // Position to spawn at
        Vector3 spawnPos = Vector3.zero;

        // Goes through all found spawn points
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // Gets the distance between the player and a spawn point
            float dist = Vector3.Distance(rootTrans.position, spawnPoints[i].transform.position);

            // If this distance is lesser that the previously minimum one
            if (dist < minDist)
            {
                minDist = dist;
                spawnPos = new Vector3(spawnPoints[i].transform.position.x, spawnPoints[i].transform.position.y, rootTrans.position.z);
            }
        }

        // Resets velocity
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].velocity = Vector3.zero;
        }

        // If a spawn point was found
        if (spawnPos != Vector3.zero)
        {
            // Teleports the player
            rootTrans.transform.position = spawnPos;

            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].name != "Root_M")
                    transforms[i].localPosition = originPos[i];
                else
                    transforms[i].localPosition = new Vector3(transforms[i].localPosition.x, transforms[i].localPosition.y, originPos[i].z);

                transforms[i].localRotation = originRot[i];
            }
        }
        else
        {
            Debug.LogError("No spawn points were found!");
            manager.GetComponent<DebugText>().AddText("ERROR!!! No spawn points were found!");
        }

        GetComponent<GhostManager>().Ghost(ghostTime);
            
        playDeathGrunt();
    }


    public void playDeathGrunt()
    {
        int i = Random.Range(0, GruntsList.Count);
        GruntsList[i].pitch = Random.Range(0.9f, 1.1f);
        GruntsList[i].Play();
    }
}
