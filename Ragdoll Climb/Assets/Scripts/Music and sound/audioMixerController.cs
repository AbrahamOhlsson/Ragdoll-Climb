using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class audioMixerController : MonoBehaviour {

    public  AudioMixer myAudioMixer;
    public AudioMixerGroup SFX;

    public AudioSource sourceTest;
	// Use this for initialization
	void Start () {
        //myAudioMixer.
       // sourceTest.outputAudioMixerGroup = SFX;

        //SFX = myAudioMixer.FindMatchingGroups("Music");
       // SFX.audioMixer.SetFloat("SFXVolume", -5f);
	}
	
	// Update is called once per frame
	void Update () {

        SFX.audioMixer.SetFloat("SFXVolume", -5f);
        //  print(getMixerValue(SFX.audioMixer, "SFXVolume"));
	}





    public float getMixerValue(AudioMixer testMixer, string nameOfValue)
    {
        float value;
        bool result = testMixer.GetFloat(nameOfValue, out value);
        if (result)
        {

            print("in get mixer value (result == true)");
            return value;
        }
        else
        {
            print("error in get mixer value (result == false)");
            return 0f;
        }
    }

}
