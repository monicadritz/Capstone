using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour {

	private MoveTurtle myMoveTurtle;

	private SpriteRenderer mySpriteRenderer;

	private Animator myAnim;


	// Use this for initialization
	void Start () {
		myMoveTurtle = GetComponentInParent<MoveTurtle> ();
		mySpriteRenderer = GetComponent<SpriteRenderer> ();
		myAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Gimmick" || other.gameObject.tag == "Star") {
			myAnim.SetBool ("Collided", true);
			myMoveTurtle.SwitchToShell ();
		}
	}
}
