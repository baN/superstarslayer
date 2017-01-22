using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionHandler : MonoBehaviour {
	private Vector3 originalPosition;

	void Start (){
		originalPosition = transform.position;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.CompareTag("Planet"))
		{
			transform.position = originalPosition;
			gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
}
