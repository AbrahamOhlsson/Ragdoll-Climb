using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;



public class musicManager : MonoBehaviour
{
    [SerializeField]
    int partInt;
    public float distansToGoal; // public for test
    [Space]
    [SerializeField]
    Sound[] songs;

    [Space]
    public AudioSource song1;
    [Space]
    public AudioSource song2;
    //[Space]
    //public AudioSource song3;
    //[Space]
    //public AudioSource song4;
    //[Space]
    //public AudioSource song5;
    //[Space]
    //public AudioSource song6;
    //[Space]
    //public AudioSource song7;
    //[Space]
    //public AudioSource song8;
    //[Space]
    //public AudioSource song9;
    //[Space]
    //public AudioSource song10;

    [Space]
    [Space]
    [Space]

    public Transform startBlockTransform;
    public Transform endBlockTransform;
    public Transform cameraTransform;


    // Use this for initialization
    void Awake()
    {
        song1 = gameObject.AddComponent<AudioSource>();
        song2 = gameObject.AddComponent<AudioSource>();

        //foreach (Sound i in songs)
        //{
        //    i.source = gameObject.AddComponent<AudioSource>();
        //    i.source.clip = i.clip;

        //    i.source.volume = i.volume;
        //    i.source.pitch = i.pitch;

        //}


        startBlockTransform = GameObject.Find("StartModul").transform;

        endBlockTransform = GameObject.Find("EndModul(Clone)").transform;

        cameraTransform = GameObject.Find("Main Camera (1)").transform;

    }

    void Start()
    {
        
        partInt = 1;  // 1 = first part 

        distansToGoal = endBlockTransform.position.y - startBlockTransform.position.y;

        song1.clip = songs[0].clip;
        song1.volume = songs[0].volume;
        song1.pitch = songs[0].pitch;
        song1.Play();

        //PlaySound("music");
    }


    void Update()
    {
        //if()

        if ( (cameraTransform.position.y - startBlockTransform.position.y) > ((distansToGoal / 10)*partInt) )
        {
            partInt++;

            //Player next song 
            PlayNextMusic();
            

            //StartCoroutine( FadeOut(song1, 1f));
        } 
        

    }



    public void PlayNextMusic()
    {
        if (partInt % 2 == 1)
        {
            song1.clip = songs[partInt-1].clip;
            song1.volume = songs[partInt-1].volume;
            song1.pitch = songs[partInt-1].pitch;

            song1.time = song1.clip.length * (song2.time / song2.clip.length);

            song1.Play();

            StartCoroutine(FadeOut(song2, 0.1f));
            StartCoroutine(FadeIn(song1, 0.1f));

        }

        else
        {
            song2.clip = songs[partInt-1].clip;
            song2.volume = songs[partInt-1].volume;
            song2.pitch = songs[partInt-1].pitch;

            song2.time = song2.clip.length * (song1.time/song1.clip.length) ;

            song2.Play();

            StartCoroutine( FadeOut(song1, 0.1f));
            StartCoroutine( FadeIn(song2, 0.1f));

        }

    }



    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * (Time.deltaTime / FadeTime);

            yield return null;
        }

        audioSource.Stop();
        //audioSource.volume = startVolume;
    }


    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        audioSource.volume = 0;

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * (Time.deltaTime / FadeTime);

            yield return null;
        }

        //audioSource.Stop();
       // audioSource.volume = startVolume;
    }

}
