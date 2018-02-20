using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    [SerializeField] Text uiText;

    Queue textObjs = new Queue();

    
    void Start ()
    {
	}


    void Update ()
    {
		foreach (TextObj obj in textObjs)
        {
            obj.UpdateTimer();

            if (obj.displayTimeReached)
            {
                textObjs.Dequeue();

                UpdateText();

                break;
            }
        }
	}


    public void AddText(string text)
    {
        TextObj obj = new TextObj(text);

        textObjs.Enqueue(obj);

        UpdateText();
    }


    void UpdateText()
    {
        string wholeText = "";

        foreach (TextObj obj in textObjs)
        {
            wholeText = wholeText + "\n" + obj.text;
        }

        uiText.text = wholeText;
    }


    //public void AddText(string text, float displayTime)
    //{
    //    TextObj obj = new TextObj(text, displayTime);
    //}
}


class TextObj
{
    public bool displayTimeReached = false;
    public string text = "";

    float timer = 0f;
    float displayTime = 3f;


    public TextObj()
    {
    }


    public TextObj(string _text)
    {
        text = _text;
    }


    //public TextObj(string _text, float _displayTime)
    //{
    //    text = _text;
    //    displayTime = _displayTime;
    //}


    public void UpdateTimer()
    {
        timer += Time.deltaTime;

        if (timer >= displayTime)
            displayTimeReached = true;
    }
}
