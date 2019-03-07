using UnityEngine;


public class Menu_game : Menu_generic {
    [SerializeField] GameObject settingButton;

    public override void turnOn()
    {
        base.turnOn();
        settingButton.SetActive(false);
    }
    public override void turnOff()
    {
        base.turnOff();
        settingButton.SetActive(true);
    }
}
