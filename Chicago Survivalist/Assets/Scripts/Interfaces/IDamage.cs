using UnityEngine;

public enum DamageAgent
{
    BULLET, FREEZE, ICE, WIND, CARCRUSH, RAIN
}

public interface IDamage
{
    DamageAgent getDamageAgent();
    void sideEffect(Character character, float damageInflicted);
}
public abstract class Damage
{
    public static DamageBULLET dmgBulletSingleton;
    public static DamageCARCRUSH dmgCarCrushSingleton;
    public static DamageFREEZE dmgFreezeSingleton;
    public static DamageICE dmgIceSingleton;
    public static DamageRAIN dmgRainSingleton;
    public static DamageWIND dmgWindSingleton;

    public float damageAmount;
    public Vector2 damageDirection;
    protected Damage()
    {
        dmgBulletSingleton = new DamageBULLET();
        dmgCarCrushSingleton = new DamageCARCRUSH();
        dmgFreezeSingleton = new DamageFREEZE();
        dmgIceSingleton = new DamageICE();
        dmgRainSingleton = new DamageRAIN();
        dmgWindSingleton = new DamageWIND();
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
    public new void sideEffect(Character character, float damageInflicted)
    {
        character.sf_freeze(damageInflicted);
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