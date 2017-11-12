using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour {

	// this is what will be moved left and right
	public GameObject platformToMove;

	// this will determine if the object can be moved
	public bool movePlatform;

	// endpoints between which the object will move
	public Transform leftEndpoint;
	public Transform rightEndpoint;

	// the speed at which the object will move
	public float moveSpeed;

	// this will be either the leftEndpoint's position or the rightEndpoint's position -- it is where the object is moving towards
	private Vector3 currentTarget;

	// Use this for initialization 
	void Start () {
		currentTarget = rightEndpoint.position;
	}

	// Update is called once per frame
	void Update () {
		// if the object has been given the permission to move, then move it towards its current position
		if (movePlatform) {
			platformToMove.transform.position = Vector3.MoveTowards (platformToMove.transform.position, currentTarget, moveSpeed * Time.deltaTime);
		} 

		// if the object has reached an endpoint then change it currentTarget (where it moves to) to the opposite one
		if (platformToMove.transform.position == leftEndpoint.position) {
			currentTarget = rightEndpoint.position;
		}

		if (platformToMove.transform.position == rightEndpoint.position) {
			currentTarget = leftEndpoint.position;
		}
	}

	public void setPlatformInMotion(){
		movePlatform = true;
	}
}
