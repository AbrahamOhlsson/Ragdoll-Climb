using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingSurface : MonoBehaviour
{
    [SerializeField] float breakTime = 5f;
    [SerializeField] float pushOutForce = 100f;

    bool isBreaking = false;

    List<CheckGrip> grabbingHands = new List<CheckGrip>();

    BreakingPart[] parts;

    soundManager soundManager;

	
	public void AddGrabbingHand(CheckGrip hand)
    {
        grabbingHands.Add(hand);

        if (!isBreaking)
            StartCoroutine(BreakApart());

        isBreaking = true;

        parts = GetComponentsInChildren<BreakingPart>();

        //soundManager = GameObject.Find("music and sound").GetComponent<soundManager>();
    }


    public void RemoveGrabbingHand(CheckGrip hand)
    {
        grabbingHands.Remove(hand);
    }


    IEnumerator BreakApart()
    {
        CheckGrip[] handArray = new CheckGrip[grabbingHands.Count];
        handArray = grabbingHands.ToArray();

        foreach (CheckGrip hand in handArray)
        {
            hand.transform.root.GetComponent<VibrationManager>().VibrateTimed(0.5f, 0.1f, 9);
        }

        //soundManager.PlaySoundRandPitch("crack");

        yield return new WaitForSeconds(breakTime / 2);

        foreach (CheckGrip hand in handArray)
        {
            hand.transform.root.GetComponent<VibrationManager>().VibrateTimed(0.5f, 0.1f, 9);
        }

        //soundManager.PlaySoundRandPitch("crack");

        yield return new WaitForSeconds(breakTime / 2);

        foreach (BreakingPart part in parts)
        {
            part.Break(pushOutForce);

            foreach (CheckGrip hand in handArray)
            {
                hand.controller.ReleaseGrip(hand.leftHand, false);
            }
        }

        //soundManager.PlaySoundRandPitch("break");

        yield return null;
    }
}
