using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astronaut_controls : MonoBehaviour {
	private bool _isJumping;
	private bool _isDarkside;
	private Transform target; //next planet to jump to
	private Transform darkside;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Vector3 mousePos = Input.mousePosition;
			target = GetPlanetClicked (mousePos);
			_isJumping = true;
		}
		if (_isJumping && target) {
			darkside = GetDarkside (target);
			transform.position = Vector3.MoveTowards (transform.position, target.position, .10f);
		}
		if (_isDarkside && darkside) {
			//go to the darkside
			transform.position = Vector3.MoveTowards (transform.position, darkside.position, .10f);
		}


	}

	//when astronaut lands on object (planet)
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name == "planet_darkside") {
			_isDarkside = false;
			Debug.Log ("hit the darkside of the planet");
		} else {
			_isJumping = false; //stop going to the planet
			_isDarkside = true; //start moving to the darkside
			Debug.Log ("Stopped jumping");
		}
	}

	private Transform GetDarkside(Transform planet){
		return planet.FindChild ("planet_darkside");
	}

	//Returns the object clicked on
	private Transform GetPlanetClicked(Vector3 mousePos){
		Ray ray = Camera.main.ScreenPointToRay (mousePos);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.Log (hit.transform.gameObject.name);
			return hit.transform;
		}
		return null;
	}
		
}
