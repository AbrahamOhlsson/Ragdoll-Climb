using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreArmCollision : MonoBehaviour
{
    Collider[] allCollidersArray;
    List<Collider> allCollidersList = new List<Collider>();

    List<Collider> allArmColl = new List<Collider>();
    List<Collider> leftArmColl = new List<Collider>();
    List<Collider> rightArmColl = new List<Collider>();


	void Start ()
    {
        allCollidersArray = GetComponentsInChildren<Collider>();

        for (int i = 0; i < allCollidersArray.Length; i++)
        {
            allCollidersList.Add(allCollidersArray[i]);
        }

        for (int i = allCollidersList.Count - 1; i >= 0; i--)
        {
            if (allCollidersList[i].gameObject.layer != gameObject.layer)
            {
                allArmColl.Add(allCollidersList[i]);
                allCollidersList.RemoveAt(i);
            }
        }

        for (int i = 0; i < allArmColl.Count; i++)
        {
            for (int j = 0; j < allCollidersList.Count; j++)
            {
                Physics.IgnoreCollision(allArmColl[i], allCollidersList[j]);
            }

            for (int j = 0; j < allArmColl.Count; j++)
            {
                Physics.IgnoreCollision(allArmColl[i], allArmColl[j]);
            }
        }
	}
}
