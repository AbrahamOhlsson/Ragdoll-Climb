using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyProgression : MonoBehaviour
{
    [SerializeField] Lobby lobby;
    [SerializeField] CharacterSelection_SP characterSelection;

    int keyAmount;

    Singleton singleton;


	void Start ()
    {
        singleton = Singleton.instance;

        for (int i = 0; i < singleton.levelStats_woods.Count; i++)
            keyAmount += singleton.levelStats_woods[i].starAmount;
        for (int i = 0; i < singleton.levelStats_ice.Count; i++)
            keyAmount += singleton.levelStats_ice[i].starAmount;
        for (int i = 0; i < singleton.levelStats_volcano.Count; i++)
            keyAmount += singleton.levelStats_volcano[i].starAmount;
        for (int i = 0; i < singleton.levelStats_metal.Count; i++)
            keyAmount += singleton.levelStats_metal[i].starAmount;

        if (keyAmount == 99)
        {
            lobby.canSwitchCharacter = true;
            characterSelection.canSwitchCharacter = true;
        }
    }
	

	void Update () {
		
	}
}
