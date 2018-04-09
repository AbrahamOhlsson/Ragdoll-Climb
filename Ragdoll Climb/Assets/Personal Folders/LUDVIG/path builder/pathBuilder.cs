using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathBuilder : MonoBehaviour {

    int maxBlock;
    int currentBlock;
    public float startBoxZpos;
    enum Direction {Right, Middle, Left };
    [SerializeField]
    Direction StartPos;
    [SerializeField]
    Direction EndPos;

    

    // Use this for initialization
    void Awake  () {     /// Awake om man vill spawna dom dir

        maxBlock = FindObjectOfType<pathController>().maxBlock;
        currentBlock = FindObjectOfType<pathController>().currentBlock;

        startBoxZpos = FindObjectOfType<pathController>().startBox.transform.position.z;
       


        // move block
        if (StartPos == Direction.Middle)
        {
            transform.position = new Vector3(transform.position.x ,transform.position.y + (transform.localScale.y / 2), transform.position.z + (transform.localScale.z / 2) - (FindObjectOfType<pathController>().startBox.transform.localScale.z / 2));
        }

        else if (StartPos == Direction.Right)
        {
            transform.position = new Vector3(((transform.position.x - (transform.localScale.x / 2)) + 5), (transform.position.y + (transform.localScale.y / 2)), transform.position.z + (transform.localScale.z / 2) - (FindObjectOfType<pathController>().startBox.transform.localScale.z / 2));
        }

        else if (StartPos == Direction.Left)
        {
            transform.position = new Vector3(((transform.position.x + (transform.localScale.x / 2)) - 5), (transform.position.y + (transform.localScale.y / 2)), transform.position.z + (transform.localScale.z / 2) - (FindObjectOfType<pathController>().startBox.transform.localScale.z / 2));
        }



        //###################################################################################################################################
        //   spawn block from the middle
        if (EndPos == Direction.Middle)
        {
            // Spawn next block 
            if (currentBlock < maxBlock)
            {
                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Count;
                int randomNum = Random.Range(0, listsize);
                GameObject randomObj = FindObjectOfType<pathController>().bloakList[randomNum];

                if (randomObj == null)
                {
                    while (randomObj == null)
                    {
                        randomNum = Random.Range(0, listsize);
                        randomObj = FindObjectOfType<pathController>().bloakList[randomNum];
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

                FindObjectOfType<pathController>().bloakList.RemoveAt(randomNum);
                Instantiate(randomObj, new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation);

            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Count;


                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); 

            }
        }

        //   spawn block from the Right
        else if (EndPos == Direction.Left)
        {
            // Spawn next block 
            if (currentBlock < maxBlock)
            {
                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Count;
                int randomNum = Random.Range(0, listsize);
                GameObject randomObj = FindObjectOfType<pathController>().bloakList[randomNum];

                if (randomObj == null)
                {
                    while (randomObj == null)
                    {
                        randomNum = Random.Range(0, listsize);
                        randomObj = FindObjectOfType<pathController>().bloakList[randomNum];
                    }
                }

                //if (GameObject.Find(FindObjectOfType<pathController>().bloakList[randomNum].name) != null)
                //{

                //    while (GameObject.Find(FindObjectOfType<pathController>().bloakList[randomNum].name) != null)
                //    {
                //        randomNum = Random.Range(0, listsize);
                //        randomObj = FindObjectOfType<pathController>().bloakList[randomNum];
                //        print("nu finns ett obj 2");
                //    }

                //}

                FindObjectOfType<pathController>().bloakList.RemoveAt(randomNum);
                Instantiate(randomObj, new Vector3(((transform.position.x - (transform.localScale.x / 2)) + 5), transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); 

            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Count;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(((transform.position.x - (transform.localScale.x / 2)) + 5), transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); 

            }
        }

        //   spawn block from the Left
        else if (EndPos == Direction.Right)
        {
            // Spawn next block 
            if (currentBlock < maxBlock)
            {
                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Count;
                int randomNum = Random.Range(0, listsize);
                GameObject randomObj = FindObjectOfType<pathController>().bloakList[randomNum];

                if (randomObj == null)
                {
                    while (randomObj == null)
                    {
                        randomNum = Random.Range(0, listsize);
                        randomObj = FindObjectOfType<pathController>().bloakList[randomNum];
                    }
                }

                //if (GameObject.Find(FindObjectOfType<pathController>().bloakList[randomNum].name) !=null)
                //{

                //    while (GameObject.Find(FindObjectOfType<pathController>().bloakList[randomNum].name) != null)
                //    {
                //        randomNum = Random.Range(0, listsize);
                //        randomObj = FindObjectOfType<pathController>().bloakList[randomNum];
                //        print("nu finns ett obj 3");
                //    }

                //}

                FindObjectOfType<pathController>().bloakList.RemoveAt(randomNum);
                Instantiate(randomObj, new Vector3(((transform.position.x + (transform.localScale.x / 2)) - 5), transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation);

            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Count;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(((transform.position.x + (transform.localScale.x / 2)) - 5), transform.position.y + (transform.localScale.y / 2), startBoxZpos), transform.rotation); 


            }
        }


    }
	
}
