using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astronaut_controls : MonoBehaviour {
	public bool _hasBladeTop = false;
	public bool _hasBladeLow = false;
	public bool _hasHilt = false;
	public bool _beastMode = false; //redundant if has all the pieces.
	private bool _isJumping;
	private bool died = false;
	private Transform target; //next planet to jump to
	private Transform darkside; //should always be going to the darkside unless jumping.
	private Vector3 initialPosition; 
	private AudioSource[] sounds;
	private AudioSource deathSound;
	private AudioSource jumpSound;
	private AudioSource swordCollectedSound;


	// Use this for initialization
	void Start () {
		//set astronauts initial position saved so when she dies, it resets here.
		initialPosition = transform.position;
		sounds = GetComponents <AudioSource>();
		deathSound = sounds [0];
		jumpSound = sounds [1];
		swordCollectedSound = sounds [2];
	}

	// Update is called once per frame
	void Update () {
		if (died) {
			//lose all sword pieces, turn off display, return to original position, don't move anywhere, reset targets.
			ResetSwordCollection ();
			PlayDeathSound ();
			Invisible (true);
			ResetPositionToInitial ();
			died = false;
			_isJumping = false;
			target = null;
			darkside = null;
			MoveTo (transform.position, initialPosition);

		}
		if (_hasBladeTop && _hasBladeLow && _hasHilt) {
			//Defeat the sun stage, switch the sprite to holding the sword
			_beastMode = true;


		}
		if (Input.GetMouseButton (0)) {
			Vector2 mousePos = Input.mousePosition;
			target = GetPlanetClicked (mousePos);
			_isJumping = true;
			PlayJumpingSound ();
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
		//-----TODO: Toby, please fill in the TODO items below
		if (col.gameObject.name == "sword_top") {
			PlaySwordCollectedSound ();
			_hasBladeTop = true;
			Debug.Log ("Has sword_top");
		}
		if (col.gameObject.name == "sword_low") {
			PlaySwordCollectedSound ();
			_hasBladeLow = true;
			Debug.Log ("Has sword_low");
		}
		if (col.gameObject.name == "sword_hilt") {
			PlaySwordCollectedSound ();
			_hasHilt = true;
			Debug.Log ("Has hilt");
		}
		//---------
		else {
			_isJumping = false; //stop going to the planet
			Debug.Log ("Stopped jumping");
		}
	}

	public void PlayJumpingSound(){
		jumpSound.Play ();
	}
	public void PlayDeathSound(){
		deathSound.Play();
	}
	public void PlaySwordCollectedSound(){
		swordCollectedSound.Play ();
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

	//return darkside of planet's position.
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
