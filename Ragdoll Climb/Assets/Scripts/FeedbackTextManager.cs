using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackTextManager : MonoBehaviour
{
    List<FeedbackText> texts = new List<FeedbackText>();
    List<Vector2> goalPositions = new List<Vector2>();


	void Start ()
    {
        // Gets all FeedbackText components in children and puts them in an array
        FeedbackText[] tempTexts = GetComponentsInChildren<FeedbackText>();

        // Adds all the components in a list instead
        for (int i = 0; i < tempTexts.Length; i++)
        {
            texts.Add(tempTexts[i]);
        }

        // Gets all the objects of the components initial positions to be goal positions for them
        for (int i = 0; i < texts.Count; i++)
        {
            goalPositions.Add(texts[i].GetComponent<RectTransform>().localPosition);
        }
    }
	
    
    public void PushList(FeedbackText text)
    {
        // Puts the given FeedbackText to be first in the list
        texts.Remove(text);
        texts.Insert(0, text);

        // Updates all the FeedbackTexts goal positions
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].goalPos = goalPositions[i];
        }
    }
}
