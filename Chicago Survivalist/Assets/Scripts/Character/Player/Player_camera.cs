using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Take cares of Player camera work
/// </summary>
public class Player_camera : MonoBehaviour {
    public Camera playerCam;
    Vector3 campos;

	// Update is called once per frame
	void Update () {
        //Camera follow
        campos = transform.position;
        campos.y = playerCam.transform.position.y;
        playerCam.transform.position = campos;
    }
}
