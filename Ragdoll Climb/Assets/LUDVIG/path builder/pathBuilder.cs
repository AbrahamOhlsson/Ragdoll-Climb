using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathBuilder : MonoBehaviour {

    int maxBlock;
    int currentBlock;
    enum Direction {Right, Middle, Left };
    [SerializeField]
    Direction StartPos;
    [SerializeField]
    Direction EndPos;
    

    // Use this for initialization
    void Start  () {

        maxBlock = FindObjectOfType<pathController>().maxBlock;
        currentBlock = FindObjectOfType<pathController>().currentBlock;


        // move block
        if (StartPos == Direction.Middle)
        {
            transform.position = new Vector3(transform.position.x ,transform.position.y + (transform.localScale.y / 2), transform.position.z);
            //print(currentBlock);
        }

        else if (StartPos == Direction.Right)
        {
                     transform.position = new Vector3(((transform.position.x - (transform.localScale.x / 2)) + 5), (transform.position.y + (transform.localScale.y / 2)), transform.position.z);

        }

        else if (StartPos == Direction.Left)
        {
            transform.position = new Vector3(((transform.position.x + (transform.localScale.x / 2)) - 5), (transform.position.y + (transform.localScale.y / 2)), transform.position.z);
        }



        //###################################################################################################################################
        //   spawn block from the middle
        if (EndPos == Direction.Middle)
        {
            // Spawn next block 
            if (currentBlock < maxBlock)
            {
                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().bloakList[Random.Range(0, listsize )], new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2), transform.position.z), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));
              

            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2), transform.position.z), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));
        

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

                Instantiate(FindObjectOfType<pathController>().bloakList[Random.Range(0, listsize )], new Vector3(((transform.position.x - (transform.localScale.x / 2)) + 5), transform.position.y + (transform.localScale.y / 2), 0), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));
              

            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(((transform.position.x - (transform.localScale.x / 2)) + 5), transform.position.y + (transform.localScale.y / 2), 0), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));
                

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

                Instantiate(FindObjectOfType<pathController>().bloakList[Random.Range(0, listsize )], new Vector3(((transform.position.x + (transform.localScale.x / 2)) - 5), transform.position.y + (transform.localScale.y / 2), 0), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));
                

            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(((transform.position.x + (transform.localScale.x / 2)) - 5), transform.position.y + (transform.localScale.y / 2), 0), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));


            }
        }




    }
	
}
