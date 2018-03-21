using System.Collections;
using UnityEngine;



public class musicManager : MonoBehaviour
{
    //[SerializeField]
    //float musicVolume;

    [SerializeField]
    int partInt;
    float distansToGoal; // public for test
    [Space]
    [SerializeField]
    Sound[] songs;

    [Space]
    [Space]
    [Space]
    [Space]
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

        song1.loop = true;
        song2.loop = true;

        song1.clip = songs[0].clip;
        song1.volume = songs[0].volume;
        song1.pitch = songs[0].pitch;
        song1.Play();

        //PlaySound("music");
    }


    void Update()
    {
        if (startBlockTransform == null)
        {
            startBlockTransform = GameObject.Find("StartModul").transform;
        }

        if (endBlockTransform == null)
        {
            endBlockTransform = GameObject.Find("EndModul(Clone)").transform;
        }

        if (cameraTransform == null)
        {
            cameraTransform = GameObject.Find("Main Camera (1)").transform;
        }




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

            StartCoroutine(FadeOut(song2, 0.11f));
            StartCoroutine(FadeIn(song1, 0.1f));

        }

        else
        {
            song2.clip = songs[partInt-1].clip;
            song2.volume = songs[partInt-1].volume;
            song2.pitch = songs[partInt-1].pitch;

            song2.time = song2.clip.length * (song1.time/song1.clip.length) ;

            song2.Play();

            StartCoroutine( FadeOut(song1, 0.11f));
            StartCoroutine( FadeIn(song2, 0.1f));

        }

    }



    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        float timetest = 0;     // TA BORT SEN 

        while (audioSource.volume > 0) 
        {
            timetest += 1 * Time.deltaTime;
            audioSource.volume -= (startVolume / FadeTime) * Time.deltaTime; //startVolume * (FadeTime / Time.deltaTime);

            yield return null;
        }

        print("klar i fade Out på " + timetest + " sec");
        audioSource.Stop();
        
    }


    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float timetest = 0;     // TA BORT SEN 
        float startVolume = audioSource.volume;
        audioSource.volume = 0;

        while (audioSource.volume < startVolume)
        {
            timetest += 1 * Time.deltaTime;

            audioSource.volume +=   (startVolume / FadeTime) * Time.deltaTime; // startVolume * ( FadeTime / Time.deltaTime);
            print("här kommer FadeTime / Time.deltaTime " + 1*(Time.deltaTime / FadeTime) );
            yield return null;
        }
        audioSource.volume = startVolume;

        print("klar i fade in på " + timetest + " sec");
        
    }

}
