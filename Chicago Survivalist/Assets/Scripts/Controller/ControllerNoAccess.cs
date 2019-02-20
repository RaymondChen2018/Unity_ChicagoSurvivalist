using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //Movement
    [SerializeField] private float movement_speed = 50;

    protected Vector3 moveto;
    protected Rigidbody RB;

    //Movement
    protected void moveTo()
    {
        moveto = moveto.normalized * movement_speed;
        moveto.y = 0;
        RB.AddForce(moveto);
        RB.rotation = Quaternion.Euler(0, -Mathf.Atan2(moveto.z, moveto.x) * 180 / 3.14f, 0);
    }

}
