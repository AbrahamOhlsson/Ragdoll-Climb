using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRandom : MonoBehaviour
{
    GameObject PlayerTP;
    public GameObject particleSys;

   Vector3 teleportPos = Vector3.zero;
    
    [SerializeField]
    float sphereRadius;

    int layerMask = 1 << 20;
    Collider[] teleportPoints;

    private void Start()
    {
        teleportPoints = Physics.OverlapSphere(transform.position, sphereRadius, layerMask, QueryTriggerInteraction.Collide);

        //print(teleportPoints.Length);
        //teleportPos = teleportPoints[Random.Range(0, teleportPoints.Length)].transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerTP = other.transform.root.gameObject;
            GetTeleportPosition();
            Destroy(gameObject);
        }
        //else if (other.tag == "BottomObj")
        //    Destroy(gameObject);
    }

    void GetTeleportPosition()
    {
        teleportPos = teleportPoints[Random.Range(0, teleportPoints.Length)].transform.position;

        if (teleportPos != Vector3.zero)
        {
            PlayerTP.GetComponent<PlayerPowerups>().StartTeleport(teleportPos);
        }
        else
        {
            Debug.LogError("No Teleport points were found!");
        }
    }

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
	 //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
		Gizmos.DrawWireSphere(transform.position, sphereRadius);
	}
}
