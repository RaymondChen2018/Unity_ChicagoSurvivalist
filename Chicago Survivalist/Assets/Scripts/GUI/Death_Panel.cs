using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Death_Panel : Panel_generic {

    [SerializeField] private Image Menu_Death_Freeze;
    [SerializeField] private Image Menu_Death_Ice;
    [SerializeField] private Image Menu_Death_Bullet;
    [SerializeField] private Image Menu_Death_Wind;
    [SerializeField] private Image Menu_Death_CarCrush;
    [SerializeField] private Image Menu_Death_Rain;

    public void showDeathScreen(DamageAgent agent)
    {
        base.turnOn();
        switch (agent)
        {
            case DamageAgent.BULLET:
                Menu_Death_Bullet.enabled = true;
                break;
            case DamageAgent.FREEZE:
                Menu_Death_Freeze.enabled = true;
                break;
            case DamageAgent.ICE:
                Menu_Death_Ice.enabled = true;
                break;
            case DamageAgent.RAIN:
                Menu_Death_Rain.enabled = true;
                break;
            case DamageAgent.CARCRUSH:
                Menu_Death_CarCrush.enabled = true;
                break;
            case DamageAgent.WIND:
                Menu_Death_Wind.enabled = true;
                break;
        }
    }
    public void hideDeathScreen()
    {
        base.turnOff();
        Menu_Death_Freeze.enabled = false;
        Menu_Death_Ice.enabled = false;
        Menu_Death_Bullet.enabled = false;
        Menu_Death_Wind.enabled = false;
        Menu_Death_CarCrush.enabled = false;
        Menu_Death_Rain.enabled = false;
    }

}
