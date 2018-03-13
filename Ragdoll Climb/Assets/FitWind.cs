using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FitWind : MonoBehaviour
{
    [SerializeField] float size = 20f;

    BoxCollider collider;
    ParticleSystem[] particleSystems;
    ParticleSystem.MainModule[] psMain;
    WindForce windForce;


    private void Start()
    {
        collider = GetComponent<BoxCollider>();

        particleSystems = GetComponentsInChildren<ParticleSystem>();

        psMain = new ParticleSystem.MainModule[particleSystems.Length];

        for (int i = 0; i < particleSystems.Length; i++)
        {
            psMain[i] = particleSystems[i].main;
        }

        windForce = GetComponent<WindForce>();
    }


    private void Update()
    {
        collider.size = new Vector3(collider.size.x, collider.size.y, size);
        collider.center = new Vector3(0f, 0f, size / 2);

        for (int i = 0; i < particleSystems.Length; i++)
        {
            psMain[i].startLifetime = size / 10;
        }
    }
}
