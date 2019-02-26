using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// Temperature point under where the character receive damage
    /// </summary>
    public const float FREEZE_START_THREDSHOLD = -2;
    protected bool Deceased = false;
    /// <summary>
    /// Thermal recover per second
    /// </summary>
    const float TEMPERATURE_RECOVER_RATE = 1;
    const float BODY_TEMPERATURE = 27;
    const float MAX_HEALTH = 100;
    [SerializeField] protected float health = 100;
    [SerializeField] protected float bodyTemperature = 27;
    /// <summary>
    /// In seconds
    /// </summary>
    [SerializeField] protected float concussion = 0;

    //Temporary variables
    protected Action<DamageAgent> triggerDeathListener;

    protected void update()
    {
        
        //Recover concussion
        recoverConcussion();
        //Recover temperature
        //recoverTemperature();
    }

    protected void recoverConcussion()
    {
        if (concussion > 0)
        {
            concussion = Mathf.Max(concussion - Time.deltaTime, concussion);
        }
    }
    protected void recoverTemperature()
    {
        float temperatureRecoverAmount = TEMPERATURE_RECOVER_RATE * Time.deltaTime;
        if (bodyTemperature < BODY_TEMPERATURE)
        {
            bodyTemperature = Mathf.Min(BODY_TEMPERATURE, bodyTemperature + temperatureRecoverAmount);
        }
        else if (bodyTemperature > BODY_TEMPERATURE)
        {
            bodyTemperature = Mathf.Max(BODY_TEMPERATURE, bodyTemperature - temperatureRecoverAmount);
        }
    }
    

    /// <summary>
    /// Temperarily slow down character movement;
    /// Concussion reaction regard to damage infliction
    /// </summary>
    /// <param name="damageReference">Damage dealt</param>
    public void sf_concuss(float damageReference)
    {
        concussion += damageReference / 20;
    }
    /// <summary>
    /// Damage every second, slow down movement;
    /// Temperature reaction regard to damage infliction
    /// </summary>
    /// <param name="damageReference">Damage dealt</param>
    public void sf_freeze(float damageReference)
    {
        bodyTemperature -= damageReference * 2;
    }
    public float getHealth()
    {
        return health;
    }
    public float getTemperature()
    {
        return bodyTemperature;
    }
    public float getConcussionTimer()
    {
        return concussion;
    }
    /// <summary>
    /// Is this character currently in a state of concussion?
    /// </summary>
    /// <returns></returns>
    public bool isConcussed()
    {
        return concussion > 0;
    }
    public CharacterType getType()
    {
        return CharacterType.PLAYER;
    }
    public void Revive()
    {
        if (Deceased)
        {
            Deceased = false;
            health = MAX_HEALTH;
            triggerRevive();
        }
    }


    protected void Decease(DamageAgent agent)
    {
        if (!Deceased)
        {
            Deceased = true;
            triggerDeathListener(agent);
        }
    }
    protected void triggerRevive()
    {
        Debug.Log("Character.revive");
    }


}
