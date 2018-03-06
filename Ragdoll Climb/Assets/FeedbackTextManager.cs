using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackTextManager : MonoBehaviour
{
    List<FeedbackText> texts = new List<FeedbackText>();
    List<Vector2> goalPositions = new List<Vector2>();


	void Start ()
    {
        FeedbackText[] tempTexts = GetComponentsInChildren<FeedbackText>();

        for (int i = 0; i < tempTexts.Length; i++)
        {
            texts.Add(tempTexts[i]);
        }

        for (int i = 0; i < texts.Count; i++)
        {
            goalPositions.Add(texts[i].GetComponent<RectTransform>().localPosition);
        }
    }
	

	void Update ()
    {
		
	}


    public void PushList(FeedbackText text)
    {
        texts.Remove(text);
        texts.Insert(0, text);

        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].goalPos = goalPositions[i];
        }
    }
}
