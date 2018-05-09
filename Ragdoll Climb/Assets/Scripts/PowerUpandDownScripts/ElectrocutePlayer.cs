using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrocutePlayer : MonoBehaviour
{
	bool canStun = true;
    int topPlayerNum;
    int bottomPlayerNum;

    GameObject topPlayer;
    GameObject bottomPlayer;


    
     public List<GameObject> playerRoot_mList;

    private void Start()
    {
        topPlayerNum=0;
        bottomPlayerNum=0;

    }

    void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && canStun)
		{
            if (other.transform.root.GetComponent<PlayerInfo>().solid)
            {
                canStun = false;
                

                foreach (GameObject playerRoot_m in GameObject.FindGameObjectsWithTag("Player"))
                {
                    
                    if (playerRoot_m.name == "Root_M")
                    {
                        
                        playerRoot_mList.Add(playerRoot_m);
                        
                    }
                }

             

                for (int j = 0; j < Singleton.instance.playerAmount; j++)
                {
                    if (playerRoot_mList[topPlayerNum].transform.position.y < playerRoot_mList[j].transform.position.y) 
                    {
                        topPlayerNum = j;
                       
                    }

                }


                for (int j = 0; j < Singleton.instance.playerAmount; j++)
                {
                    if (playerRoot_mList[bottomPlayerNum].transform.position.y > playerRoot_mList[j].transform.position.y)
                      {
                        bottomPlayerNum = j;
                    }
                }
                

                topPlayer = playerRoot_mList[topPlayerNum];
                bottomPlayer = playerRoot_mList[bottomPlayerNum];

               
                playerRoot_mList.Remove(bottomPlayer); // remove the bottom player 
                playerRoot_mList.Add(topPlayer);     // increase the chance of the top player is hit

               
                int i = Random.Range(0, Singleton.instance.playerAmount);

                LightningBolt lightningCloud = playerRoot_mList[i].transform.root.GetComponentInChildren<LightningBolt>(true);
                lightningCloud.gameObject.SetActive(true);
                lightningCloud.StartLightning();

                
                //players[i].transform.Find("Main/DeformationSystem/LightningCloud").gameObject.SetActive(true);
                //players[i].transform.Find("Main/DeformationSystem/LightningCloud").GetComponent<LightningBolt>().startLightning();

                //GameObject.Find("Player " + i + "/Main/DeformationSystem/LightningCloud").SetActive(true);
                //GameObject.Find("Player " + i + "/Main/DeformationSystem/LightningCloud").GetComponent<LightningBolt>().startLightning();
                //electrocutePlayer.transform.Find("Main/DeformationSystem/LightningCloud").gameObject.SetActive(true);

                Destroy(gameObject);

            }
		}
	}
}
