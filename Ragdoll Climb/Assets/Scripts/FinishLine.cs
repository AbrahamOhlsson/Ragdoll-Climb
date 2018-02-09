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

    [SerializeField] private GameObject menuRematch;
    //[SerializeField] private Button showingMenuTitle;
    //[SerializeField] private Button showingMenuPlay;
    //[SerializeField] private Button showingMenuQuit;

    bool gameOver = false;

    private string winnerText;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !gameOver)
        {
            int playerNr = other.transform.root.GetComponent<PlayerController>().playerNr;

            if (playerNr == 1)
            {
                redWins.enabled = true;
            }
            else if (playerNr == 2)
            {
                blueWins.enabled = true;
            }
            else if (playerNr == 3)
            {
                greenWins.enabled = true;
            }
            else if (playerNr == 4)
            {
                yellowWins.enabled = true;
            }

            gameOver = true;

            menuRematch.SetActive(true);
            menuRematch.transform.GetChild(3).GetComponent<Button>().Select();
        }
    }
}
