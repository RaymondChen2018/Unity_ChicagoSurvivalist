using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Controller for individual AI.
/// If it is a civilian, behaves independantly.
/// If it is a gang mob, operates in squads(submit control to a squad entity) against other gang squads
/// </summary>
public class NPC_controller : MovementController, IController
{
    /// <summary>
    /// If a civilian
    /// </summary>
    [SerializeField] protected bool Civilian  = true;

    //Squad control
    private Cluster_controller squad = null;
    private Vector3 velVecCluster;

    void Start () {
        RB = GetComponent<Rigidbody>();
    }
	void Update () {
        
	}

    /// <summary>
    /// Use AI to find out where this npc moves toward;
    /// </summary>
    void analyzMoveTo()
    {
        if(squad != null)
        {
            velVec = velVecCluster;
        }
        else
        {
            //To-do: AI
        }
    }
    /// <summary>
    /// Subscribe this AI and submit its control to a squad entity.
    /// </summary>
    /// <param name="_cluster">The squad entity to subscribe to</param>
    public void RegisterCluster(Cluster_controller _cluster)
    {
        squad = _cluster;
    }
    /// <summary>
    /// Unsubscribe this AI and regain its control.
    /// </summary>
    /// <param name="_cluster">The squad entity to subscribe to</param>
    public void DeregisterCluster()
    {
        squad.deregister(this);
        squad = null;
    }
    /// <summary>
    /// Is this AI controlled by a squad?
    /// </summary>
    /// <returns></returns>
    public bool hasSquad()
    {
        return squad != null;
    }
}
