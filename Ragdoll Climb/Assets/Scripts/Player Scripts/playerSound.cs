using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class playerSound : MonoBehaviour {

    public AudioMixerGroup MyMixerGroup;
    [Space]
    public Sound[] sounds;


    // Use this for initialization
    void Awake()
    {
        foreach (Sound i in sounds)
        {
            i.source = gameObject.AddComponent<AudioSource>();
            i.source.clip = i.clip;

            i.source.volume = i.volume;
            i.source.pitch = i.pitch;
            i.source.playOnAwake = false;
            i.source.outputAudioMixerGroup = MyMixerGroup;
        }

    }

    //void Start()
    //{


    //}


    public void PlaySound(string name)
    {
        Sound s = null;

        foreach (Sound i in sounds)
        {
            if (i.name == name)
            {
                s = i;
                s.pitch = 1;
                s.source.Play();
            }
        }

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " in " + name + " not found.");

        }
    }


    public void PlaySoundRandPitch(string name)
    {
        Sound s = null;

        foreach (Sound i in sounds)
        {
            if (i.name == name)
            {
                s = i;
                s.pitch = Random.Range(0.9f, 1.1f);
                s.source.Play();
            }
        }

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " in " + name + " not found.");

        }
    }

}
