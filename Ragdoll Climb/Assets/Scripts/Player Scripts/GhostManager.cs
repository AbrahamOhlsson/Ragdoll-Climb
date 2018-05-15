using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] float blinkInterval = 0.25f;
    [SerializeField] float transparency = 0.5f;
    [SerializeField] float minPlayerDist = 2.5f;
    [SerializeField] float ghostMassMult = 0.8f;

    [SerializeField] MultiplayerManager manager;

    List<Material> originMats = new List<Material>();
    List<Material> transMats = new List<Material>();

    float blinkTimer = 0;
    float ghostTimer = 0;
    float ghostTime = 2;

    float[] startMasses;

    List<GameObject> otherPlayers = new List<GameObject>();

    // Transform of the "Root_M" object of this player
    Transform rootTrans;
    // Transforms of the "Root_M" object of other players
    List<Transform> otherTrans = new List<Transform>();

    List<Collider> myColliders = new List<Collider>();
    List<Collider> otherColliders = new List<Collider>(); 

    Renderer[] rends;

    Rigidbody[] rbs;

    PlayerInfo playerInfo;

	// Use this for initialization
	void Start ()
    {
        playerInfo = GetComponent<PlayerInfo>();

        rootTrans = playerInfo.rootObj.transform;

        rbs = GetComponentsInChildren<Rigidbody>();

        startMasses = new float[rbs.Length];

        rends = transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();

        foreach (Collider coll in GetComponentsInChildren<Collider>())
        {
            if (!coll.name.Contains("wrist") && !coll.name.Contains("Wrist") && !coll.name.Contains("cloth"))
                myColliders.Add(coll);
        }

        for (int i = 0; i < rbs.Length; i++)
        {
            startMasses[i] = rbs[i].mass;
        }
        
        // Gets players
        for (int i = 0; i < manager.players.Count; i++)
        {
            otherPlayers.Add(manager.players[i]);

            // Removes unjoined player if not in debug mode
            if (!Singleton.instance.debug && !manager.players[i].activeSelf)
                otherPlayers.Remove(manager.players[i]);
        }

        // Excludes this player
        otherPlayers.Remove(gameObject);

        for (int i = 0; i < otherPlayers.Count; i++)
        {
            otherTrans.Add(otherPlayers[i].GetComponent<PlayerInfo>().rootObj.transform);

            Collider[] colls = otherPlayers[i].GetComponentsInChildren<Collider>();

            for (int j = 0; j < colls.Length; j++)
            {
                if (!colls[j].name.Contains("Wrist") && !colls[j].name.Contains("wrist") && !colls[j].name.Contains("cloth"))
                    otherColliders.Add(colls[j]);
            }
        }

        
    }
	
	// Update is called once per frame
	void Update ()
    {
        // The player has no collision with other players
        if (!playerInfo.solid)
        {
            // If it is time to blink transparency
            if (blinkTimer >= blinkInterval)
            {
                // Goes through all renderers
                for (int i = 0; i < rends.Length; i++)
                {
                    // If the renderer already is transparent
                    if (/*!rends[i].enabled*/ rends[i].material.color.a == transparency)
                    {
                        rends[i].material = originMats.Find(x => x.name == rends[i].material.name.Replace("_trans (Instance)", ""));

                        // The renderer is not transparent anymore
                        rends[i].material.color = new Color(rends[i].material.color.r, rends[i].material.color.g, rends[i].material.color.b, 1f);
                        //rends[i].enabled = true;
                    }
                    else
                    {
                        rends[i].material = transMats.Find(x => x.name == rends[i].material.name.Replace(" (Instance)", "_trans"));

                        // The renderer is now transparent
                        rends[i].material.color = new Color(rends[i].material.color.r, rends[i].material.color.g, rends[i].material.color.b, transparency);
                        //rends[i].enabled = false;
                    }
                }

                blinkTimer = 0f;
            }

            blinkTimer += Time.deltaTime;

            // If it is time to go solid again
            if (ghostTimer >= ghostTime)
            {
                // Will be used to store the minimum distance to any other player
                float minDist = Mathf.Infinity;

                // Goes throuh all the other players transforms
                for (int i = 0; i < otherTrans.Count; i++)
                {
                    // Gets the distance between this player and another
                    float dist = Vector3.Distance(rootTrans.position, otherTrans[i].position);

                    // If this new distance is lesser that the previously minimum one
                    if (dist < minDist)
                    {
                        minDist = dist;
                    }
                }

                // If the minimum distance to players are great enough to go solid again
                if (minDist > minPlayerDist)
                    Solid();
            }

            ghostTimer += Time.deltaTime;
        }
    }


    // Makes the player solid again
    public void Solid()
    {
        playerInfo.solid = true;

        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material = originMats.Find(x => x.name == rends[i].material.name.Replace("_trans (Instance)", "") || x.name == rends[i].material.name.Replace(" (Instance)", ""));

            // Makes player not transparent
            rends[i].material.color = new Color(rends[i].material.color.r, rends[i].material.color.g, rends[i].material.color.b, 1f);
        }

        // The player will now have collision again with other players
        for (int i = 0; i < myColliders.Count; i++)
        {
            for (int j = 0; j < otherColliders.Count; j++)
            {
                if (!myColliders[i].name.Contains("Wrist") && !myColliders[i].name.Contains("wrist"))
                    Physics.IgnoreCollision(myColliders[i], otherColliders[j], false);
            }
        }

        ResetMass();
    }


    public void Ghost(float time)
    {
        playerInfo.solid = false;

        ghostTime = time;
        ghostTimer = 0;

        // Disconnects all potential players that are grabbing this one
        playerInfo.DisconnectGrabbingPlayers();

        GetComponent<PlayerPowerups>().ResetPlayerMass();

        // Ignores collision between this player and the others
        for (int i = 0; i < myColliders.Count; i++)
        {
            for (int j = 0; j < otherColliders.Count; j++)
                Physics.IgnoreCollision(myColliders[i], otherColliders[j]);
        }

        ChangeMass(ghostMassMult);
    }


    public void SetMats()
    {
        for (int i = 0; i < rends.Length; i++)
        {
            if (!originMats.Exists(x => x.name == rends[i].material.name.Replace(" (Instance)", "")))
            {
                originMats.Add(new Material((Material)Resources.Load("Materials/" + rends[i].material.name.Replace(" (Instance)", ""))));
                transMats.Add(new Material((Material)Resources.Load("Materials/" + originMats[i].name.Replace(" (Instance)", "") + "_trans")));
            }
        }

        for (int i = 0; i < originMats.Count; i++)
        {
            originMats[i].color = playerInfo.color;
            transMats[i].color = playerInfo.color;
        }
    }


    private void ChangeMass(float massMult)
    {
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].mass = massMult * startMasses[i];
        }
    }


    private void ResetMass()
    {
        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].mass = startMasses[i];
        }
    }
}
