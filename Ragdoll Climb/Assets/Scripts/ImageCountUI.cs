using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCountUI : MonoBehaviour {


    [SerializeField] private Image customImageThree;
    [SerializeField] private Image customImageTwo;
    [SerializeField] private Image customImageOne;
    [SerializeField] private Image customImageClimb;




    void Start()
    {
        StartCoroutine(imageCountdown());
    }

    IEnumerator imageCountdown()
    {
        Debug.Log("Image");
        customImageThree.enabled = true;
        yield return new WaitForSeconds(1);
        customImageThree.enabled = false;
        Debug.Log("1");
        customImageTwo.enabled = true;
        yield return new WaitForSeconds(1);
        customImageTwo.enabled = false;
        Debug.Log("2");
        customImageOne.enabled = true;
        yield return new WaitForSeconds(1);
        customImageOne.enabled = false;
        Debug.Log("3");
        customImageClimb.enabled = true;
        yield return new WaitForSeconds(1);
        customImageClimb.enabled = false;
        Destroy(this);
    }

}
