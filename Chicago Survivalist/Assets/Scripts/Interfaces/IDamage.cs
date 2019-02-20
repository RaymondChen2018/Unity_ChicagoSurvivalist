using UnityEngine;

public enum DamageAgent
{
    BULLET, FREEZE, ICE, WIND, CARCRUSH, RAIN
}

public interface IDamage
{
    DamageAgent getDamageAgent();
    void sideEffect(ICharacterStat character, float damageInflicted);
}
public abstract class Damage
{
    protected static float damageAmount;
    protected static Vector2 damageDirection;
    public float getRawDamageAmount()
    {
        return damageAmount;
    }
    public Vector3 getDirection()
    {
        return damageDirection;
    }
    public float outputDamage(bool defenced)
    {
        if (defenced)
        {
            return damageAmount / 2;
        }
        return damageAmount;
    }
    public void sideEffect(ICharacterStat character, float damageInflicted)
    {
        
    }
}
public class DamageBULLET : Damage, IDamage
{
    public DamageBULLET(float _damageAmount, Vector3 direction)
    {
        damageAmount = _damageAmount;
        damageDirection = direction;
    }
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.BULLET;
    }
    

}
public class DamageFREEZE : Damage, IDamage
{
    public DamageFREEZE(float _damageAmount)
    {
        damageAmount = _damageAmount;
        damageDirection = Vector3.zero;
    }
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.FREEZE;
    }
    public new void sideEffect(ICharacterStat character, float damageInflicted)
    {
        
        character.sf_freeze(damageInflicted);
    }
}
public class DamageICE : Damage, IDamage
{
    public DamageICE(float _damageAmount)
    {
        damageAmount = _damageAmount;
        damageDirection = Vector3.down;
    }
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.ICE;
    }

}
public class DamageWIND : Damage, IDamage
{
    public DamageWIND(float _damageAmount, Vector3 direction)
    {
        damageAmount = _damageAmount;
        damageDirection = direction;
    }
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.WIND;
    }
    public new void sideEffect(ICharacterStat character, float damageInflicted)
    {
        character.sf_freeze(damageInflicted);
    }
}
public class DamageCARCRUSH : Damage, IDamage
{
    public DamageCARCRUSH(float _damageAmount, Vector3 direction)
    {
        damageAmount = _damageAmount;
        damageDirection = direction;
    }
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.CARCRUSH;
    }
    public new void sideEffect(ICharacterStat character, float damageInflicted)
    {
        character.sf_concuss(damageInflicted);
    }
}
public class DamageRAIN : Damage, IDamage
{
    public DamageRAIN(float _damageAmount)
    {
        damageAmount = _damageAmount;
        damageDirection = Vector3.down;
    }
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.RAIN;
    }
}