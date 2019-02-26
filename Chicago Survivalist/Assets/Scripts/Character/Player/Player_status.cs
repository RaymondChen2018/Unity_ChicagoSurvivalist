using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_status : Character, ICharacterStat
{
    
    Player_equip playerEquip;
    Player_handler playerHandler;
    public Player_status()
    {
        triggerDeathListener = triggerDeath;
    }

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
            float adjust_rate = 0.05f;
            if (playerEquip.hasJacket() && bodyTemperature > weatherTemperature)//If decreasing temperature and has jacket, reduce temperature decrease speed
            {
                adjust_rate *= 0.2f;
            }
            bodyTemperature = Mathf.Lerp(bodyTemperature, weatherTemperature, adjust_rate * Time.deltaTime);
        }
        else
        {
            bodyTemperature = weatherTemperature;
        }
        //Receive damage from low temperature
        if (bodyTemperature < FREEZE_START_THREDSHOLD)
        {
            sf_damage((FREEZE_START_THREDSHOLD - bodyTemperature) * 0.1f * Time.deltaTime, DamageAgent.FREEZE);
        }
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
            Decease(damage.getDamageAgent());
        }
    }

    public void sf_damage(float damageAmount, DamageAgent agent)
    {
        float damage = damageAmount;

        switch (agent){
            case DamageAgent.BULLET:
                if (playerEquip.hasArmor()) { damage *= 0.5f; }
                break;
            case DamageAgent.FREEZE:
                if (playerEquip.hasJacket()) { damage *= 0.2f; }
                break;
            case DamageAgent.WIND:
                if (playerEquip.hasJacket()) { damage *= 0.4f; }
                sf_freeze(damage);
                break;
            case DamageAgent.RAIN:
                if (playerEquip.hasUmbrella()) { damage *= 0.9f; }
                break;
            case DamageAgent.ICE:
                if (playerEquip.hasUmbrella()) { damage *= 0.5f; }
                sf_concuss(damage);
                break;
            case DamageAgent.CARCRUSH:
                if (playerEquip.hasArmor()) { damage *= 0.5f; }
                sf_concuss(damage);
                break;
        }

        health -= damage;

        if (health <= 0)
        {
            Decease(agent);
        }
    }


    public void debug_damage_wind(float dmg)
    {
        Debug.Log("Wind damage!");
        sf_damage(dmg, DamageAgent.WIND);
    }
    public void debug_damage_bullet(float dmg)
    {
        Debug.Log("Bullet damage!");
        sf_damage(dmg, DamageAgent.BULLET);
    }

    new void triggerRevive()
    {
        playerHandler.triggerRevive();
    }
    void triggerDeath(DamageAgent agent)
    {
        playerHandler.triggerDeath(agent);
    }

}
