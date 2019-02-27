using UnityEngine;
namespace CharacterPackage { }
public enum CharacterType
{
    PLAYER, NPC
}

public interface ICharacterStat
{
    CharacterType getType();
    void damage(float damageAmount, DamageAgent agent);
    float getHealth();
    float getTemperature();
    float getConcussionTimer();
    bool isConcussed();
    bool hasArmor();
}

