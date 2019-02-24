using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_watcher : MonoBehaviour {
    public static Game_watcher singleton;
    [SerializeField] private Player_handler PlayerObject;
    [SerializeField] private Destination_trigger Destination;
    static Map_generator Map_generator;
    public float miles;
    private Vector3 start_position;
    private Vector3 end_position;

    void Start()
    {
        singleton = this;
        Map_generator = GetComponent<Map_generator>();
        Map_generator.map_generate();
    }
    public static void OnReachDestination(float distanceTravelled)
    {
        //If miles haven't been reached, generate next map
        singleton.miles += distanceTravelled;
        Map_generator.map_generate();
        //If reached, trigger stat board + replay
    }

    public void Reset()
    {
        miles = 0;
        Map_generator.Reset();
    }

    public static void setPlayerPos(Vector3 pos)
    {
        singleton.PlayerObject.transform.position = pos;
    }
    public static Vector3 getDestinationPos()
    {
        return singleton.Destination.transform.position;
    }
    public static float getDestinationDir()
    {
        return singleton.Destination.getDestinationDir();
    }
}
