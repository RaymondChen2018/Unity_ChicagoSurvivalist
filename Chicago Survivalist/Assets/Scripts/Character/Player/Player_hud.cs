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

    private void setFreezeScreen()
    {
        float temperature = playerStatus.getTemperature();
        if(temperature < FREEZE_START_THREDSHOLD)
        {
            tempColor = FreezeScreen.color;
            tempColor.a = Mathf.Lerp(FREEZE_START_THREDSHOLD, FREEZE_END_THREDSHOLD, temperature);
            FreezeScreen.color = tempColor;
        }
    }
    private void setInjuriedScreen()
    {
        float health = playerStatus.getHealth();
        if (health < INJURIED_START_THREDSHOLD)
        {
            tempColor = InjuriedScreen.color;
            tempColor.a = Mathf.Lerp(INJURIED_START_THREDSHOLD, 0, health);
            InjuriedScreen.color = tempColor;
        }
    }



    //If found intersection, edit value of screen_intersect
    void intersection(CONSTANTS.DIRECTION direction)//Top0, Right1, Bottom2, Left3
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

    public void setCursorOn(bool isOn)
    {
        cursorCastSprite.enabled = isOn;
    }

    internal void triggerFinishMiles()
    {
        
    }
    internal void triggerDeath()
    {

    }
}
