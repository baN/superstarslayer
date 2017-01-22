using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astronaut_controls : MonoBehaviour {
	private bool _isJumping;
	private Transform target; //next planet to jump to


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
			transform.position = Vector3.MoveTowards (transform.position, target.position, .10f);
		}


	}

	void OnCollisionEnter (Collision col)
	{
		_isJumping = false;
		Debug.Log ("Stopped jumping");
	}

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
