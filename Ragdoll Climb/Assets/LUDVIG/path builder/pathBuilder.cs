using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathBuilder : MonoBehaviour {

    int maxBlock;
    int currentBlock;
    enum Direction {LeftToRight, Middle, RightToLeft };
    [SerializeField]
    Direction direction;


    // Use this for initialization
    void Start () {

        maxBlock = FindObjectOfType<pathController>().maxBlock;
        currentBlock = FindObjectOfType<pathController>().currentBlock;


        // move block
        if (direction == Direction.Middle)
        {
            transform.position = Vector3.up * (transform.position.y + (transform.localScale.y / 2));
            // print(Vector3.up * (transform.position.y + (transform.localScale.y / 2)));
            //print(FindObjectOfType<pathController>().maxBlock);
            print(currentBlock);
        }

        else if (direction == Direction.LeftToRight)
        {
            //transform.position = Vector3.up * (transform.position.y + (transform.localScale.y / 2));
            //transform.position = Vector3.right * (transform.position.x + ((transform.localScale.x / 2) -10));
            transform.position = new Vector3((transform.position.x + ((transform.localScale.x / 2) - 15)), (transform.position.y + (transform.localScale.y / 2)), transform.position.z);


        }

        else if (direction == Direction.RightToLeft)
        {
            transform.position = new Vector3((transform.position.x + ((transform.localScale.x / 2) + 15)), (transform.position.y + (transform.localScale.y / 2)), transform.position.z);

        }




        //   spawn block from the middle
        else if (direction == Direction.Middle)
        {
            // Spawn next block 
            if (currentBlock < maxBlock)
            {
                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().bloakList[Random.Range(0, listsize - 1)], new Vector3(0, transform.position.y + (transform.localScale.y / 2), 0), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));
                print("inne i curentblock sexypoo");

            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(0, transform.position.y + (transform.localScale.y / 2), 0), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));
                print("inne i endbox");

            }
        }

        //   spawn block from the Right
        else if (direction == Direction.LeftToRight)
        {
            // Spawn next block 
            if (currentBlock < maxBlock)
            {
                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().bloakList[Random.Range(0, listsize - 1)], new Vector3(0, transform.position.y + (transform.localScale.y / 2), 0), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));
                print("inne i curentblock sexypoo");

            }

            else if (currentBlock == maxBlock)
            {

                FindObjectOfType<pathController>().currentBlock++;

                int listsize = FindObjectOfType<pathController>().bloakList.Length;

                Instantiate(FindObjectOfType<pathController>().endBox, new Vector3(0, transform.position.y + (transform.localScale.y / 2), 0), transform.rotation); // Quaternion.identity);// new Vector3(0, transform.position.y+ (transform.localScale.y / 2),0));
                print("inne i endbox");

            }
        }






    }
	
	// Update is called once per frame
	void Update () {
		


	}
}
