using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private Image p1Wins;
    [SerializeField] private Image p2Wins;
    [SerializeField] private Image p3Wins;
    [SerializeField] private Image p4Wins;



    [SerializeField] private GameObject rematchCanvas;
    [SerializeField] private Button doItButton;
    //[SerializeField] private Button showingMenuTitle;
    //[SerializeField] private Button showingMenuPlay;
    //[SerializeField] private Button showingMenuQuit;

    
    GameObject test;


    bool gameOver = false;

    private string winnerText;



    private void Awake()
    {
        //   parent need to have inactiveOnStart script if p1Win is inactive

        test = GameObject.Find("WinnerImage_p1");
        p1Wins = test.GetComponent<Image>(); ;


        test = GameObject.Find("WinnerImage_p2");
        p2Wins = test.GetComponent<Image>(); ;


        test = GameObject.Find("WinnerImage_p3");
        p3Wins = test.GetComponent<Image>(); ;


        test = GameObject.Find("WinnerImage_p4");
        p4Wins = test.GetComponent<Image>(); ;

        if (test == null) print("fel!!!");


        rematchCanvas = GameObject.Find("CanvasShowWinnerAndMenu");
        doItButton = GameObject.Find("DoItButton").GetComponent<Button>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !gameOver)
        {
            PlayerInfo playerInfo = other.transform.root.GetComponent<PlayerInfo>();

            int playerNr = playerInfo.playerNr;

            if (playerNr == 1)
            {
                p1Wins.enabled = true;
                p1Wins.color = playerInfo.color;
            }
            else if (playerNr == 2)
            {
                p2Wins.enabled = true;
                p2Wins.color = playerInfo.color;
            }
            else if (playerNr == 3)
            {
                p3Wins.enabled = true;
                p3Wins.color = playerInfo.color;
            }
            else if (playerNr == 4)
            {
                p4Wins.enabled = true;
                p4Wins.color = playerInfo.color;
            }
            
            FindObjectOfType<musicAndSoundManager>().PlaySound("applaus");

            gameOver = true;
            

            rematchCanvas.SetActive(true);
            doItButton.Select();
        }
    }
}
