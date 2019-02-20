using UnityEngine;

public enum CharacterType
{
    PLAYER, NPC
}

public interface ICharacterStat
{
    float getHealth();
    float getTemperature();
    CharacterType getType();
    /// <summary>
    /// Damage analysis
    /// </summary>
    /// <param name="damage"></param>
    void damage(IDamage damage);
    /// <summary>
    /// Damage every second, slow down movement
    /// </summary>
    /// <param name="temperatureDrop"></param>
    void sf_freeze(float damageReference);
    /// <summary>
    /// Temperarily slow down character movement
    /// </summary>
    void sf_concuss(float damageReference);
}

