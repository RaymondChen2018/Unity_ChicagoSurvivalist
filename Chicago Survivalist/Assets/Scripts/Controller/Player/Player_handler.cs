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

        //While key down
        if(Input.GetKey(ScreenPress))
        {
            obtainMoveToPos();
            moveTo();
        }
	}
    
    public void obtainMoveToPos()
    {
        getCursorCast();
        moveto = cursorCastPos - transform.position;
    }


    //Obtain cursor cast position
    void getCursorCast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 500, cursorCastMask);
        cursorCastPos = hit.point;
    }
}
