using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrunts : MonoBehaviour {

    [SerializeField] Sound[] Grunts;
    [HideInInspector] public List <AudioSource> GruntsList;

    [SerializeField] float gruntDelay;
    float cdTime;
    bool canGrunt;

    // Use this for initialization
    void Awake()
    {
        foreach (Sound i in Grunts)
        {
            i.source = gameObject.AddComponent<AudioSource>();
            i.source.clip = i.clip;

            i.source.volume = i.volume;
            i.source.pitch = i.pitch;
            i.source.playOnAwake = false;

            GruntsList.Add(i.source);
        }

    }

    void Start()
    {
        cdTime = 0;
        canGrunt = true;

    }

    private void Update()
    {
        if (cdTime > 0)
        {
            cdTime -= 1 * Time.deltaTime; 

        }

        if(cdTime <= 0 && canGrunt == false)
        {
            canGrunt = true;

        }

    }


    public void PlayGrunt()
    {
        if (canGrunt)
        {
            int i = Random.Range(0, GruntsList.Count);
            GruntsList[i].pitch = Random.Range(0.9f, 1.1f);
            GruntsList[i].Play();
            cdTime = gruntDelay;
            canGrunt = false;
        }
    }

}
