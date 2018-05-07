using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleSpawner : MonoBehaviour
{
    [SerializeField] float spawnDelay = 3f;
    [SerializeField] GameObject icicle;

    float timer = 0;

    float growthSpd = 0;

    float stunTime = 2;

    float autoDestroyTime = 2;
    
    Transform bottomObj;

    Vector3 scale;


	void Awake ()
    {
        scale = transform.GetChild(0).localScale;
        growthSpd = GetComponentInChildren<Icicle>().growthSpeed;
        stunTime = GetComponentInChildren<Icicle>().stunTime;
        autoDestroyTime = GetComponentInChildren<Icicle>().autoDestroyTime;

        transform.GetChild(0).GetComponent<Icicle>().instantiated = true;

        bottomObj = GameObject.FindGameObjectWithTag("BottomObj").transform;
    }


    void Update ()
    {
        if (timer >= spawnDelay)
        {
            GameObject inst = Instantiate(icicle, transform);
            inst.transform.localScale = scale;
            inst.GetComponent<Icicle>().instantiated = true;
            inst.GetComponent<Icicle>().growthSpeed = growthSpd;
            inst.GetComponent<Icicle>().stunTime = stunTime;
            inst.GetComponent<Icicle>().autoDestroyTime = autoDestroyTime;

            timer = 0;
        }
        else
            timer += Time.deltaTime;

        if (transform.position.y <= bottomObj.position.y)
            Destroy(gameObject);
    }
}
