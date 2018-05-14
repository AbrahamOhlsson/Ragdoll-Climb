using System.Collections;
using UnityEngine;



public class musicManager : MonoBehaviour
{
    bool firstUpdate;
    int partInt;
    float distansToGoal;
    [Range(0.1f,1f)] public float songVolume;
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
    
    [Space]
    [Space]
    [Space]

    public Transform startBlockTransform;
    public Transform endBlockTransform;
    public Transform bottomTransform;

    [Space]
    [Space]
    [Space]

    public bool isThisSinglePlayer;


    // Use this for initialization
    void Awake()
    {
        song1 = gameObject.AddComponent<AudioSource>();
        song2 = gameObject.AddComponent<AudioSource>();

        firstUpdate = true;

        // startBlockTransform = GameObject.Find("StartModul").transform;

        //endBlockTransform = GameObject.Find("EndModul(Clone)").transform;

        //cameraTransform = GameObject.Find("Main Camera").transform;

    }

    void Start()
    {
        distansToGoal = 0f;

        //if (startBlockTransform == null)
        //{
        //    startBlockTransform = GameObject.Find("StartModul").transform;
        //}

        //if (endBlockTransform == null)
        //{
        //    endBlockTransform = GameObject.Find("EndModul(Clone)").transform;
        //}

        //if (cameraTransform == null)
        //{
        //    cameraTransform = GameObject.Find("Main Camera").transform;
        //}

        partInt = 1;  // 1 = first part 

        //if (startBlockTransform != null && endBlockTransform != null && cameraTransform != null)
        //{
        //    distansToGoal = endBlockTransform.position.y - startBlockTransform.position.y;
        //}

        song1.loop = true;
        song2.loop = true;

        //song1.clip = songs[0].clip;
        //song1.volume = 0.6f; //  songs[0].volume;    sett the start volume too fix a bug 
        //song1.pitch = 1f;  // songs[0].pitch;
        //song1.Play();

        if (isThisSinglePlayer)
        {
            song1.clip = songs[0].clip;
            song1.volume = songVolume; //  songs[0].volume;    sett the start volume too fix a bug 
            song1.pitch = 1f;  // songs[0].pitch;
            song1.Play();
        }
       
    }


    void Update()
    {
        if (!isThisSinglePlayer)
        {

            if (firstUpdate)
            {
                if (startBlockTransform == null)                                                         // from here  (see below)
                {
                    startBlockTransform = GameObject.Find("StartModule").transform;
                }

                if (endBlockTransform == null)
                {
                    endBlockTransform = GameObject.Find("EndModule(Clone)").transform;
                }

                if (bottomTransform == null)
                {
                    bottomTransform = GameObject.Find("Bottom Object").transform;
                }


                if (distansToGoal == 0f)
                {
                    distansToGoal = endBlockTransform.position.y - startBlockTransform.position.y;
                }

                song1.clip = songs[0].clip;
                song1.volume = songVolume; //  songs[0].volume;    sett the start volume too fix a bug 
                song1.pitch = 1f;  // songs[0].pitch;
                song1.Play();

                firstUpdate = false;

                // to here is probably unnecessary and should be removed  (test it)
            }

       
            if ((bottomTransform.position.y - startBlockTransform.position.y) > ((distansToGoal / 10) * partInt))
            {
                partInt++;

                //Player next song 
                PlayNextSong();

            }
         }

    }



    public void PlayNextSong()
    {
        if (partInt % 2 == 1)
        {
            song1.clip = songs[partInt-1].clip;
            song1.volume = songVolume;
            song1.pitch = songs[partInt-1].pitch;

            song1.time = song1.clip.length * (song2.time / song2.clip.length);

            song1.Play();

            StartCoroutine(FadeOut(song2, 0.11f));
            StartCoroutine(FadeIn(song1, 0.1f));

        }

        else
        {
            song2.clip = songs[partInt-1].clip;
            song2.volume = songVolume;
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
       

        while (audioSource.volume > 0) 
        {

            audioSource.volume -= (startVolume / FadeTime) * Time.deltaTime; 
            yield return null;
        }

       
        audioSource.Stop();
        
    }


    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
 
        float startVolume = audioSource.volume;
        audioSource.volume = 0;

        while (audioSource.volume < startVolume)
        {
          
            audioSource.volume +=   (startVolume / FadeTime) * Time.deltaTime;
            yield return null;
        }
        audioSource.volume = startVolume;

        
        
    }

}
