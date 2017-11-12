using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherController : MonoBehaviour {

	private Animator myAnimator;		// Animator for the launcher
	private Rigidbody2D bodyToLaunch;	// rigidbody2D of the object being launched

	public float bounceForce;			// the strength of the force applied to the object in an upwards direction
	public float timeToLaunch;			// time it takes for launching animation

	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Gimmick" || other.tag == "Star") {
			bodyToLaunch = other.GetComponent<Rigidbody2D> ();
			StartCoroutine ("LaunchCo");
		}
	}

	private IEnumerator LaunchCo(){
		myAnimator.SetBool ("SomethingOnPlatform", true);
		bodyToLaunch.AddForce (new Vector2 (0, bounceForce));
		yield return new WaitForSeconds (timeToLaunch);
		myAnimator.SetBool ("SomethingOnPlatform", false);
	}
}
