using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
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
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Recover concussion
        if(concussion > 0)
        {
            concussion = Mathf.Max(concussion - Time.deltaTime, concussion);
        }
		//Recover temperature
        float temperatureRecoverAmount = TEMPERATURE_RECOVER_RATE * Time.deltaTime;
        if (bodyTemperature < TEMPERATURE_RECOVER_RATE)
        {
            bodyTemperature = Mathf.Min(bodyTemperature, bodyTemperature + temperatureRecoverAmount);
        }
        else if (bodyTemperature > TEMPERATURE_RECOVER_RATE)
        {
            bodyTemperature = Mathf.Max(bodyTemperature, bodyTemperature - temperatureRecoverAmount);
        }
        
    }

    /// <summary>
    /// Is this character currently in a state of concussion?
    /// </summary>
    /// <returns></returns>
    public bool isConcussed()
    {
        return concussion > 0;
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
        bodyTemperature -= damageReference / 10;
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
    public CharacterType getType()
    {
        return CharacterType.PLAYER;
    }
}
