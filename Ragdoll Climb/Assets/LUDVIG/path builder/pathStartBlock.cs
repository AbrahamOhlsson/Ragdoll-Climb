﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathStartBlock : MonoBehaviour {

    int maxBlock;
    int currentBlock;
    float startBoxZpos;
    enum Direction { Right, Middle, Left };
    [SerializeField]
    Direction StartPos;
    [SerializeField]
    Direction EndPos;


    // Use this for initialization
    void Awake()
    {     /// Awake om man vill spawna dom dir

        maxBlock = FindObjectOfType<pathController>().maxBlock;
        currentBlock = FindObjectOfType<pathController>().currentBlock;
        startBoxZpos = FindObjectOfType<pathController>().startBox.transform.position.z;
        startBoxZpos = startBoxZpos + (FindObjectOfType<pathController>().startBox.transform.localScale.z / 2);

        //###################################################################################################################################
        //   spawn block from the middle
        if (EndPos == Direction.Middle)
        {
            // Spawn next block 
            if (currentBlock < maxBlock)
            {
                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().bloakList[Random.Range(0, listsize)], new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));


            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));


            }
        }

        //   spawn block from the Right
        else if (EndPos == Direction.Left)
        {
            // Spawn next block 
            if (currentBlock < maxBlock)
            {
                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().bloakList[Random.Range(0, listsize)], new Vector3(((transform.position.x - (transform.localScale.x / 2)) + 5), transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));


            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(((transform.position.x - (transform.localScale.x / 2)) + 5), transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));


            }
        }

        //   spawn block from the Left
        else if (EndPos == Direction.Right)
        {
            // Spawn next block 
            if (currentBlock < maxBlock)
            {
                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().bloakList[Random.Range(0, listsize)], new Vector3(((transform.position.x + (transform.localScale.x / 2)) - 5), transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));


            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(((transform.position.x + (transform.localScale.x / 2)) - 5), transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));


            }
        }




    }

}
