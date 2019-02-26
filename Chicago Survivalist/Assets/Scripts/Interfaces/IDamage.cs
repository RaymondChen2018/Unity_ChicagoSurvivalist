using UnityEngine;

public enum DamageAgent
{
    GENERIC, BULLET, FREEZE, ICE, WIND, CARCRUSH, RAIN
}

public interface IDamage
{
    DamageAgent getDamageAgent();
    void sideEffect(Character character, float damageInflicted);
}
public abstract class Damage
{
    public static DamageBULLET Bullet;
    public static DamageCARCRUSH CarCrush;
    public static DamageFREEZE Freeze;
    public static DamageICE Ice;
    public static DamageRAIN Rain;
    public static DamageWIND Wind;

    public float damageAmount;
    public Vector2 damageDirection;
    public static void damageInitialize()
    {
        Bullet = new DamageBULLET();
        CarCrush = new DamageCARCRUSH();
        Freeze = new DamageFREEZE();
        Ice = new DamageICE();
        Rain = new DamageRAIN();
        Wind = new DamageWIND();
    }
    public float getDamage(bool defenced)
    {
        if (defenced)
        {
            return damageAmount / 2;
        }
        return damageAmount;
    }
    public void sideEffect(Character character, float damageInflicted)
    {
        
    }
}
public class DamageBULLET : Damage, IDamage
{
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.BULLET;
    }
}
public class DamageFREEZE : Damage, IDamage
{
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.FREEZE;
    }
}
public class DamageICE : Damage, IDamage
{
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.ICE;
    }
}
public class DamageWIND : Damage, IDamage
{
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.WIND;
    }
    public new void sideEffect(Character character, float damageInflicted)
    {
        character.sf_freeze(damageInflicted);
    }
}
public class DamageCARCRUSH : Damage, IDamage
{
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.CARCRUSH;
    }
    public new void sideEffect(Character character, float damageInflicted)
    {
        character.sf_concuss(damageInflicted);
    }
}
public class DamageRAIN : Damage, IDamage
{
    public DamageAgent getDamageAgent()
    {
        return DamageAgent.RAIN;
    }
}