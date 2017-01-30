using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.Linq;

public class astronaut_controls : MonoBehaviour {
	public bool _hasBladeTop = false;
	public bool _hasBladeLow = false;
	public bool _hasHilt = false;
	public bool _beastMode = false; //redundant if has all the pieces.
	public bool died = false;
	public GameObject bladeTop;
	public GameObject bladeLow;
	public GameObject hilt;
	public float respawn_time = 2.0f;

	private bool _isWaiting = false; //set this inside a coroutine to wait using the _isWaiting flag to true. After it's done waiting, set the _isWaiting back to false.
	private bool _isJumping;
	private Transform target; //next planet to jump to
	private Transform darkside; //should always be going to the darkside unless jumping.
	private Vector3 initialPosition; 
	private AudioSource[] sounds;
	private AudioSource deathSound;
	private AudioSource jumpSound;
	private List<Sprite> sprites;
	//Place the path to each png file located in Assets for imploda here
	private string[] implodaAssetsLocation = {"Assets/Art/Imploda_Play/SSS_Imploda-00.png", "Assets/Art/Imploda_Play/SSS_Imploda-Hurt.png", "Assets/Art/Imploda_Play/SSS_Imploda-Sword.png"};
	private SpriteRenderer selfSpriteRenderer;


	// Use this for initialization
	void Start () {
		//set astronauts initial position saved so when she dies, it resets here.
		selfSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		initialPosition = transform.position;
		sounds = GetComponents <AudioSource>();
		deathSound = sounds [0];
		jumpSound = sounds [1];
		sprites = new List<Sprite>();
		InitializeSpritesArray (implodaAssetsLocation, ref sprites);
	}

	// Update is called once per frame
	void Update () {

		if (_isWaiting == false) {
			if (_hasHilt) {
				if (_hasBladeTop)
					SwitchSpriteHiltTop ();
				else if (_hasBladeLow)
					SwitchSpriteHiltLow ();
				else
					SwitchSpriteHilt ();
			} else if (_hasBladeLow) {
				if (_hasBladeTop)
					SwitchSpriteLowTop ();
				else
					SwitchSpriteLow ();
			} else if (_hasBladeTop)
				SwitchSpriteTop ();

			if (_hasBladeTop && _hasBladeLow && _hasHilt) {
				//Defeat the sun stage, switch the sprite to holding the sword
				SwitchSpriteBeast();
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
				MoveTo (transform.position, target.position);
			}
			if (darkside) {
				//go to the darkside if planet parent is selected.
				MoveTo (transform.position, darkside.position);
			}

			if (died) {
				//lose all sword pieces, turn off display, return to original position, don't move anywhere, reset targets.
				ResetSwordCollection ();
				PlayDeathSound ();
				SwitchSpriteHurt ();
				StartCoroutine (diedPart2()); //wait for a bit
			}

		}

	}
		
	/**
	 *  Theorhetically this function will return here until seconds is up and finish the rest of this function.
	 */
	private IEnumerator diedPart2()
	{
		_isWaiting = true;
		yield return new WaitForSeconds( respawn_time ); 
		// finish up the death transaction
		Invisible (true);
		ResetPositionToInitial ();
		died = false;
		_isJumping = false;
		target = null;
		darkside = null;
		MoveTo (transform.position, initialPosition);
		SwitchSpriteNormal ();
		_isWaiting = false; //return state so Update() can switch back to normal
	}

