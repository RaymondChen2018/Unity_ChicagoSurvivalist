using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_status : Character, ICharacterStat
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Damage call
    /// </summary>
    /// <param name="damage">Damage object including type and amount of damage</param>
    public void damage(IDamage damage)
    {
        DamageAgent agent = damage.getDamageAgent();
        float damageAmount = ((Damage)damage).getDamage(false);

        health -= damageAmount;
        damage.sideEffect(this, damageAmount);
    }

    public void sf_damage(float damageAmount, DamageAgent agent)
    {
        throw new System.NotImplementedException();
    }
}
