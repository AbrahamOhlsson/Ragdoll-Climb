using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushCheck : MonoBehaviour
{
    [SerializeField] float minDist = 1.5f;
    [SerializeField] float crushImpulse = 100f;

    CrushAndBottomColl[] colls;


    void Start ()
    {
        colls = GetComponentsInChildren<CrushAndBottomColl>();
    }


    void FixedUpdate ()
    {
        // Every collider is being checked with eachother
		for (int i = 0; i < colls.Length; i++)
        {
            for (int j = 0; j < colls.Length; j++)
            {
                //if (colls[i].touchingCrush && colls[j].touchingBottom && colls[i].canBeCrushed
                //    && colls[i].lowestCrushPoint > GetComponent<PlayerInfo>().rootObj.transform.position.y)
                //{
                //    if (colls[i].lowestCrushPoint - colls[j].highestBottomObjPoint <= minDist)
                //    {
                //        print("DEATH");
                //        GetComponent<DeathManager>().Death();
                //        break;
                //    }
                //}

                // If a collider is touching a crushing object and another is touching the bottom
                if (colls[i].touchingCrush && colls[j].touchingBottom)
                {
                    //print("Crush: " + colls[i].impulse + ",     Bottom: " + colls[j].impulse);

                    // If either of the colliders has a big enough impulse
                    if (colls[i].impulse >= crushImpulse || colls[j].impulse >= crushImpulse)
                    {
                        // Kills the player
                        print("DEATH");
                        GetComponent<DeathManager>().Death();
                        break;
                    }
                }
            }
        }
	}
}