	//2D Collision detection...
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.name == "UVBullet(Clone)") {
			Debug.Log ("Astronaut was hit by a UVWave");
			died = true;
		}
		if (col.gameObject.name == "sword_top") {
			_hasBladeTop = true;
			Debug.Log ("Has sword_top");
		}
		if (col.gameObject.name == "sword_low") {
			_hasBladeLow = true;
			Debug.Log ("Has sword_low");
		}
		if (col.gameObject.name == "sword_hilt") {
			_hasHilt = true;
			Debug.Log ("Has hilt");
		}
		if (col.gameObject.name == "UVBullet(Clone)") {
			//ignore
		}
		else {
			_isJumping = false; //stop going to the planet
			Debug.Log ("Stopped jumping");
		}
	}


	//-------- AUDIO-------
	public void PlayJumpingSound(){
		jumpSound.Play ();
	}
	public void PlayDeathSound(){
		deathSound.Play();
	}

	public void MoveTo(Vector3 source, Vector3 destination){
		transform.position = Vector2.MoveTowards (source, destination, .10f);
	}

	//----------- RESPAWN ---------
	public void ResetSwordCollection(){
		_hasBladeLow = false;
		_hasBladeTop = false;
		_hasHilt = false;
		hilt.GetComponent<PickupSwords> ().Respawn ();
		bladeLow.GetComponent<PickupSwords> ().Respawn ();
		bladeTop.GetComponent<PickupSwords> ().Respawn ();
	}
	public void ResetPositionToInitial(){
		transform.position = initialPosition;
	}
	public void Invisible(bool invis){
		if (selfSpriteRenderer != null)
			selfSpriteRenderer.enabled = invis;
	}
		
	private void SwitchSpriteHurt(){
		selfSpriteRenderer.sprite = sprites [1];
	}
	private void SwitchSpriteNormal(){
		selfSpriteRenderer.sprite = sprites [0];
	}
	private void SwitchSpriteBeast(){
		selfSpriteRenderer.sprite = sprites [2];
	}
	private void SwitchSpriteHilt(){
		selfSpriteRenderer.sprite = sprites [3];
	}
	private void SwitchSpriteHiltTop(){
		selfSpriteRenderer.sprite = sprites [4];
	}
	private void SwitchSpriteHiltLow(){
		selfSpriteRenderer.sprite = sprites [5];
	}
	private void SwitchSpriteLow(){
		selfSpriteRenderer.sprite = sprites [6];
	}
	private void SwitchSpriteLowTop(){
		selfSpriteRenderer.sprite = sprites [7];
	}
	private void SwitchSpriteTop(){
		selfSpriteRenderer.sprite = sprites [8];
	}
	/*
	*  Function extracts the png files and converts them into sprites, storing them in the parameter store passed in
	*  
	*/
	private void InitializeSpritesArray(string[] location, ref List<Sprite> store){ //WARNING only works for png files!!---
//		foreach (string file in location) {	//load one of the files at a time
//			Object[] data = AssetDatabase.LoadAllAssetsAtPath(file); //grab all the pieces of the file
//			Sprite s = (Sprite)data[1];
//			store.Add(s); //add the extracted sprite to the list
//		}

		SpriteRenderer sr;
		sr = GameObject.Find ("imploda_00_holder").GetComponent<SpriteRenderer>();
		store.Add (sr.sprite);
		sr = GameObject.Find ("imploda_hurt_holder").GetComponent<SpriteRenderer>();
		store.Add (sr.sprite);
		sr = GameObject.Find ("imploda_sword_holder").GetComponent<SpriteRenderer>();
		store.Add (sr.sprite);
		sr = GameObject.Find ("imploda_sword_hilt").GetComponent<SpriteRenderer>();
		store.Add (sr.sprite);
		sr = GameObject.Find ("imploda_sword_hilt_top").GetComponent<SpriteRenderer>();
		store.Add (sr.sprite);
		sr = GameObject.Find ("imploda_sword_hilt_low").GetComponent<SpriteRenderer>();
		store.Add (sr.sprite);
		sr = GameObject.Find ("imploda_sword_low").GetComponent<SpriteRenderer>();
		store.Add (sr.sprite);
		sr = GameObject.Find ("imploda_sword_low_top").GetComponent<SpriteRenderer>();
		store.Add (sr.sprite);
		sr = GameObject.Find ("imploda_sword_top").GetComponent<SpriteRenderer>();
		store.Add (sr.sprite);
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
