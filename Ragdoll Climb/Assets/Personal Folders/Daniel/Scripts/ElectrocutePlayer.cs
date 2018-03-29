using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrocutePlayer : MonoBehaviour
{
	GameObject electrocutePlayer;
	bool canStun =true;

    List<GameObject> players;

    private void Start()
    {
        players = GameObject.Find("GameManager").GetComponent<MultiplayerManager>().players;
    }

    void OnTriggerEnter(Collider other)
	{
		electrocutePlayer = other.transform.root.gameObject;

		if (electrocutePlayer.tag == "Player" && canStun)
		{
            if (electrocutePlayer.GetComponent<PlayerInfo>().solid)
            {
                canStun = false;

                int i = Random.Range(0, PlayerInfoSingleton.instance.playerAmount);

                players[i].transform.Find("Main/DeformationSystem/LightningCloud").gameObject.SetActive(true);
                players[i].transform.Find("Main/DeformationSystem/LightningCloud").GetComponent<LightningBolt>().startLightning();
                //GameObject.Find("Player " + i + "/Main/DeformationSystem/LightningCloud").SetActive(true);
                //GameObject.Find("Player " + i + "/Main/DeformationSystem/LightningCloud").GetComponent<LightningBolt>().startLightning();
                //electrocutePlayer.transform.Find("Main/DeformationSystem/LightningCloud").gameObject.SetActive(true);

                Destroy(gameObject);
            }
		}
	}
}
