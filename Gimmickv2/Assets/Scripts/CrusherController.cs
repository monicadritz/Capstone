using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherController : MonoBehaviour {

	// endpoints between which the object will move
	public Transform topEndpoint;
	public Transform bottomEndpoint;

	// the speed at which the object will move
	public const float moveSpeed = 2f;

	//extent to which Gimmick must be under the crusher to get crushed
	public const float XTOLERANCE = 0.1f; 

	// this will be either the leftEndpoint's position or the rightEndpoint's position -- it is where the object is moving towards
	private Vector3 currentTarget;

	private LevelManager theLevelManager;
	private GimmickController gimmick;

	// Use this for initialization 
	void Start () {
		currentTarget = topEndpoint.position;
		theLevelManager = FindObjectOfType<LevelManager> ();
		gimmick = FindObjectOfType<GimmickController> ();
	}

	// Update is called once per frame
	void Update () {
		// if the object has been given the permission to move, then move it towards its current position

		transform.position = Vector3.MoveTowards (transform.position, currentTarget, moveSpeed * Time.deltaTime);

		// if the object has reached an endpoint then change it currentTarget (where it moves to) to the opposite one
		if (transform.position == topEndpoint.position) {
			currentTarget = bottomEndpoint.position;
		}

		if (transform.position == bottomEndpoint.position) {
			currentTarget = topEndpoint.position;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.tag == "Gimmick" && gimmick.isGrounded) {
			if (other.contacts.Length >= 2) {
				// When two box colliders collide, contacts (normally?) has the two endpoints of the "line segment" where they intersect
				float x1 = other.contacts [0].point.x;
				float x2 = other.contacts [1].point.x;

				if (Mathf.Abs (x2 - x1) > XTOLERANCE) {
					theLevelManager.Invincible = false;
					theLevelManager.HurtPlayer (100);
				}
			}
		}
	}
}
