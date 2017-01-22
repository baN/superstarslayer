using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astronaut_controls : MonoBehaviour {
	public bool _hasBladeTop;
	public bool _hasBladeLow;
	public bool _hasHilt;
	private bool _isJumping;
	private bool died = false;
	private Transform target; //next planet to jump to
	private Transform darkside; //should always be going to the darkside unless jumping.
	private Vector3 initialPosition; 


	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (died) {
			ResetSwordCollection ();
			Invisible (true);
			ResetPositionToInitial ();
			died = false;
			_isJumping = false;
			target = null;
			darkside = null;
			MoveTo (transform.position, initialPosition);

		}
		if (Input.GetMouseButton (0)) {
			Vector2 mousePos = Input.mousePosition;
			target = GetPlanetClicked (mousePos);
			_isJumping = true;
		}
		if (_isJumping && target) {
			//planet is selected, start intial jump towards that planet
			darkside = GetDarkside (target);
			MoveTo(transform.position, target.position);
		}
		if (darkside) {
			//go to the darkside if planet parent is selected.
			MoveTo(transform.position, darkside.position);
		}


	}

	//when astronaut hits something 2D.
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.name == "UVBullet(Clone)") {
			Debug.Log ("Astronaut was hit by a UVWave");
			died = true;
		}
		else {
			_isJumping = false; //stop going to the planet
			Debug.Log ("Stopped jumping");
		}
	}

	public void MoveTo(Vector3 source, Vector3 destination){
		transform.position = Vector2.MoveTowards (source, destination, .10f);
	}

	public void ResetSwordCollection(){
		_hasBladeLow = false;
		_hasBladeTop = false;
		_hasHilt = false;
	}

	public void ResetPositionToInitial(){
		transform.position = initialPosition;
	}

	public void Invisible(bool invis){
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer> ();
		if (sr != null)
			sr.enabled = invis;
	}

	private Transform GetDarkside(Transform planet){
		return planet.FindChild ("planet_darkside");
	}

	//Returns the object clicked on
	private Transform GetPlanetClicked(Vector2 mousePos){
		Vector3 ray = Camera.main.ScreenToWorldPoint (mousePos);
		RaycastHit2D hit = Physics2D.Raycast (ray, Vector2.zero);
		if (hit.collider != null) {
			Debug.Log (hit.collider.gameObject.name);
			return hit.transform;
		}
		return null;
	}
		
}
