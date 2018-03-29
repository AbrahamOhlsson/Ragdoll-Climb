using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int maxHP = 100;
    [SerializeField] float knockOutTime = 3f;

    [Tooltip("Amount of HP regained each second.")]
    [SerializeField] float healthRegen = 1f;

    float hp;

    PlayerStun stunScript;


    void Start ()
    {
        hp = maxHP;

        stunScript = GetComponent<PlayerStun>();
    }


    void Update ()
    {
        // Regains HP
		if (hp < maxHP)
        {
            hp += healthRegen * Time.deltaTime;
            hp = Mathf.Clamp(hp, 0, maxHP);
        }
	}


    public void Damage(float amount)
    {
        // No damage if already stunned
        if (!stunScript.isStunned)
        {
            hp -= amount;

            // Stunned if hp runs out
            if (hp <= 0)
            {
                stunScript.Stun(knockOutTime);
                hp = maxHP;
            }
        }
    }
}
