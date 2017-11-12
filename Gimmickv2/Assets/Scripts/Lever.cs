using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

	public Sprite leverInactive;
	public Sprite leverActive;

	private SpriteRenderer theSpriteRenderer; 

	private MovePlatform myMovingPlatform;

	// Use this for initialization
	void Start () {
		theSpriteRenderer = GetComponent<SpriteRenderer> ();
		theSpriteRenderer.sprite = leverInactive;

		myMovingPlatform = GetComponentInParent<MovePlatform> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Star") {
			theSpriteRenderer.sprite = leverActive;
			myMovingPlatform.setPlatformInMotion ();
		}

		/* OLD method of activating lever
		if (other.tag == "Gimmick" && Input.GetButtonDown("Jump")) {
			theSpriteRenderer.sprite = leverActive;
			myMovingPlatform.setPlatformInMotion ();
		}
		*/
	}
}
