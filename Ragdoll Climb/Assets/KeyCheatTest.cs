using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCheatTest : MonoBehaviour
{
    Singleton singleton;
    bool firstTime = true;


    void Start ()
    {
        singleton = Singleton.instance;
	}


    void LateUpdate ()
    {
		if (firstTime)
        {
            for (int i = 0; i < singleton.levelStats_woods.Count; i++)
            {
                singleton.levelStats_woods[i].completed = true;

                if (i != 0)
                {
                    singleton.levelStats_woods[i].starAmount = 3;
                }
            }
            for (int i = 0; i < singleton.levelStats_ice.Count; i++)
            {
                singleton.levelStats_ice[i].completed = true;
                singleton.levelStats_ice[i].starAmount = 3;
            }
            for (int i = 0; i < singleton.levelStats_volcano.Count; i++)
            {
                singleton.levelStats_volcano[i].completed = true;
                singleton.levelStats_volcano[i].starAmount = 3;
            }
            for (int i = 0; i < singleton.levelStats_metal.Count; i++)
            {
                singleton.levelStats_metal[i].completed = true;
                singleton.levelStats_metal[i].starAmount = 3;
            }

            singleton.Save();
        }
	}
}
