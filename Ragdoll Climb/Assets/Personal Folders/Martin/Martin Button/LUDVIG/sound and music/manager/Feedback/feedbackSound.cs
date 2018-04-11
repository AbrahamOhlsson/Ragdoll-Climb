using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]

public class feedbackSound
{

    public string name;

    public enum player { Red, Blue, Green, Yellow };
    [SerializeField]
    public player PlayerColor;

    //[Range(1, 4)] 
    //public int player=1;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    
}
