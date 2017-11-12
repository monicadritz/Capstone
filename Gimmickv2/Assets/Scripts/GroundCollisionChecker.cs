using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollisionChecker : MonoBehaviour {

	public bool isGrounded;

	// Use this for initialization
	void Start () {
		isGrounded = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.tag == "Ground" || other.transform.tag == "StarGround") {
			isGrounded = false;
			foreach (ContactPoint2D groundTouch in other.contacts) {
				Debug.Log ("Touched ground: X=" + groundTouch.point.x + ", Y= " + groundTouch.point.y);
			}
		}
	}
}
