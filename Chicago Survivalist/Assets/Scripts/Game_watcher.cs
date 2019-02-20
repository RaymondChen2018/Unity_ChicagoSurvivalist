using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_watcher : MonoBehaviour {
    public static Game_watcher singleton;
    public float miles;
    private Vector3 start_position;
    private Vector3 end_position;


	public void set_start_end(Vector3 start, Vector3 end)
    {
        start_position = start;
        end_position = end;
    }
}
