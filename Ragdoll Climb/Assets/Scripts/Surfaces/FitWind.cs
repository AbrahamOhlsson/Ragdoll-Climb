using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class FitWind : MonoBehaviour
{
    [SerializeField] float lenght = 20f;
    [SerializeField] float width = 3f;

    [SerializeField] float cloudAmount = 50f;
    [SerializeField] float lineAmount = 5f;
    float swirlAmount = 0.15f;

    BoxCollider coll;
    ParticleSystem[] particleSystems;
    ParticleSystem.MainModule[] psMain;
    ParticleSystem.ShapeModule[] psShape;
    ParticleSystem.EmissionModule[] psEmission;
    ParticleSystem.RotationOverLifetimeModule[] psRot;


    private void Start()
    {
        GameObject wind = Instantiate((GameObject)Resources.Load("Swirl"), Vector3.zero, Quaternion.identity, transform);
        wind.transform.localEulerAngles = new Vector3(90f, 0f, 0f);

        coll = GetComponent<BoxCollider>();

        particleSystems = GetComponentsInChildren<ParticleSystem>();

        psMain = new ParticleSystem.MainModule[particleSystems.Length];
        psShape = new ParticleSystem.ShapeModule[particleSystems.Length];
        psEmission = new ParticleSystem.EmissionModule[particleSystems.Length];
        psRot = new ParticleSystem.RotationOverLifetimeModule[particleSystems.Length];

        for (int i = 0; i < particleSystems.Length; i++)
        {
            psMain[i] = particleSystems[i].main;
            psShape[i] = particleSystems[i].shape;
            psEmission[i] = particleSystems[i].emission;
            psRot[i] = particleSystems[i].rotationOverLifetime;
        }




        // Resizes trigger box based on given values in inspector
        coll.size = new Vector3(width, coll.size.y, lenght);
        coll.center = new Vector3(0f, 0f, lenght / 2);

        if (coll.size.z < 4f)
            Destroy(wind);
        else
        {
            wind.transform.localPosition = new Vector3(0, 0, coll.center.z - 4f/2);
        }

        for (int i = 0; i < particleSystems.Length; i++)
        {
            // Sets lifetime so the particles vanishes at the end of the trigger box
            if (!particleSystems[i].name.Contains("Swirl"))
                psMain[i].startLifetime = lenght / 10;
            if (particleSystems[i].name.Contains("Lines"))
                particleSystems[i].gameObject.SetActive(false);

            // Resizes shape of particle emitter to match trigger box width
            if (!particleSystems[i].name.Contains("Swirl"))
                psShape[i].scale = new Vector3(width, psShape[i].scale.y, psShape[i].scale.z);
            else
            {
                //particleSystems[i].transform.position = transform.position + coll.center;
                psShape[i].scale = new Vector3(width, lenght - 4f, 0);
            }

            // Sets rate of particle emitting based on width and amount required
            if (particleSystems[i].name == "Clouds")
            {
                psEmission[i].rateOverTime = cloudAmount * width;
                psRot[i].enabled = true;
                psRot[i].separateAxes = true;
                ParticleSystem.MinMaxCurve curve = new ParticleSystem.MinMaxCurve();
                curve.mode = ParticleSystemCurveMode.TwoConstants;
                curve.constantMin = -3f;
                curve.constantMax = 3f;
                psRot[i].z = curve;
            }
            else if (particleSystems[i].name == "Lines")
                psEmission[i].rateOverTime = lineAmount * width;
            else if (particleSystems[i].name.Contains("Swirl"))
                psEmission[i].rateOverTime = swirlAmount * width;
        }
    }
}
