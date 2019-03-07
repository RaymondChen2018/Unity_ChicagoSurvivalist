using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_hud : MonoBehaviour {

    //Cursor Render
    [SerializeField] public SpriteRenderer cursorCastSprite;

    //TrackArrow Render
    public GameObject trackArrow = null;
    Vector3 screen_intersect = Vector3.zero;
    Vector3 destinPosition;
    Vector3 thisPosition;

    //Screen Info
    const float screen_edge_contract_ratio = 0.77f;
    float lowerCamViewBound;
    float topCamViewBound;
    float leftCamViewBound;
    float rightCamViewBound;
    float heightCamViewBoundHalved;
    float widthCamViewBoundHalved;

    //Screen Fx
    [SerializeField] private Image FreezeScreen;
    /// <summary>
    /// Point where a sheet frozen screen starts fading in as temperature decreases further;
    /// </summary>
    const float FREEZE_START_THREDSHOLD = 5;
    /// <summary>
    /// Point where a sheet frozen screen will reach its max influence;
    /// </summary>
    const float FREEZE_END_THREDSHOLD = -20;
    [SerializeField] private Image InjuriedScreen;
    /// <summary>
    /// Health where a sheet bleed screen starts fading in as health drops further;
    /// </summary>
    const float INJURIED_START_THREDSHOLD = 50;

    //Menu
    [SerializeField] private Death_Panel Menu_Death;
    

    //HUD
    [SerializeField] private Text HUD_Temperature_Main;
    [SerializeField] private Text HUD_Temperature_P;
    [SerializeField] private Text HUD_Temperature_Point;
    [SerializeField] private Text HUD_Temperature_Sign;
    [SerializeField] private Sprite[] itemIcons;
    [SerializeField] private Image[] equipIcons;

    //Components
    Player_handler playerHandler;
    Player_status playerStatus;

    //Temperary variables
    Color tempColor;

    // Use this for initialization
    void Start () {
        playerHandler = GetComponent<Player_handler>();
        playerStatus = GetComponent<Player_status>();
    }
	
	// Update is called once per frame
	void Update () {
        //Update HUD temperature
        setHUDTemperature();

        //Update camera bound
        setCamBoundStat();

        //Calculate destination arrow
        setDestineArrow();

        //Cursor cast
        if (cursorCastSprite.enabled)
        {
            cursorCastSprite.transform.position = playerHandler.cursorCastPos;
        }

        //Screen Fx
        setInjuriedScreen();
        setFreezeScreen();
        setConcussionScreen();
    }
    /// <summary>
    /// Shows weather temperature to the HUD display
    /// </summary>
    private void setHUDTemperature()
    {
        int weatherTemperature_main = (int)Weather.getTemperature();
        int weatherTemperature_point = (int)Mathf.Abs((Weather.getTemperature() - weatherTemperature_main)*10);
        HUD_Temperature_Main.text = (weatherTemperature_main).ToString();
        HUD_Temperature_Point.text = weatherTemperature_point.ToString();
        if(weatherTemperature_main < Character.FREEZE_START_THREDSHOLD)
        {
            HUD_Temperature_Main.color = Color.red;
            HUD_Temperature_P.color = Color.red;
            HUD_Temperature_Point.color = Color.red;
            HUD_Temperature_Sign.color = Color.red;
        }
        else
        {
            HUD_Temperature_Main.color = Color.cyan;
            HUD_Temperature_P.color = Color.cyan;
            HUD_Temperature_Point.color = Color.cyan;
            HUD_Temperature_Sign.color = Color.cyan;
        }
    }
    /// <summary>
    /// Determine if display concussion effeect
    /// </summary>
    private void setConcussionScreen()
    {
        if(playerStatus.getConcussionTimer() > 0)
        {
            //Concussion on
        }
        else
        {
            //Concussion off
        }
    }
    /// <summary>
    /// Displays freeze screen effect when temperature is low
    /// </summary>
    private void setFreezeScreen()
    {
        float temperature = playerStatus.getTemperature();
        if(temperature < FREEZE_START_THREDSHOLD)
        {
            tempColor = FreezeScreen.color;
            tempColor.a = Mathf.Lerp(0, 1, (FREEZE_START_THREDSHOLD - temperature)/(FREEZE_START_THREDSHOLD - FREEZE_END_THREDSHOLD));
            FreezeScreen.color = tempColor;
        }
    }
    /// <summary>
    /// Display bleed screen when the player's health is low
    /// </summary>
    private void setInjuriedScreen()
    {
        float health = playerStatus.getHealth();
        if (health < INJURIED_START_THREDSHOLD)
        {
            tempColor = InjuriedScreen.color;
            tempColor.a = Mathf.Lerp(0, 1, 1 - health/INJURIED_START_THREDSHOLD);
            InjuriedScreen.color = tempColor;
        }
    }

    /// <summary>
    /// To do: Make this part more understandable;
    /// Determines the screen coordinate that the destination arrow is set at
    /// And store the value at a local variable
    /// </summary>
    /// <param name="direction">Calculate which direction?</param>
    void intersection(CONSTANTS.DIRECTION direction)
    {
        screen_intersect.y = 5;
        switch (direction)
        {
        case CONSTANTS.DIRECTION.UP://Top    
                float x = (destinPosition.x - thisPosition.x) * (heightCamViewBoundHalved) / (destinPosition.z - thisPosition.z) + thisPosition.x;
                if (x >= leftCamViewBound && x <= rightCamViewBound)
                {
                    screen_intersect.x = x;
                    screen_intersect.z = topCamViewBound;
                }
                break;
        case CONSTANTS.DIRECTION.RIGHT://Right
                float z = (destinPosition.z - thisPosition.z) * (widthCamViewBoundHalved) / (destinPosition.x - thisPosition.x) + thisPosition.z;
                if (z >= lowerCamViewBound && z <= topCamViewBound)
                {
                    screen_intersect.x = rightCamViewBound;
                    screen_intersect.z = z;
                }
                break;
        case CONSTANTS.DIRECTION.DOWN://Bottom
                x = (destinPosition.x - thisPosition.x) * (-heightCamViewBoundHalved) / (destinPosition.z - thisPosition.z) + thisPosition.x;
                if (x >= leftCamViewBound && x <= rightCamViewBound)
                {
                    screen_intersect.x = x;
                    screen_intersect.z = lowerCamViewBound;
                }
                break;
        case CONSTANTS.DIRECTION.LEFT://Left
                z = (destinPosition.z - thisPosition.z) * (-widthCamViewBoundHalved) / (destinPosition.x - thisPosition.x) + thisPosition.z;
                if (z >= lowerCamViewBound && z <= topCamViewBound)
                {
                    screen_intersect.x = leftCamViewBound;
                    screen_intersect.z = z;
                }
                break;
        }
    }

    /// <summary>
    /// Determines the camera view bound coordinates and store them in variables
    /// </summary>
    void setCamBoundStat()
    {
        Ray ray = Camera.main.ScreenPointToRay(Vector3.zero);
        lowerCamViewBound = Camera.main.transform.position.z - screen_edge_contract_ratio * Camera.main.transform.position.y * ray.direction.z / ray.direction.y;
        topCamViewBound = Camera.main.transform.position.z + screen_edge_contract_ratio * Camera.main.transform.position.y * ray.direction.z / ray.direction.y;
        leftCamViewBound = Camera.main.transform.position.x - screen_edge_contract_ratio * Camera.main.transform.position.y * ray.direction.x / ray.direction.y;
        rightCamViewBound = Camera.main.transform.position.x + screen_edge_contract_ratio * Camera.main.transform.position.y * ray.direction.x / ray.direction.y;
        heightCamViewBoundHalved = (topCamViewBound - lowerCamViewBound) / 2;
        widthCamViewBoundHalved = (rightCamViewBound - leftCamViewBound) / 2;
    }
    /// <summary>
    /// Display destination arrow
    /// </summary>
    void setDestineArrow()
    {
        destinPosition = Game_watcher.getDestinationPos();
        thisPosition = transform.position;
        //Calculate the intersection b/w cam-to-destine line and the screen bounds
        if (destinPosition.z > topCamViewBound)
        {
            intersection(CONSTANTS.DIRECTION.UP);
        }
        else if (destinPosition.z < lowerCamViewBound)
        {
            intersection(CONSTANTS.DIRECTION.DOWN);
        }
        if (destinPosition.x > rightCamViewBound)
        {
            intersection(CONSTANTS.DIRECTION.RIGHT);
        }
        else if (destinPosition.x < leftCamViewBound)
        {
            intersection(CONSTANTS.DIRECTION.LEFT);
        }
        //Destination inside camera view
        if (destinPosition.z <= topCamViewBound && destinPosition.z >= lowerCamViewBound && destinPosition.x <= rightCamViewBound && destinPosition.x >= leftCamViewBound)
        {
            trackArrow.transform.rotation = Quaternion.Euler(90, Game_watcher.getDestinationDir(), 0);
            trackArrow.transform.position = Game_watcher.getDestinationPos();
        }
        //Destination outside camera view
        else
        {
            Vector3 diff = destinPosition - thisPosition;
            trackArrow.transform.rotation = Quaternion.Euler(90, -180 * Mathf.Atan2(diff.z, diff.x) / Mathf.PI, 0);
            trackArrow.transform.position = screen_intersect;
        }
    }
    /// <summary>
    /// Set equip icon
    /// </summary>
    /// <param name="itemIndex">Item1 = 0; Item2 = 1</param>
    /// <param name="item">The type of item to equip</param>
    /// <param name="isHint">When player approaches item, shows as hint(half faded icon)</param>
    public void setEquip(byte itemIndex, Item item, bool isHint)
    {
        Image itemIcon = equipIcons[itemIndex];
        itemIcon.sprite = itemIcons[(int)item];
        if (isHint)
        {
            itemIcon.color = Color.red;
        }
        else
        {
            itemIcon.color = Color.white;
        }
    }

    /// <summary>
    /// Switch on/off cursor
    /// </summary>
    /// <param name="isOn"></param>
    public void setCursorOn(bool isOn)
    {
        cursorCastSprite.enabled = isOn;
    }
    /// <summary>
    /// Call back when player wins the game
    /// </summary>
    internal void _triggerFinishMiles()
    {
        
    }
    /// <summary>
    /// Call back when player dies
    /// </summary>
    /// <param name="agent">The cause the player died of</param>
    internal void _triggerDeath(DamageAgent agent)
    {
        Menu_Death.showDeathScreen(agent);    
    }
}
