using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_watcher : MonoBehaviour {
    public static Game_watcher singleton;
    [SerializeField] private Player_handler PlayerObject;
    [SerializeField] private Destination_trigger Destination;
    static Map_generator Map_generator;
    public float miles;

    private const float MILES_REQUIREMENT = 1000;

    void Start()
    {
        singleton = this;
        Damage.damageInitialize();
        Map_generator = GetComponent<Map_generator>();
        Map_generator.map_generate();
    }
    public void ResetGame()
    {
        miles = 0;
        Map_generator.ResetGame();
    }

    public static void OnReachDestination(float distanceTravelled)
    {
        singleton.miles += distanceTravelled;
        float milesTravelled = singleton.miles;

        if (milesTravelled < MILES_REQUIREMENT)
        {
            //If miles haven't been reached, generate next map
            Map_generator.map_generate();
        }
        else
        {
            //If reached, trigger stat board + replay
            singleton.PlayerObject.triggerFinishMiles();
        }
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

    public void quitGame()
    {
        SceneManager.LoadScene("Menu_scene");
    }
}
