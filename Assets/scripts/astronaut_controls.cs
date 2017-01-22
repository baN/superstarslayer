using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astronaut_controls : MonoBehaviour {
	public float speed;
	private bool _isJumping;
	private Transform target; //next planet to jump to

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Debug.Log ("Pressed the left mouse button!");
			Vector3 mousePos = Input.mousePosition;
			target = GetPlanetClicked (mousePos);
			_isJumping = true;
		}

		if (_isJumping) {
			Debug.Log ("Jumping");
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
			Debug.Log ("Transform.position:" + transform.position.x + "," + transform.position.y);
			Debug.Log ("TARGET.position:" + target.position.x + "," + target.position.y);
		}

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
