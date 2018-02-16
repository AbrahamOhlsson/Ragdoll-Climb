using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCountUI : MonoBehaviour
{
    [SerializeField] private Image customImageThree;
    [SerializeField] private Image customImageTwo;
    [SerializeField] private Image customImageOne;
    [SerializeField] private Image customImageClimb;

    [SerializeField] private MultiplayerManager multiplayerManager;

    
    void Start()
    {
        StartCoroutine(imageCountdown());
    }

    IEnumerator imageCountdown()
    {
        customImageThree.enabled = true;
        yield return new WaitForSeconds(1);
        customImageThree.enabled = false;
        customImageTwo.enabled = true;
        yield return new WaitForSeconds(1);
        customImageTwo.enabled = false;
        customImageOne.enabled = true;
        yield return new WaitForSeconds(1);
        customImageOne.enabled = false;
        customImageClimb.enabled = true;
        multiplayerManager.ActivatePlayers();
        yield return new WaitForSeconds(1);
        customImageClimb.enabled = false;

        Debug.Log("contdown");
        FindObjectOfType<musicAndSoundManager>().Play("startHorn"); 

        Destroy(this);
    }

}
