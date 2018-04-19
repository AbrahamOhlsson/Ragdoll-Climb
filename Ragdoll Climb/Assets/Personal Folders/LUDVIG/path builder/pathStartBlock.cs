using System.Collections;
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
    void Start()
    {     /// Awake om man vill spawna dom dir

        maxBlock = FindObjectOfType<pathController>().maxBlock;
        currentBlock = FindObjectOfType<pathController>().currentBlock;
        startBoxZpos = FindObjectOfType<pathController>().startBox.transform.position.z;



        if (currentBlock < maxBlock)
        {
            FindObjectOfType<pathController>().currentBlock++;

            int listsize = FindObjectOfType<pathController>().blockList.Count;
            int randomNum = Random.Range(0, listsize);
            GameObject randomObj = FindObjectOfType<pathController>().blockList[randomNum];

            if (randomObj == null)
            {
                while (randomObj == null)
                {
                    randomNum = Random.Range(0, listsize);
                    randomObj = FindObjectOfType<pathController>().blockList[randomNum];
                }
            }

            //if (GameObject.Find(FindObjectOfType<pathController>().bloakList[randomNum].name) != null)
            //{

            //    while (GameObject.Find(FindObjectOfType<pathController>().bloakList[randomNum].name) != null)
            //    {
            //        randomNum = Random.Range(0, listsize);
            //        randomObj = FindObjectOfType<pathController>().bloakList[randomNum];
            //        print("nu finns ett obj 1");
            //    }

            //}

            FindObjectOfType<pathController>().blockList.RemoveAt(randomNum);
            Instantiate(randomObj, new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation);

        }

       
    }
}


