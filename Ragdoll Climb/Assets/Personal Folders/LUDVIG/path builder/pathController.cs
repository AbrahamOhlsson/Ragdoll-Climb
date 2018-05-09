using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathController : MonoBehaviour {

    [HideInInspector] public int currentBlock = 1;
    [HideInInspector] public List<GameObject> blockList;
    [HideInInspector] public int maxBlock;
    

    [SerializeField] List<GameObject> easyBloakList;
    [SerializeField] List<GameObject> normBloakList;
    [SerializeField] List<GameObject> hardBloakList;
   
    [Space]
    public GameObject endBox;
    [Space]
    public GameObject startBox;

    // dificulty
    public Singleton.Difficulties Difficulties;
    public Singleton.Lengths Lengths;


    // måste hända innan alla andra build script 
    private void Awake()
    {
        Difficulties =  FindObjectOfType<Singleton>().levelDifficulty;
        Lengths = FindObjectOfType<Singleton>().levelLength;


        // LENGTH ####################################
        if (Lengths == Singleton.Lengths.Short)
        {
            maxBlock = 2; // ska vara 2 
        }


        else if (Lengths == Singleton.Lengths.Medium)
        {
            maxBlock = 3;
        }


        else if (Lengths == Singleton.Lengths.Long)
        {
            maxBlock = 4;
        }


        else if (Lengths == Singleton.Lengths.Humongous)
        {
            maxBlock = 5;
        }


        else if (Lengths == Singleton.Lengths.Gigantic)
        {
            maxBlock = 6;
        }


        // Difficultie  ################################
        if (Difficulties == Singleton.Difficulties.VeryEasy)
        {
            blockList = easyBloakList;
        }


        else if (Difficulties == Singleton.Difficulties.Easy)
        {
            blockList = easyBloakList;

            foreach (GameObject i in normBloakList)
            {
                blockList.Add(i);

            }
        }


        else if (Difficulties == Singleton.Difficulties.Medium)
        {
            blockList = normBloakList;
        }


        else if (Difficulties == Singleton.Difficulties.Hard)
        {
            blockList = hardBloakList;

            foreach (GameObject i in normBloakList)
            {
                blockList.Add(i);

            }
        }


        else if (Difficulties == Singleton.Difficulties.VeryHard)
        {
            blockList = hardBloakList;
        }


        else if (Difficulties == Singleton.Difficulties.Mix)
        {
            blockList = easyBloakList;

            foreach(GameObject i in normBloakList)
            {
                blockList.Add(i);

            }

            foreach (GameObject i in hardBloakList)
            {
                blockList.Add(i);

            }
        }


    }


}
