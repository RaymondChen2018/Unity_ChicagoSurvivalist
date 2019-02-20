using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_hud : MonoBehaviour {
    //Singleton
    public static Player_hud singleton;

    //Cursor Render
    [SerializeField] public SpriteRenderer cursorCastSprite;
    //TrackArrow Render
    public GameObject trackArrow = null;
    Vector3 screen_intersect = Vector3.zero;
    Vector3 destinPosition;
    Vector3 thisPosition;

    //Screen Info
    static float screen_edge_contract_ratio = 0.77f;
    float bottom;
    float top;
    float left;
    float right;
    float height_world;
    float width_world;

    //Components
    Player_handler playerHandler;

    
    // Use this for initialization
    void Start () {
        singleton = this;
        playerHandler = GetComponent<Player_handler>();
    }
	
	// Update is called once per frame
	void Update () {

        Ray ray = Camera.main.ScreenPointToRay(Vector3.zero);
        bottom = Camera.main.transform.position.z - screen_edge_contract_ratio * Camera.main.transform.position.y * ray.direction.z / ray.direction.y ;
        top = Camera.main.transform.position.z + screen_edge_contract_ratio * Camera.main.transform.position.y * ray.direction.z / ray.direction.y;
        left = Camera.main.transform.position.x - screen_edge_contract_ratio * Camera.main.transform.position.y * ray.direction.x / ray.direction.y;
        right = Camera.main.transform.position.x + screen_edge_contract_ratio * Camera.main.transform.position.y * ray.direction.x / ray.direction.y;
        height_world = (top - bottom)/2;
        width_world = (right - left) / 2;


        //Calculate destination arrow
        if (Destination_trigger.singleton != null)
        {
            destinPosition = Destination_trigger.singleton.transform.position;
            thisPosition = transform.position;
            

            //Target above
            if (destinPosition.z > top)
            {
                intersection(CONSTANTS.DIRECTION.UP);
            }
            else if(destinPosition.z < bottom)
            {
                intersection(CONSTANTS.DIRECTION.DOWN);
            }
            if(destinPosition.x > right)
            {
                intersection(CONSTANTS.DIRECTION.RIGHT);
            }
            else if(destinPosition.x < left)
            {
                intersection(CONSTANTS.DIRECTION.LEFT);
            }

            //Inside view 
            if (destinPosition.z <= top && destinPosition.z >= bottom && destinPosition.x <= right && destinPosition.x >= left)
            {
                trackArrow.transform.rotation = Quaternion.Euler(90, Destination_trigger.singleton.get_entrance_dir(), 0);
                trackArrow.transform.position = Destination_trigger.singleton.transform.position;
            }
            //Outside view
            else
            {
                Vector3 diff = destinPosition - thisPosition;
                trackArrow.transform.rotation = Quaternion.Euler(90, -180 * Mathf.Atan2(diff.z, diff.x) / Mathf.PI, 0);
                trackArrow.transform.position = screen_intersect;
            }
            //Convert intersection point
            trackArrow.SetActive(true);

            

            
        }
        else
        {
            trackArrow.SetActive(false);
        }

        if (cursorCastSprite.enabled)
        {
            cursorCastSprite.transform.position = playerHandler.cursorCastPos;
        }
    }

    //If found intersection, edit value of screen_intersect
    void intersection(CONSTANTS.DIRECTION direction)//Top0, Right1, Bottom2, Left3
    {
        screen_intersect.y = 5;
        switch (direction)
        {
            case CONSTANTS.DIRECTION.UP://Top
                {
                    
                    float x = (destinPosition.x - thisPosition.x) * (height_world) / (destinPosition.z - thisPosition.z) + thisPosition.x;
                    //Debug.Log("locate x: " + x + "; left:"+left+"; right: "+right);
                    if (x >= left && x <= right)
                    {
                        //Debug.Log("locate: " + screen_intersect);
                        screen_intersect.x = x;
                        screen_intersect.z = top;
                    }
                    break;
                }
            case CONSTANTS.DIRECTION.RIGHT://Right
                {
                    float z = (destinPosition.z - thisPosition.z) * (width_world) / (destinPosition.x - thisPosition.x) + thisPosition.z;
                    if (z >= bottom && z <= top)
                    {
                        //Debug.Log("locate: " + screen_intersect);
                        screen_intersect.x = right;
                        screen_intersect.z = z;
                    }
                    break;
                }
            case CONSTANTS.DIRECTION.DOWN://Bottom
                {
                    float x = (destinPosition.x - thisPosition.x) * (-height_world) / (destinPosition.z - thisPosition.z) + thisPosition.x;
                    if (x >= left && x <= right)
                    {
                        //Debug.Log("locate: " + screen_intersect);
                        screen_intersect.x = x;
                        screen_intersect.z = bottom;
                    }
                    break;
                }
            case CONSTANTS.DIRECTION.LEFT://Left
                {
                    float z = (destinPosition.z - thisPosition.z) * (-width_world) / (destinPosition.x - thisPosition.x) + thisPosition.z;
                    if (z >= bottom && z <= top)
                    {
                        //Debug.Log("locate: " + screen_intersect);
                        screen_intersect.x = left;
                        screen_intersect.z = z;
                    }
                    break;
                }
        }
    }

    public void setCursorOn(bool isOn)
    {
        cursorCastSprite.enabled = isOn;
    }
}
