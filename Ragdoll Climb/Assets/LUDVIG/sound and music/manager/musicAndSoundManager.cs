using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
 using UnityEngine;



public class musicAndSoundManager : MonoBehaviour {

    public Sound[] sounds;
    public Sound[] feedbackSounds;

    // Use this for initialization
    void Awake () {
		foreach (Sound i in sounds)
        {
            i.source = gameObject.AddComponent<AudioSource>();
            i.source.clip = i.clip;

            i.source.volume = i.volume;
            i.source.pitch = i.pitch;

        }

	}

    void Start()
    {
        PlaySound("music");
       
    }


    public void PlaySound(string name)
    {
        Sound s= null;

        foreach (Sound i in sounds)
        {
            if(i.name == name)
            {
                s = i;
                s.source.Play();
            }
        }

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found.");

        }
    }

    

}
