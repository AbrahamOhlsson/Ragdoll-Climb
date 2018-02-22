using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelGeneration : MonoBehaviour
{
    public List<Transform> levelModules = new List<Transform>();

    public Transform goal;

    public int moduleAmount = 5;

    float modulesDistY = 15;
    int randomIndex = 0;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < moduleAmount; i++)
        {
            randomIndex = Random.Range(0, levelModules.Count);

            Instantiate(levelModules[randomIndex], new Vector3(0, modulesDistY * i), Quaternion.identity);
        }

        Instantiate(goal, new Vector3(0, modulesDistY * moduleAmount), Quaternion.identity);
        print(modulesDistY);
    }
}
