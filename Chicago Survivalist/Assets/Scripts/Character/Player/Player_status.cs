using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_status : Character, ICharacterStat
{
    Player_equip playerEquip;
    Player_handler playerHandler;

    // Use this for initialization
    void Start()
    {
        playerEquip = GetComponent<Player_equip>();
        playerHandler = GetComponent<Player_handler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Interface implementation
    /// <summary>
    /// Damage call
    /// </summary>
    /// <param name="damage">Damage object including type and amount of damage</param>
    public void damage(IDamage damage)
    {
        DamageAgent agent = damage.getDamageAgent();
        float damageAmount = 0;
        switch (agent)
        {
            case DamageAgent.BULLET:
                damageAmount = ((Damage)damage).getDamage(playerEquip.hasArmor());
                break;
            case DamageAgent.FREEZE:
                damageAmount = ((Damage)damage).getDamage(playerEquip.hasJacket());
                break;
            case DamageAgent.WIND:
                damageAmount = ((Damage)damage).getDamage(playerEquip.hasJacket());
                break;
            case DamageAgent.RAIN:
                damageAmount = ((Damage)damage).getDamage(playerEquip.hasUmbrella());
                break;
            case DamageAgent.ICE:
                damageAmount = ((Damage)damage).getDamage(playerEquip.hasUmbrella());
                break;
        }

        health -= damageAmount;
        damage.sideEffect(this, damageAmount);
        if(health <= 0)
        {
            playerHandler.triggerDeath();
        }
    }
    
}
