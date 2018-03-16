using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [SerializeField] float blinkInterval = 0.25f;
    [SerializeField] float transparency = 0.5f;
    [SerializeField] float minPlayerDist = 3f;
    [SerializeField] float spawnPointSearchRad = 20f;
    [SerializeField] float invincibleTime = 3f;

    [SerializeField] Transform bottomObj;
    [SerializeField] MultiplayerManager manager;

    bool solid = true;

    // Layer mask for 14 that prevents ignorance of that layer when using this mask in overlappin sphere
    int layerMask = 1 << 14;

    float blinkTimer = 0f;
    float invincibleTimer = 0f;

    List<GameObject> otherPlayers = new List<GameObject>();

    // Transform of the "Root_M" object of this player
    Transform rootTrans;
    // Transforms of the "Root_M" object of other players
    List<Transform> otherTrans = new List<Transform>();

    Collider[] myColliders;
    List<Collider> otherColliders = new List<Collider>();

    Renderer[] rends;


	void Start ()
    {
        // Gets all the players
        for (int i = 0; i < manager.players.Count; i++)
        {
            otherPlayers.Add(manager.players[i]);
        }

        // Excludes this player
        otherPlayers.Remove(gameObject);

        rootTrans = GetRoot(gameObject);

        rends = transform.GetChild(0).GetComponentsInChildren<Renderer>();

        myColliders = GetComponentsInChildren<Collider>();

        // Gets stuff from the other players
        for (int i = 0; i < otherPlayers.Count; i++)
        {
            Collider[] colls = otherPlayers[i].GetComponentsInChildren<Collider>();

            for (int j = 0; j < colls.Length; j++)
            {
                otherColliders.Add(colls[j]);
            }

            otherTrans.Add(GetRoot(otherPlayers[i]));
        }
	}
	

	void Update ()
    {
        // The player has no collision with other players
		if (!solid)
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
                        // The renderer is not transparent anymore
                        rends[i].material.color = new Color(rends[i].material.color.r, rends[i].material.color.g, rends[i].material.color.b, 1f);
                        //rends[i].enabled = true;
                    }
                    else
                    {
                        // The renderer is now transparent
                        rends[i].material.color = new Color(rends[i].material.color.r, rends[i].material.color.g, rends[i].material.color.b, transparency);
                        //rends[i].enabled = false;
                    }
                }

                blinkTimer = 0f;
            }

            blinkTimer += Time.deltaTime;

            // If it is time to go solid again
            if (invincibleTimer >= invincibleTime)
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

            invincibleTimer += Time.deltaTime;
        }

        if (rootTrans.position.y < bottomObj.position.y)
            Death();
	}


    // Gets the "Root_M" object of players
    private Transform GetRoot(GameObject obj)
    {
        Rigidbody[] limbs = obj.GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < limbs.Length; i++)
        {
            if (limbs[i].name == "Root_M")
                return limbs[i].transform;
        }

        return null;
    }


    // Kills the player
    public void Death()
    {
        solid = false;
        invincibleTimer = 0f;

        // Disconnects all potential layers that are grabbing this one
        GetComponent<PlayerInfo>().DisconnectGrabbingPlayers();

        // Ignores collision between this player and the others
        for (int i = 0; i < myColliders.Length; i++)
        {
            for (int j = 0; j < otherColliders.Count; j++)
            {
                Physics.IgnoreCollision(myColliders[i], otherColliders[j]);
            }
        }

        // Gets all spawnpoints within a radius
        Collider[] spawnPoints = Physics.OverlapSphere(transform.position, spawnPointSearchRad, layerMask, QueryTriggerInteraction.Collide);

        // Will be used to store the minimum distance to any spawn point
        float minDist = Mathf.Infinity;

        // Position to spawn at
        Vector3 spawnPos = Vector3.zero;

        // Goes through all found spawn points
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // Gets the distance between the player and a spawn point
            float dist = Vector3.Distance(transform.position, spawnPoints[i].transform.position);

            // If this distance is lesser that the previously minimum one
            if (dist < minDist)
            {
                minDist = dist;
                spawnPos = spawnPoints[i].transform.position;
            }
        }

        // If a spawn point was found
        if (spawnPos != Vector3.zero)
        {
            // Teleports the player
            rootTrans.transform.position = spawnPos;
        }
        else
        {
            Debug.LogError("No spawn points were found!");
        }
    }


    // Makes the player solid again
    public void Solid()
    {
        solid = true;

        for (int i = 0; i < rends.Length; i++)
        {
            // Makes player not transparent
            rends[i].material.color = new Color(rends[i].material.color.r, rends[i].material.color.g, rends[i].material.color.b, 1f);
            //rends[i].enabled = true;
        }

        // The player will now have collision again with other players
        for (int i = 0; i < myColliders.Length; i++)
        {
            for (int j = 0; j < otherColliders.Count; j++)
            {
                Physics.IgnoreCollision(myColliders[i], otherColliders[j], false);
            }
        }
    }
}
