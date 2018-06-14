using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

public class MovingCranePlatform : MonoBehaviour {

    [SerializeField]
    Transform platform;

    [SerializeField]
    Transform startTransform;

    [SerializeField]
    Transform endTransform;

    [SerializeField]
    float platformSpeed;

    [SerializeField]
    float lerpSpeed = 0.2f;

    Vector3 direction;
    Vector3 velocity = Vector3.zero;
    Transform destination;

    AudioSource myAudioSource;


    private void Start()
    {
        SetDestination(startTransform);

        myAudioSource = gameObject.AddComponent<AudioSource>();

        myAudioSource.clip = (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/Music and sound/SFX/worldSpark.wav", typeof(AudioClip));
       
        myAudioSource.playOnAwake = true;
        myAudioSource.loop = true;
        myAudioSource.time = Random.Range(0, myAudioSource.clip.length);
        myAudioSource.spatialBlend = 1;
        myAudioSource.maxDistance = 30;
        myAudioSource.minDistance = 0.5f;
        myAudioSource.Play();

        myAudioSource.outputAudioMixerGroup = GameObject.Find("music and sound").GetComponent<soundManager>().MyMixerGroup;
    }




    private void FixedUpdate()
    {
        //platform.GetComponent<Rigidbody>().MovePosition(platform.position + direction * platformSpeed * Time.fixedDeltaTime);
        platform.GetComponent<Rigidbody>().position = Vector3.SmoothDamp(platform.GetComponent<Rigidbody>().position, destination.position, ref velocity, platformSpeed);

        if(Vector3.Distance (platform.position, destination.position) <= 0.1f)
        {
            SetDestination(destination == startTransform ? endTransform : startTransform);
        }
    
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startTransform.position, platform.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(endTransform.position, platform.localScale);
    }


    void SetDestination(Transform dest)
    {
        destination = dest;
        direction = (destination.position - platform.position).normalized;
    }

}
