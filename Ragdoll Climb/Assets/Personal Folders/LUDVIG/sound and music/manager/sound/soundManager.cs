using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;



public class soundManager : MonoBehaviour
{

    public Sound[] sounds;
    

    // Use this for initialization
    void Awake()
    {
        foreach (Sound i in sounds)
        {
            i.source = gameObject.AddComponent<AudioSource>();
            i.source.playOnAwake = false;
            i.source.clip = i.clip;
            i.source.volume = i.volume;
            i.source.pitch = i.pitch;

        }

    }

    void Start()
    {
        

    }


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
            Debug.LogWarning("Sound:" + name + " not found.");

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
                s.pitch = Random.Range(0.9f,1.1f);
                s.source.Play();
            }
        }

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found.");

        }
    }


}
