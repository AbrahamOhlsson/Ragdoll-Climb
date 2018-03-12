using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class feedbackManager : MonoBehaviour {

    
    public feedbackSound[] feedbackSounds;
   

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
        //PlaySound("ice", 1);
        //gameObject.GetComponent<feedbackManager>().PlaySound("sada",1);

    }


    /// <summary>
    ///  "play a feedback sound "
    /// </summary>
    /// <param name="name"> name of the sound </param>
    /// <param name="player"> / Red = 1 / Blue = 2 / Green = 3 / Yellow = 4 / </param>
    public void PlaySound(string name, feedbackSound.player playerColler)
    {
        feedbackSound s = null;  //varför en ny? (se line 51   ;) )

        foreach (feedbackSound i in feedbackSounds)
        {
            if (i.name == name)
            {
                


                if (i.PlayerColor == playerColler)  // Red = 1 / Blue = 2 / Green = 3 / Yellow = 4 /
                {
                    s = i;
                    s.source.Play();
                    
                    i.source.Play();
                    
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
          //  Debug.LogWarning("Feedback Sound:" + name + " not found.");

        }




    }

}
