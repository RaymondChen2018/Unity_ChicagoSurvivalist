using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_handler : MonoBehaviour {
    //Key input
    [SerializeField] private KeyCode ScreenPress;
    [SerializeField] public LayerMask cursorCastMask;
    //Movement
    [SerializeField] private float movement_speed = 10;


    //Local variables
    Rigidbody RB;
    Vector3 moveto;
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
            obtainCursorCast();
            moveTo();
        }
	}
    //Obtain cursor cast position
    void obtainCursorCast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 500, cursorCastMask);
        cursorCastPos = hit.point;
    }
    //Movement
    void moveTo()
    {
        moveto = cursorCastPos - transform.position;
        moveto = moveto.normalized * movement_speed;
        moveto.y = 0;
        RB.AddForce(moveto);
        RB.rotation = Quaternion.Euler(0, -Mathf.Atan2(moveto.z, moveto.x) * 180 / 3.14f, 0);
    }
}
