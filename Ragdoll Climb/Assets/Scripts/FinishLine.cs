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

    bool gameOver = false;

    private string winnerText;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !gameOver)
        {
            PlayerInfo playerInfo = other.transform.root.GetComponent<PlayerInfo>();

            int playerNr = playerInfo.playerNr;

            print(playerNr);

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
