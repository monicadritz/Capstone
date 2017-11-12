using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	//public const float MOVESPEED = 12f; // Speed of the bullet
	public float xVel;
	public float yVel;


	// Use this for initialization
	void Start () {
		
	}


	/*
	 * The bullet always moves horizontally in the direction it's facing (localScale.x), at speed MOVESPEED.
	 * The bullet is destroyed after leaving the screen.
	 */
	void Update () {
		float newXPos = transform.position.x;
		float newYPos = transform.position.y;
		newXPos += (xVel * Time.deltaTime);
		newYPos += (yVel * Time.deltaTime);
		transform.position = new Vector3 (newXPos, newYPos, 0f);
	}
		
	void OnBecameInvisible() {
		Destroy (gameObject);
	}
}
