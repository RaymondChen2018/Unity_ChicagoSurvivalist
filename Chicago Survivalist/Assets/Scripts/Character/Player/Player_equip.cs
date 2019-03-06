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
    Player_hud playerHUD;

    void Start()
    {
        playerHUD = GetComponent<Player_hud>();    
    }

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
            playerHUD.setEquip(0, item, false);
        }
        else if (item2 == Item.NONE)
        {
            item2 = item;
            playerHUD.setEquip(1, item, false);
        }
    }

    public void approachItem(Item item)
    {
        if (item1 == Item.NONE)
        {
            item1 = item;
            playerHUD.setEquip(0, item, false);
        }
        else if (item2 == Item.NONE)
        {
            item2 = item;
            playerHUD.setEquip(1, item, false);
        }
    }


    sbyte getAvailableSlot()
    {
        if (item1 == Item.NONE)
        {
            return 0;
        }
        else if (item2 == Item.NONE)
        {
            return 1;
        }
        return -1;
    }
}
