using UnityEngine;
/// <summary>
/// AI's character status including health and body temperature
/// AI's health & temperature does not influence the AI's behaviors.
/// </summary>
public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// Is this character deceased?
    /// </summary>
    protected bool Deceased = false;
    /// <summary>
    /// Maximum health
    /// </summary>
    const float MAX_HEALTH = 100;
    /// <summary>
    /// Current health level
    /// </summary>
    [SerializeField] protected float health = 100;

    /// <summary>
    /// How fast does the character's body temperature aligns with the weather's temperature
    /// </summary>
    protected const float TEMPERATURE_ADJUST_RATE = 0.05f;
    /// <summary>
    /// Temperature point under where the character receive damage
    /// </summary>
    public const float FREEZE_START_THREDSHOLD = -2;
    /// <summary>
    /// Thermal recover per second
    /// </summary>
    const float TEMPERATURE_RECOVER_RATE = 1;
    /// <summary>
    /// Temperature of body, used to determine if the weather's temperature can damage the character
    /// </summary>
    const float BODY_TEMPERATURE = 27;
    /// <summary>
    /// Current body temperature
    /// </summary>
    [SerializeField] protected float bodyTemperature = 27;
    /// <summary>
    /// In seconds; When in concussion, the character's behavior is affected
    /// </summary>
    [SerializeField] protected float concussion = 0;

    protected void update()
    {
        //Recover concussion
        recoverConcussion();
    }
    /// <summary>
    /// Count down the concussion timer
    /// </summary>
    protected void recoverConcussion()
    {
        if (concussion > 0)
        {
            concussion = Mathf.Max(concussion - Time.deltaTime, concussion);
        }
    }
    /// <summary>
    /// Body recovers temperature to normal body temperature (27 degree)
    /// </summary>
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
    /// Concussion length reaction regard to damage infliction
    /// </summary>
    /// <param name="damageReference">Damage dealt</param>
    protected void sf_concuss(float damageReference)
    {
        concussion += damageReference / 20;
    }
    /// <summary>
    /// Prompts to adjust to weather temperature with a multiplier
    /// Temperature reaction regard to damage infliction
    /// </summary>
    /// <param name="adjustRateMultiplier">Damage dealt</param>
    protected void sf_adjust_temperature(float adjustRateMultiplier, float temperatureOffset = 0)
    {
        //Fix the adjustrate and fix wind damage
        float weatherTemperature = Weather.getTemperature();
        bodyTemperature = Mathf.Lerp(bodyTemperature, weatherTemperature + temperatureOffset, TEMPERATURE_ADJUST_RATE * adjustRateMultiplier);
    }
    /// <summary>
    /// Decease
    /// </summary>
    /// <param name="agent"></param>
    protected virtual void Decease(DamageAgent agent)
    {
        if (Deceased)
        {   
            return;
        }
        Deceased = true;
    }
    /// <summary>
    /// Revive
    /// </summary>
    public virtual void Revive()
    {
        if (!Deceased)
        {
            return;
        }
        Deceased = false;
        health = MAX_HEALTH;
    }
    /// <summary>
    /// Reduce health and inflicts side effects, if any.
    /// </summary>
    /// <param name="damageAmount">The amound of damage inflicted</param>
    /// <param name="agent">The type of damage inflicted</param>
    public virtual void damage(float damageAmount, DamageAgent agent)
    {
        float dmg = damageAmount;
        //Side effects
        switch (agent)
        {
            case DamageAgent.WIND:
                sf_adjust_temperature(dmg, Weather.WIND_INFLUENCE_OFFSET);
                dmg = 0;
                break;
            case DamageAgent.ICE:
                sf_concuss(dmg);
                break;
            case DamageAgent.CARCRUSH:
                sf_concuss(dmg);
                break;
        }
        health -= dmg;
        if (health <= 0)
        {
            Decease(agent);
        }
    }
    /// <summary>
    /// Get current health level
    /// </summary>
    /// <returns></returns>
    public float getHealth()
    {
        return health;
    }
    /// <summary>
    /// Get current temperature level
    /// </summary>
    /// <returns></returns>
    public float getTemperature()
    {
        return bodyTemperature;
    }
    /// <summary>
    /// Get the remaining concussion time in seconds
    /// </summary>
    /// <returns></returns>
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
}
