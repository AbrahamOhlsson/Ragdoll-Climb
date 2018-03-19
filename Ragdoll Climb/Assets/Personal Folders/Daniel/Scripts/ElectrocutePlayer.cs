using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrocutePlayer : MonoBehaviour {

	GameObject electrocutePlayer;
	bool canStun =true;

	void OnTriggerEnter(Collider other)
	{
		electrocutePlayer = other.transform.root.gameObject;

		if (electrocutePlayer.tag == "Player" && canStun)
		{
            if (electrocutePlayer.GetComponent<PlayerInfo>().solid)
            {
                canStun = false;
                int i = Random.Range(1, PlayerInfoSingleton.instance.playerAmount);

                print("player amount" + PlayerInfoSingleton.instance.playerAmount + 1);
                print("stun player test " + i);
                GameObject.Find("Player " + i + "/Main/DeformationSystem/LightningCloud").SetActive(true);
                GameObject.Find("Player " + i + "/Main/DeformationSystem/LightningCloud").GetComponent<LightningBolt>().startLightning();
                //electrocutePlayer.transform.Find("Main/DeformationSystem/LightningCloud").gameObject.SetActive(true);

                Destroy(gameObject);
            }
		}
	}
}
