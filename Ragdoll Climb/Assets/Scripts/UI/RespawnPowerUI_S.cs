using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnPowerUI_S : MonoBehaviour {

    public Animator animator;
    private Text textCountdown;

	void Start () {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        textCountdown = animator.GetComponent<Text>();


	}
	
}
