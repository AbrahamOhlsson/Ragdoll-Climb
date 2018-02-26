using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    bool isStunned;
    float timer;

    void Update()
    { 
        if(isStunned)
        {
            timer -= Time.deltaTime;

            if (timer <= 0 && isStunned)
            {
                UnStun();
            }
        }
    }

    public void Stun(float stunTime)
    {
        if(!isStunned)
        {
            timer = stunTime;

            isStunned = true;

            GetComponent<PlayerController>().canMove = false;
            GetComponent<PlayerController>().ReleaseGrip(true, false);
            GetComponent<PlayerController>().ReleaseGrip(false, false);
        }
    }

    public void UnStun()
    {
        if (!GetComponent<FreezePlayerPowerUp>().isFrozen)
        {
            isStunned = false;
            GetComponent<PlayerController>().canMove = true;
        }
    }

}
