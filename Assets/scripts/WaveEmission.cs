using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEmission : MonoBehaviour {
	public Transform emissionTarget;
	public GameObject bullet;

	public float spinSpeed;
	public float bulletSpeed;
	[Range(0,100)]
	public int bulletCount;
	public float waveLength = 1;

	private float bulletWait;//time between projectiles
	private GameObject[] bulletArray;



	private int bulletIndex = 0;
	// Use this for initialization
	void Start () {
		bulletWait = 20 / spinSpeed;
		bulletArray = new GameObject[bulletCount];
		loadBullets ();
		InvokeRepeating ("shootWave",0,waveLength);
	}

	void loadBullets(){
		for (int i = 0; i < bulletCount; i++) {
			bulletArray[i] = Instantiate(bullet, transform.position,  Quaternion.identity);
		}
	}
	void shootWave(){
		bulletIndex = 0;
		fire ();
	}
	void fire(){
		GameObject currentBullet = bulletArray [bulletIndex];
		//put the bullet back inside the emitter and stop it
		currentBullet.transform.position = transform.position;
		currentBullet.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;

		Rigidbody2D bulletRigidBody = bulletArray [bulletIndex].GetComponent<Rigidbody2D> ();
		Vector2 moveDirection = (emissionTarget.position - transform.position) * bulletSpeed; 
		bulletRigidBody.AddForce (moveDirection);
		bulletIndex++;
		if(bulletIndex<bulletCount){
			Invoke("fire", bulletWait);//this will happen after 2 seconds
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 0, Time.deltaTime * spinSpeed);
	}
}
