using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FitWind : MonoBehaviour
{
    [SerializeField] float lenght = 20f;
    [SerializeField] float width = 3f;

    [SerializeField] float cloudAmount = 50f;
    [SerializeField] float lineAmount = 5f;

    BoxCollider coll;
    ParticleSystem[] particleSystems;
    ParticleSystem.MainModule[] psMain;
    ParticleSystem.ShapeModule[] psShape;
    ParticleSystem.EmissionModule[] psEmission;
    //WindForce windForce;


    private void Start()
    {
        coll = GetComponent<BoxCollider>();

        particleSystems = GetComponentsInChildren<ParticleSystem>();

        psMain = new ParticleSystem.MainModule[particleSystems.Length];
        psShape = new ParticleSystem.ShapeModule[particleSystems.Length];
        psEmission = new ParticleSystem.EmissionModule[particleSystems.Length];

        for (int i = 0; i < particleSystems.Length; i++)
        {
            psMain[i] = particleSystems[i].main;
            psShape[i] = particleSystems[i].shape;
            psEmission[i] = particleSystems[i].emission;
        }

        //windForce = GetComponent<WindForce>();
    }


    private void Update()
    {
        // Resizes trigger box based on given values in inspector
        coll.size = new Vector3(width, coll.size.y, lenght);
        coll.center = new Vector3(0f, 0f, lenght / 2);

        for (int i = 0; i < particleSystems.Length; i++)
        {
            // Sets lifetime so the particles vanishes at the end of the trigger box
            psMain[i].startLifetime = lenght / 10;

            // Resizes shape of particle emitter to match trigger box width
            psShape[i].scale = new Vector3(width, psShape[i].scale.y, psShape[i].scale.z);

            // Sets rate of particle emitting based on width and amount required
            if (particleSystems[i].name == "Clouds")
                psEmission[i].rateOverTime = cloudAmount * width;
            else if (particleSystems[i].name == "Lines")
                psEmission[i].rateOverTime = lineAmount * width;
        }
    }
}
