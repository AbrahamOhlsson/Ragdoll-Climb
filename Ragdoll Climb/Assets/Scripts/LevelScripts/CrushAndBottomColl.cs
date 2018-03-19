using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushAndBottomColl : MonoBehaviour
{
    //[HideInInspector] public bool canBeCrushed = false;
    [HideInInspector] public bool touchingCrush = false;
    [HideInInspector] public bool touchingBottom = false;
    //[HideInInspector] public float lowestCrushPoint = 10000000f;
    //[HideInInspector] public float highestBottomObjPoint = -100000000f;
    [HideInInspector] public float impulse = 0f;

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Crush")
        {
            //lowestCrushPoint = 10000000f;
            touchingCrush = true;
        }

        if (collision.transform.tag == "BottomObj")
        {
            //highestBottomObjPoint = -10000000f;
            touchingBottom = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Crush")
        {
            touchingCrush = false;
            //canBeCrushed = false;

            if (!touchingBottom)
                impulse = 0;
        }

        if (collision.transform.tag == "BottomObj")
        {
            touchingBottom = false;
            //canBeCrushed = false;

            if (!touchingCrush)
                impulse = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //if (collision.transform.tag == "Crush")
        //{
        //        print(collision.impulse);

        //    for (int i = 0; i < collision.contacts.Length; i++)
        //    {
        //        Vector3 contactPoint = collision.contacts[i].point;
        //        Transform collTrans = collision.transform;

        //        if (lowestCrushPoint > contactPoint.y)
        //            lowestCrushPoint = contactPoint.y;

        //        if (contactPoint.x > collTrans.position.x + collTrans.localScale.x / 2 || contactPoint.x < collTrans.position.x - collTrans.localScale.x / 2)
        //            canBeCrushed = false;
        //        else
        //            canBeCrushed = true;

        //        //print("Side = " + (collTrans.position.x + collTrans.localScale.x / 2) + ",  Contact = " + contactPoint.x);
        //    }
        //}

        //if (collision.transform.tag == "BottomObj")
        //{
        //    for (int i = 0; i < collision.contacts.Length; i++)
        //    {
        //        if (highestBottomObjPoint < collision.contacts[i].point.y)
        //            highestBottomObjPoint = collision.contacts[i].point.y;
        //    }
        //}

        // If a crushing object or the bottom object is being touched
        if (collision.transform.tag == "Crush" || collision.transform.tag == "BottomObj")
        {
            impulse = collision.impulse.magnitude;
        }
    }
}
