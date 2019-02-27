using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_handler : MovementController, IController
{
    //Key input
    [SerializeField] private KeyCode ScreenPress;
    [SerializeField] public LayerMask cursorCastMask;

    //Local variables
    [HideInInspector] public Vector3 cursorCastPos;
    private bool Deceased = false;
    
    //Components
    Player_status playerStatus;
    Player_equip playerEquip;
    Player_hud playerHUD;

    // Use this for initialization
    void Start () {
        RB = GetComponent<Rigidbody>();
        playerStatus = GetComponent<Player_status>();
        playerHUD = GetComponent<Player_hud>();
        playerEquip = GetComponent<Player_equip>();
    }

    // Update is called once per frame
    void Update () {
        //On key down
        if (Input.GetKeyDown(ScreenPress))
        {
            playerHUD.setCursorOn(true);
        }
        //On key release
        else if (Input.GetKeyUp(ScreenPress))
        {
            playerHUD.setCursorOn(false);
        }

        //Move
        if(!Deceased && Input.GetKey(ScreenPress))
        {
            
            analyzMoveTo();
            moveTo();
        }
    }

    /// <summary>
    /// Player is not in any squad
    /// </summary>
    /// <returns></returns>
    public bool hasSquad()
    {
        return false;
    }
    /// <summary>
    /// Call back when player wins
    /// </summary>
    public void _triggerFinishMiles()
    {
        playerHUD._triggerFinishMiles();
    }
    /// <summary>
    /// Call back when player dies
    /// </summary>
    /// <param name="agent">The cause the player died of</param>
    public void _triggerDeath(DamageAgent agent)
    {
        Deceased = true;
        playerHUD._triggerDeath(agent);
    }
    /// <summary>
    /// Call back when the player revives
    /// </summary>
    public void _triggerRevive()
    {
        Deceased = false;
    }
    /// <summary>
    /// Player moves toward cursor position;
    /// </summary>
    void analyzMoveTo()
    {
        getCursorCast();
        velVec = cursorCastPos - transform.position;
    }
    /// <summary>
    /// get the mouse coordinate on ground in (x,y) and store it at a local variable
    /// </summary>
    void getCursorCast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 500, cursorCastMask);
        cursorCastPos = hit.point;
    }
}
