using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ICharacterStat
{
    [SerializeField] protected float health = 100;
    [SerializeField] protected float bodyTemperature = 27;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// Concussion reaction regard to damage infliction
    /// </summary>
    /// <param name="damageReference">Damage dealt</param>
    public void sf_concuss(float damageReference)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
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
    public CharacterType getType()
    {
        return CharacterType.PLAYER;
    }

    public void damage(IDamage damage)
    {
        
    }
}
