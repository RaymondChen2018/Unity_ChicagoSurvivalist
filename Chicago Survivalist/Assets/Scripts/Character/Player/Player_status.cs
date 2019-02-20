using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_status : Character
{
    Player_equip playerEquip;

    // Use this for initialization
    void Start()
    {
        playerEquip = GetComponent<Player_equip>();
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
    public new void damage(IDamage damage)
    {
        DamageAgent agent = damage.getDamageAgent();
        float damageAmount = 0;
        switch (agent)
        {
            case DamageAgent.BULLET:
                damageAmount = ((Damage)damage).outputDamage(playerEquip.hasArmor());
                break;
            case DamageAgent.FREEZE:
                damageAmount = ((Damage)damage).outputDamage(playerEquip.hasJacket());
                break;
            case DamageAgent.WIND:
                damageAmount = ((Damage)damage).outputDamage(playerEquip.hasJacket());
                break;
            case DamageAgent.RAIN:
                damageAmount = ((Damage)damage).outputDamage(playerEquip.hasUmbrella());
                break;
            case DamageAgent.ICE:
                damageAmount = ((Damage)damage).outputDamage(playerEquip.hasUmbrella());
                break;
        }

        health -= damageAmount;
        damage.sideEffect(this, damageAmount);
    }
    
}
