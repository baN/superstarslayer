﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Assign this script to all sword pieces.
*/
public class PickupSwords : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.name == "SpaceBabe")
			Destroy (gameObject);
	}
}