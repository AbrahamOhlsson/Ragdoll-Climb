using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private Image redWins;
    [SerializeField] private Image blueWins;
    [SerializeField] private Image greenWins;
    [SerializeField] private Image yellowWins;

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

            if (playerNr == 1)
            {
                redWins.enabled = true;
                redWins.color = playerInfo.color;
            }
            else if (playerNr == 2)
            {
                blueWins.enabled = true;
                blueWins.color = playerInfo.color;
            }
            else if (playerNr == 3)
            {
                greenWins.enabled = true;
                greenWins.color = playerInfo.color;
            }
            else if (playerNr == 4)
            {
                yellowWins.enabled = true;
                yellowWins.color = playerInfo.color;
            }
            
            FindObjectOfType<musicAndSoundManager>().PlaySound("applaus");

            gameOver = true;
            

            rematchCanvas.SetActive(true);
            doItButton.Select();
        }
    }
}
