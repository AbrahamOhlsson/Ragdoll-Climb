using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Collections;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    soundManager soundManager;

    
	void Start ()
    {
        soundManager = FindObjectOfType<soundManager>();

        if (GetComponent<Button>())
            GetComponent<Button>().onClick.AddListener(ClickSound);
	}
	

	void Update ()
    {
		
	}


    public void OnSelect(BaseEventData eventData)
    {
        soundManager.PlaySound("ButtonNavigation");
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        soundManager.PlaySound("ButtonNavigation");
    }


    public void ClickSound()
    {
        soundManager.PlaySound("ButtonClick");
    }
}
