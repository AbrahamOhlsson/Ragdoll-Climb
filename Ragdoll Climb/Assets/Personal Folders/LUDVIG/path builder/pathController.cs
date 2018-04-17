using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathController : MonoBehaviour {

    public int maxBlock;
    [Space]
    [HideInInspector]
    public int currentBlock = 1;

    enum darlingTest { ez, norm, HARD };
    [Space]
    [Header("gör inget ATM")]
    [SerializeField] darlingTest difficulty;

    public List<GameObject> bloakList;
    [Space]
    [Header("gör inget ATM")]
    [SerializeField] List<GameObject> EZbloakList;
    [SerializeField] List<GameObject> NORMbloakList;
    [SerializeField] List<GameObject> HARDbloakList;
   
    [Space]
    public GameObject endBox;
    [Space]
    public GameObject startBox;


    // måste hända innan alla andra build script 
    private void Awake()
    {
        if (difficulty == darlingTest.ez)
        {
            //bloakList = EZbloakList;

        }

        if (difficulty == darlingTest.norm)
        {
           //bloakList = NORMbloakList;

        }

        if (difficulty == darlingTest.HARD)
        {
            //bloakList = HARDbloakList;

        }

    }


    // Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame

}
