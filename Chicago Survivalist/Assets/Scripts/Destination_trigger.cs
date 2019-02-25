using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination_trigger : MonoBehaviour {
    
    float distanceTraveled = 0;
    float entrance_direction = 0;
    // Use this for initialization
    void Start () {
        GetComponent<MeshRenderer>().enabled = false;	
        
	}

    public void setEntranceDir(CONSTANTS.DIRECTION dir)
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
    public float getDestinationDir()
    {
        return entrance_direction;
    }
    private void OnTriggerEnter(Collider other)
    {
        Game_watcher.OnReachDestination(Map_generator.getDistanceTravelled());
    }
}
