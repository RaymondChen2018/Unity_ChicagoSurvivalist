using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// An object that never gets deleted
/// </summary>
public class Overseer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
