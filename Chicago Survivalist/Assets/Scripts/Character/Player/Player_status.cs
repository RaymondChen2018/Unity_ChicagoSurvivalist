using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Player's health & temperature influence GUI elements and gameplay.
/// </summary>
public class Player_status : Character, ICharacterStat
{
    //How much does jacket prevent freeze damage & temperature decline
    const float JACKET_PROTECTION_RATIO = 0.2f;
    
    //Components
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
        base.update();
        //Fit to environment temperature
        float weatherTemperature = Weather.getTemperature();
        if (Mathf.Abs(bodyTemperature - weatherTemperature) > 1)
        {
            float jacketMultiplier = 1;
            if (playerEquip.hasJacket() && bodyTemperature > weatherTemperature)//If decreasing temperature and has jacket, reduce temperature decrease speed
            {
                jacketMultiplier = JACKET_PROTECTION_RATIO;
            }
            sf_adjust_temperature(Time.deltaTime * jacketMultiplier);
        }
        else
        {
            bodyTemperature = weatherTemperature;
        }
        //Receive damage from low temperature
        if (bodyTemperature < FREEZE_START_THREDSHOLD)
        {
            damage((FREEZE_START_THREDSHOLD - bodyTemperature) * 0.1f * Time.deltaTime, DamageAgent.FREEZE);
        }
    }

    ////Interface implementation
    ///// <summary>
    ///// Damage call
    ///// </summary>
    ///// <param name="damage">Damage object including type and amount of damage</param>
    //public void damage(IDamage damage)
    //{
    //    DamageAgent agent = damage.getDamageAgent();
    //    float damageAmount = 0;
    //    switch (agent)
    //    {
    //        case DamageAgent.BULLET:
    //            damageAmount = ((Damage)damage).getDamage(playerEquip.hasArmor());
    //            break;
    //        case DamageAgent.FREEZE:
    //            damageAmount = ((Damage)damage).getDamage(playerEquip.hasJacket());
    //            break;
    //        case DamageAgent.WIND:
    //            damageAmount = ((Damage)damage).getDamage(playerEquip.hasJacket());
    //            break;
    //        case DamageAgent.RAIN:
    //            damageAmount = ((Damage)damage).getDamage(playerEquip.hasUmbrella());
    //            break;
    //        case DamageAgent.ICE:
    //            damageAmount = ((Damage)damage).getDamage(playerEquip.hasUmbrella());
    //            break;
    //    }

    //    health -= damageAmount;
    //    damage.sideEffect(this, damageAmount);
    //    if(health <= 0)
    //    {
    //        Decease(damage.getDamageAgent());
    //    }
    //}
    /// <summary>
    /// Player type
    /// </summary>
    /// <returns></returns>
    public CharacterType getType()
    {
        return CharacterType.PLAYER;
    }
    /// <summary>
    /// Reduce health and take actions in response to damage
    /// </summary>
    /// <param name="damageAmount">The amound of damage inflicted</param>
    /// <param name="agent">The type of damage inflicted</param>
    override public void damage(float damageAmount, DamageAgent agent)
    {
        float damage = damageAmount;
        switch (agent){
            case DamageAgent.BULLET:
                if (playerEquip.hasArmor()) { damage *= 0.5f; }
                break;
            case DamageAgent.FREEZE:
                if (playerEquip.hasJacket()) { damage *= JACKET_PROTECTION_RATIO; }
                break;
            case DamageAgent.WIND:
                if (playerEquip.hasJacket()) { damage *= JACKET_PROTECTION_RATIO; }
                break;
            case DamageAgent.RAIN:
                if (playerEquip.hasUmbrella()) { damage *= 0.9f; }
                break;
            case DamageAgent.ICE:
                if (playerEquip.hasUmbrella()) { damage *= 0.5f; }
                break;
            case DamageAgent.CARCRUSH:
                if (playerEquip.hasArmor()) { damage *= 0.5f; }
                break;
        }
        base.damage(damage, agent);
    }
    /// <summary>
    /// Revive
    /// </summary>
    public override void Revive()
    {
        if (!Deceased)
        {
            return;
        }
        base.Revive();
        playerHandler._triggerRevive();
    }
    /// <summary>
    /// Display GUI elements on death
    /// </summary>
    /// <param name="agent">The type of damage that killed the player</param>
    protected override void Decease(DamageAgent agent)
    {
        if (Deceased)
        {
            return;
        }
        base.Decease(agent);
        playerHandler._triggerDeath(agent);
    }
    /// <summary>
    /// Is this Player armored?
    /// </summary>
    /// <returns></returns>
    public bool hasArmor()
    {
        return playerEquip.hasArmor();
    }

    


    //Debug commands====================================
    public void debug_damage_wind(float dmg)
    {
        Debug.Log("Wind damage!");
        damage(dmg, DamageAgent.WIND);
    }
    public void debug_damage_bullet(float dmg)
    {
        Debug.Log("Bullet damage!");
        damage(dmg, DamageAgent.BULLET);
    }
}
