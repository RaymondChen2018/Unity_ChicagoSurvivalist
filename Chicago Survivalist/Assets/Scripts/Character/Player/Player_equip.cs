using UnityEngine;

/// <summary>
/// Player inventory
/// </summary>
public class Player_equip : MonoBehaviour {

    /// <summary>
    /// Inventory slot 1/2
    /// </summary>
    [SerializeField] private Item item1 = Item.NONE;
    /// <summary>
    /// Inventory slot 2/2
    /// </summary>
    [SerializeField] private Item item2 = Item.NONE;
	
    /// <summary>
    /// Does the player has armor?
    /// </summary>
    /// <returns></returns>
    public bool hasArmor()
    {
        return item1 == Item.ARMOR || item1 == Item.ARMOR;
    }
    /// <summary>
    /// Does the player has umbrella?
    /// </summary>
    /// <returns></returns>
    public bool hasUmbrella()
    {
        return item1 == Item.UMBRELLA || item1 == Item.UMBRELLA;
    }
    /// <summary>
    /// Does the player has jacket?
    /// </summary>
    /// <returns></returns>
    public bool hasJacket()
    {
        return item1 == Item.JACKET || item1 == Item.JACKET;
    }
    /// <summary>
    /// Pick up a new item
    /// </summary>
    /// <param name="item"></param>
    public void pickUpItem(Item item)
    {
        if (item1 == Item.NONE)
        {
            item1 = item;

        }
        else if (item2 == Item.NONE)
        {
            item2 = item;

        }
    }
}
