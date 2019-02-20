using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_equip : MonoBehaviour {

    [SerializeField] private Item item1 = Item.NONE;
    [SerializeField] private Item item2 = Item.NONE;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public bool hasArmor()
    {
        return item1 == Item.ARMOR || item1 == Item.ARMOR;
    }
    public bool hasUmbrella()
    {
        return item1 == Item.UMBRELLA || item1 == Item.UMBRELLA;
    }
    public bool hasJacket()
    {
        return item1 == Item.JACKET || item1 == Item.JACKET;
    }
    public void obtainItem(Item item)
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
