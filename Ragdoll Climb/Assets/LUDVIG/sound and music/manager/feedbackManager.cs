using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class feedbackManager : MonoBehaviour {

    
    public feedbackSound[] feedbackSounds;

    public float CdTime;
    // Use this for initialization
    void Awake()
    {
        foreach (feedbackSound i in feedbackSounds)
        {
            i.source = gameObject.AddComponent<AudioSource>();
            i.source.clip = i.clip;

            i.source.volume = i.volume;
            i.source.pitch = i.pitch;

        }

    }

    void Start()
    {
        CdTime = 0;
    }

    void Update()
    {
        if (CdTime <0){
            CdTime += 1 * Time.deltaTime;
        }
    }



    /// <summary>
    ///  "play a feedback sound "
    /// </summary>
    /// <param name="name"> name of the sound </param>
    /// <param name="player"> / Red = 1 / Blue = 2 / Green = 3 / Yellow = 4 / </param>
    public void PlaySound(string name, feedbackSound.player playerColler)
    {
        feedbackSound s = null;  //varför en ny? (se if (s == null)  ;)  )

        if (CdTime >= 0)
        {

            foreach (feedbackSound i in feedbackSounds)
            {
                if (i.name == name)
                {

                        s = i;

                    if (i.PlayerColor == playerColler)  // Red = 1 / Blue = 2 / Green = 3 / Yellow = 4 /
                    {
                       
                        s.source.Play();

                       // i.source.Play();

                        CdTime = -i.source.clip.length ;

                    }
                    else if (i.PlayerColor != playerColler)
                    {
                        Debug.LogWarning("Feedback Sound:" + name + "not  found or it did not have the correct color! ");

                    }

                    //print("in name == name. " + playerColler + " <- from func :::  -> from i "+ i.PlayerColor);
                }
            }

            if (s == null)
            {
                  Debug.LogWarning("Feedback Sound:" + name + " not found.");

            }


        }

    }

}
