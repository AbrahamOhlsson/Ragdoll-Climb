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
    public PlayerInfoSingleton.Difficulties Difficulties;
    public PlayerInfoSingleton.Lengths Lengths;


    // måste hända innan alla andra build script 
    private void Awake()
    {
        Difficulties =  FindObjectOfType<PlayerInfoSingleton>().levelDifficulty;
        Lengths = FindObjectOfType<PlayerInfoSingleton>().levelLength;


        // LENGTH ####################################
        if (Lengths == PlayerInfoSingleton.Lengths.Short)
        {
            maxBlock = 2; // ska vara 2 
        }


        else if (Lengths == PlayerInfoSingleton.Lengths.Medium)
        {
            maxBlock = 3;
        }


        else if (Lengths == PlayerInfoSingleton.Lengths.Long)
        {
            maxBlock = 4;
        }


        else if (Lengths == PlayerInfoSingleton.Lengths.Humongous)
        {
            maxBlock = 5;
        }


        else if (Lengths == PlayerInfoSingleton.Lengths.Gigantic)
        {
            maxBlock = 6;
        }


        // Difficultie  ################################
        if (Difficulties == PlayerInfoSingleton.Difficulties.VeryEasy)
        {
            blockList = easyBloakList;
        }


        else if (Difficulties == PlayerInfoSingleton.Difficulties.Easy)
        {
            blockList = easyBloakList;

            foreach (GameObject i in normBloakList)
            {
                blockList.Add(i);

            }
        }


        else if (Difficulties == PlayerInfoSingleton.Difficulties.Medium)
        {
            blockList = normBloakList;
        }


        else if (Difficulties == PlayerInfoSingleton.Difficulties.Hard)
        {
            blockList = hardBloakList;

            foreach (GameObject i in normBloakList)
            {
                blockList.Add(i);

            }
        }


        else if (Difficulties == PlayerInfoSingleton.Difficulties.VeryHard)
        {
            blockList = hardBloakList;
        }


        else if (Difficulties == PlayerInfoSingleton.Difficulties.Mix)
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
