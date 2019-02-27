using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI's health & temperature does not influence the AI's behaviors.
/// </summary>
public class NPC_status : Character, ICharacterStat
{
    /// <summary>
    /// AI type
    /// </summary>
    /// <returns></returns>
    public CharacterType getType()
    {
        return CharacterType.NPC;
    }
    /// <summary>
    /// Reduce health and take actions in response to damage
    /// </summary>
    /// <param name="damageAmount">The amound of damage inflicted</param>
    /// <param name="agent">The type of damage inflicted</param>
    override public void damage(float damageAmount, DamageAgent agent)
    {
        float damage = damageAmount;
        if(agent == DamageAgent.BULLET && hasArmor())
        {
            damage *= 0.5f;
        }
        base.damage(damage, agent);
    }
    /// <summary>
    /// Is this AI armored?
    /// </summary>
    /// <returns></returns>
    public bool hasArmor()
    {
        return false;
    }
}
