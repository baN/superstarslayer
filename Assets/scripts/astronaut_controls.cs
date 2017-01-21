using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astronaut_controls : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Debug.Log ("Pressed the left mouse button!");
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 100)) {
				Debug.Log (hit.transform.gameObject.name);
			}

		}
			
	}


	private Transform GetNearestPlanet(Transform[] planets){
		Transform tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;

		foreach (Transform t in planets) {
			//distance between current planet and astronaut
			float dist = Vector3.Distance (t.position, currentPos);
			if (dist < minDist) {
				tMin = t; //new nearest planet
				minDist = dist; //new nearest distance
			}
		}
		return tMin; //return nearest planet
	}
}
