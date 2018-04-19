using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathController : MonoBehaviour {

    public int maxBlock;
    [Space]
    [HideInInspector]
    public int currentBlock = 1;

  

    public List<GameObject> blockList;
    [Space]
    [Header("gör inget ATM")]
    [SerializeField] List<GameObject> EZbloakList;
    [SerializeField] List<GameObject> NORMbloakList;
    [SerializeField] List<GameObject> HARDbloakList;
   
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
            blockList = EZbloakList;
        }

        else if (Difficulties == PlayerInfoSingleton.Difficulties.Easy)
        {
            blockList = EZbloakList;
        }

        else if (Difficulties == PlayerInfoSingleton.Difficulties.Medium)
        {
            blockList = NORMbloakList;
        }

        else if (Difficulties == PlayerInfoSingleton.Difficulties.Hard)
        {
            blockList = HARDbloakList;
        }

        else if (Difficulties == PlayerInfoSingleton.Difficulties.VeryHard)
        {
            blockList = HARDbloakList;
        }

        else if (Difficulties == PlayerInfoSingleton.Difficulties.Mix)
        {
            blockList = EZbloakList;

            foreach(GameObject i in NORMbloakList)
            {
                blockList.Add(i);

            }

            foreach (GameObject i in HARDbloakList)
            {
                blockList.Add(i);

            }
        }



    }

    

}
