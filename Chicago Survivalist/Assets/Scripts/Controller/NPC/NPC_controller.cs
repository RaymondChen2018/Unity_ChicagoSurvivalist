using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_controller : MovementController, IController
{
    // Use this for initialization
    void Start () {
        RB = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //AI
    public void obtainMoveToPos()
    {
        //
    }
}
