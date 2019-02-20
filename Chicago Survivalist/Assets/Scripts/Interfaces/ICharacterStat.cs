using UnityEngine;

public enum CharacterType
{
    PLAYER, NPC
}

public interface ICharacterStat
{
    CharacterType getType();
    /// <summary>
    /// Damage analysis
    /// </summary>
    /// <param name="damage"></param>
    void damage(IDamage damage);
}

