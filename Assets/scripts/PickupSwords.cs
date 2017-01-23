using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Assign this script to all sword pieces.
*/
public class PickupSwords : MonoBehaviour {
	private SpriteRenderer sr;
	private CircleCollider2D cc;
	private GameObject astronaut;


	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer> ();
		cc = gameObject.GetComponent<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.name == "SpaceBabe") {
			//Destroy (gameObject);
			sr.enabled = false;
			cc.enabled = false;
		}
	}

	public void Respawn(){
		sr.enabled = true;
		cc.enabled = true;
	}
}
