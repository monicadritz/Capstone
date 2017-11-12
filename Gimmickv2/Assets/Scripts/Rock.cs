using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
	public int damageToGive;

	private Rigidbody2D myRigidbody;
	private LevelManager theLevelManager;

	public float timeInvincible;					// the duration after getting hurt the player will be invincible

	// If this is false then the rock hasn't landed on the ground yet and is falling down 
	// or still stuck to the ceiling.  The player can only take damage when this is false.
	private bool onGround;       

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
		if (myRigidbody == null) {
			Debug.Log ("myRigidbody == null");
		}

		theLevelManager = FindObjectOfType<LevelManager> ();
		if (theLevelManager == null) {
			Debug.Log ("theLevelManager == null");
		}

		onGround = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/* This will be called once Gimmick is under the object attached to this script.  
	 * It will cause the object to have no constraints regarding it position.  Since the object
	 * will start frozen in the air, having no constraints will cause the object to fall down.
	 */
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Gimmick") {
			myRigidbody.constraints = RigidbodyConstraints2D.None;
		}
	}

	/* This function will be called once Gimmick collides with the box collider of the
	 * object attached to this script.  It will cause the player to take damage.
	 */
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Gimmick") {
			Debug.Log ("Rock touched Gimmick");
			if (!onGround) {
				theLevelManager.flashTimer = timeInvincible;
				theLevelManager.HurtPlayer (damageToGive);
				Debug.Log ("Rock touched Gimmick and not on ground -- just gave damage");
			}
		}

		if (other.gameObject.tag == "Ground") {
			onGround = true;
			Debug.Log ("rock on the ground");
		}
	}
}
