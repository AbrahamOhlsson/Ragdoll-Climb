using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballRotate : MonoBehaviour {

	[SerializeField]
	private int rotationMax = 0;
	[SerializeField]
	private float randomTimer = 0;
	private float fireballTime = 0;

	[SerializeField] bool canRotate;

	private void Start()
	{
        if (!canRotate) print("fire ball bool = "+ canRotate);

        if (!canRotate) Destroy(this);
	}

	void Update ()
    {
        if (canRotate)
        {
            fireballTime = fireballTime + (1 * Time.deltaTime);
            print("eld");
            if (fireballTime >= randomTimer)
            {
                print("eld");
                transform.rotation = Quaternion.Euler(Random.Range(0, rotationMax), 90, 0);
                fireballTime = 0;
            }
        }
    }

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, new Vector3(0,10,0));
		//Gizmos.DrawRay(transform.position, 
	}
}
