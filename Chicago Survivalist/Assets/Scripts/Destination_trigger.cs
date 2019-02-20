using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination_trigger : MonoBehaviour {
    
    public LayerMask mask;
    public bool levelPass = false;
    public static Destination_trigger singleton;
    float entrance_direction = 0;

	// Use this for initialization
	void Start () {
        singleton = this;
        GetComponent<MeshRenderer>().enabled = false;	
	}
    public void set_entrance_dir(CONSTANTS.DIRECTION dir)
    {
        switch (dir)
        {
            case CONSTANTS.DIRECTION.UP:
                entrance_direction = 90;
                break;
            case CONSTANTS.DIRECTION.DOWN:
                entrance_direction = -90;
                break;
            case CONSTANTS.DIRECTION.RIGHT:
                entrance_direction = 180;
                break;
            case CONSTANTS.DIRECTION.LEFT:
                entrance_direction = 0;
                break;
        }

    }
    public float get_entrance_dir()
    {
        return entrance_direction;
    }
    private void OnTriggerEnter(Collider other)
    {
        //If miles haven't been reached, generate next map
        //Game_watcher.singleton.miles += transform.position.y - 
        Map_generator.singleton.map_generate();
        
        //If reached, trigger stat board + replay

    }
}
