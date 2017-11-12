using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTurtle : MonoBehaviour {

	public GameObject movingTurtle;			// the turtle that will be moving left and right

	private SpriteRenderer turtleSpriteRenderer;   // the Sprite Renderer for the moving turtle
	public Sprite shellSprite;				// the Sprite the turtle will switch to on contact

	public Transform leftEndpoint;			// farthest point to the left of the turtles path
	public Transform rightEndpoint;			// farthest point to the right of the turtles path

	public float walkingTurtleSpeed;		// speed of turtle when he walking
	public float slidingShellSpeed;			// speed of turtle's shell after the turtle has been hit once

	private bool turtleIsWalking;			// true if the turtle is walking, false if turtle has been hit and goes into his shell

	private Vector3 currentTarget;			// where the turtle or shell are moving towards at any moment in the game
	private bool movingLeft;				// true if the turtle or shell are moving towards the left endpoint, otherwise false

	private LevelManager theLevelManager;	// manages the level, used in this script to add points

	// Use this for initialization
	void Start () {
		currentTarget = leftEndpoint.position;
		movingLeft = true;

		turtleIsWalking = true;

		theLevelManager = FindObjectOfType<LevelManager> ();

		turtleSpriteRenderer = movingTurtle.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (movingTurtle) {
			// Move the turtle towards its current target at the appropriate speed (depending on if in shell or walking)
			if (turtleIsWalking) {
				movingTurtle.transform.position = Vector3.MoveTowards (movingTurtle.transform.position, currentTarget, walkingTurtleSpeed);
			} else {
				movingTurtle.transform.position = Vector3.MoveTowards (movingTurtle.transform.position, currentTarget, slidingShellSpeed);
			}

			// if the turtle has reached the left endpoint then switch his direction to the other end
			// else if the turtle has reached the right endpoint then switch his direction to the other end
			if (movingTurtle.transform.position == leftEndpoint.position) {
				SwitchTargetEndpoint ();
			} else if (movingTurtle.transform.position == rightEndpoint.position) {
				SwitchTargetEndpoint ();
			}
		}
	}

	private void SwitchTargetEndpoint(){
		if (movingLeft) {
			currentTarget = rightEndpoint.position;
			movingLeft = false;
			turtleSpriteRenderer.flipX = true;
		} else {
			currentTarget = leftEndpoint.position;
			movingLeft = true;
			turtleSpriteRenderer.flipX = false;
		}
	}

	public void SwitchToShell(){
		if (turtleIsWalking) {
			turtleIsWalking = false;
			SwitchTargetEndpoint ();
		}
	}


}
