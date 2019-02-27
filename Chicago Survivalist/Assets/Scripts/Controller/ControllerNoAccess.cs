using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    /// <summary>
    /// If the character is closer than this distance, character is deemed arrived.
    /// </summary>
    const float MOVETO_POS_TOLERANCE = 0.7f;
    /// <summary>
    /// Movement speed
    /// </summary>
    [SerializeField] private float movement_speed = 50;

    //Temperary variables
    protected Vector3 velVec;
    protected Rigidbody RB;

    /// <summary>
    /// Move the character
    /// </summary>
    protected void moveTo()
    {
        if (velVec.x* velVec.x + velVec.z* velVec.z < MOVETO_POS_TOLERANCE * MOVETO_POS_TOLERANCE)
        {
            return;
        }
        velVec = velVec.normalized * movement_speed;
        velVec.y = 0;
        RB.AddForce(velVec);
        RB.rotation = Quaternion.Euler(0, -Mathf.Atan2(velVec.z, velVec.x) * 180 / 3.14f, 0);
    }
}
