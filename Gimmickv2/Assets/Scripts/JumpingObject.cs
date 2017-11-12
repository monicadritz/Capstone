using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingObject : MonoBehaviour {

	// object that is going to be jumping
	public GameObject jumpingObject;

	// this is the SpriteRenderer component of the object that is jumping constantly
	private SpriteRenderer theSpriteRenderer;

	// sprites that will activate depending on whether the object is jumping up or falling down
	public Sprite movingUpSprite;
	public Sprite movingDownSprite;

	// these are the two points between where the object will jump
	public Transform topEndpoint;
	public Transform bottomEndpoint;

	// these are the speeds at which the object will jump up and fall down
	public float jumpUpSpeed;
	public float fallDownSpeed;

	// variables whose values will change depending on which direction the object is moving (up or down)
	private Vector3 currentTargetEndpoint;
	private float currentTargetSpeed;

	// Use this for initialization
	void Start () {
		currentTargetSpeed = jumpUpSpeed;
		currentTargetEndpoint = topEndpoint.position;

		theSpriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		if (theSpriteRenderer == null) {
			Debug.Log ("Failed to get SpriteRenderer");
		} else {
			theSpriteRenderer.sprite = movingUpSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (jumpingObject != null) {
			jumpingObject.transform.position = Vector3.MoveTowards (jumpingObject.transform.position, currentTargetEndpoint, currentTargetSpeed);

			if (jumpingObject.transform.position == topEndpoint.position) {
				currentTargetEndpoint = bottomEndpoint.position;
				currentTargetSpeed = fallDownSpeed;
				theSpriteRenderer.sprite = movingDownSprite;
				//Debug.Log ("done moving up");
			} 

			if (jumpingObject.transform.position == bottomEndpoint.position) {
				currentTargetEndpoint = topEndpoint.position;
				currentTargetSpeed = jumpUpSpeed;
				theSpriteRenderer.sprite = movingUpSprite;
				//Debug.Log ("done moving down");
			} 
		}
	}
}
