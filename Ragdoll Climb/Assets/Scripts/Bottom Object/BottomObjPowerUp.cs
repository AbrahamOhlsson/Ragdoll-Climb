using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomObjPowerUp : MonoBehaviour {

	[SerializeField] float spawnOffset = 10; 
	[SerializeField] GameObject[] powerUps;
	[SerializeField] Transform powerupParent;
	Transform lastTrans; // ändra till vector3 från transform så vi inte får error 

	int index;

    float nullTimer = 0;
    float nullSpawnDelay = 3f;

	private void Start()
	{
		lastTrans = Instantiate(powerUps[index], transform.position, transform.rotation, powerupParent).AddComponent<MoveToRight>().transform;
	}

	void Update()
	{
        if (lastTrans == null)
        {
            if (nullTimer >= nullSpawnDelay)
            {
                index = Random.Range(0, powerUps.Length);
                lastTrans = Instantiate(powerUps[index], transform.position, transform.rotation, powerupParent).AddComponent<MoveToRight>().transform;
                nullTimer = 0;
            }

            nullTimer += Time.deltaTime;
        }

		if(lastTrans != null && Vector3.Distance(lastTrans.position, transform.position) >= spawnOffset)
		{
            index = Random.Range(0, powerUps.Length);
            lastTrans = Instantiate(powerUps[index], transform.position, transform.rotation, powerupParent).AddComponent<MoveToRight>().transform;
		}
	}
}
