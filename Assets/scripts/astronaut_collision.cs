using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astronaut_collision : MonoBehaviour {

	void OnCollisionEnter (Collision col){
		Debug.Log ("Collision detected!");
	}
	 
}
