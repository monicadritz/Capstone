using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour {

	public GameObject bear;

	private SpriteRenderer bearSpriteRenderer;
	private Animator bearAnimator;

	public Transform leftEndpoint;
	public Transform rightEndpoint;		

	public float moveSpeed;				// speed bear will be moving when it is patrolling and hasn't seen Gimmick
	public float alertedMoveSpeed;		// speed at which bear will move once it sees Gimmick

	public float changeDirectionTime;	// time it takes for the change direction animation to complete
	public float bearAttackTime;		// time it takes for bear to attack

	private Vector3 currentTarget;

	public bool movingLeft;				// true if the bear is moving towards the left
	public bool gimmickSeen;			// true if gimmick has been seen and thus the bear will move towards gimmick
	public bool gimmickInDangerZone;	// true if gimmick in zone where can take damage from bear

	public BoxCollider2D leftDangerZone;	// left side of the bear where he is in striking distance
	public BoxCollider2D rightDangerZone;	// right side of the bear wehre he is in striking distance

	private LevelManager theLevelManager;
	public int damageToGive;				// amount of damage bear gives to Gimmick on each swipe

	public float gimmickBounceBackForce;	// force at which Gimmick will bounce backwards after getting hit

	public GameObject gimmick;				// gimmick -- used for getting his position
	private Rigidbody2D gimmickRigidbody;	// used to push Gimmick back

	public bool gimmickUnderAttack;			// true when Gimmick has just been attacked

	public float timeInvincible;					// the duration after getting hurt the player will be invincible

	// Use this for initialization
	void Start () {
		currentTarget = leftEndpoint.position;

		movingLeft = true;

		gimmickSeen = false;

		gimmickInDangerZone = false;

		bearSpriteRenderer = bear.GetComponent<SpriteRenderer> ();

		bearAnimator = bear.GetComponent<Animator> ();

		theLevelManager = FindObjectOfType<LevelManager> ();

		gimmickRigidbody = gimmick.GetComponent<Rigidbody2D> ();

		leftDangerZone.enabled = true;
		rightDangerZone.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (bear != null) {
			// Bear should just patrol left and right if it hasn't seen Gimmick
			// if it see Gimmick then it should run towards it
			if (!gimmickSeen && !gimmickUnderAttack) {
				bear.transform.position = Vector3.MoveTowards (bear.transform.position, currentTarget, moveSpeed * Time.deltaTime);
			} else if (!gimmickUnderAttack) {
				// TODO:
				// move towards Gimmick's position at alerted speed
				// make sure sprite renderer is flipped in right direction

			}

			// TODO:
			// Gimmick is in danger zone then it should:
			//		1. stop moving and perform attack animation
			// 		2. deal damage to Gimmick
			//		3. push Gimmick out of the zone
			if (gimmickInDangerZone && !gimmickUnderAttack) {
				gimmickUnderAttack = true;
				StartCoroutine ("AttackGimmickCo");
			}

			// if bear reaches boundary of how far it travels, then it changes directions and stops chasing Gimmick
			if (bear.transform.position.x == leftEndpoint.position.x) {
				gimmickSeen = false;
				StartCoroutine ("ChangeDirectionCo");
			}
			if (bear.transform.position.x == rightEndpoint.position.x) {
				gimmickSeen = false;
				StartCoroutine ("ChangeDirectionCo");
			}
		}
	}

	private IEnumerator ChangeDirectionCo(){
		if (bear.transform.position.x == leftEndpoint.position.x) {
			Debug.Log ("changing direction");

			currentTarget = rightEndpoint.position;

			// make Bear change direction
			bearAnimator.SetBool ("Bear Changing Direction", true);
			bearAnimator.SetBool ("Bear Walking", false);
			bearAnimator.SetBool ("Bear Attack", false);

			yield return new WaitForSeconds (changeDirectionTime);

			bearSpriteRenderer.flipX = true;

			// make Bear walk
			bearAnimator.SetBool ("Bear Changing Direction", false);
			bearAnimator.SetBool ("Bear Walking", true);
			bearAnimator.SetBool ("Bear Attack", false);

			// since now moving to the right, make the right side the danger zone where Gimmick can be attacked by the bear
			movingLeft = false;
			leftDangerZone.enabled = false;
			rightDangerZone.enabled = true;
			gimmickInDangerZone = false;
		}

		if (bear.transform.position.x == rightEndpoint.position.x) {
			currentTarget = leftEndpoint.position;

			// make Bear change direction
			bearAnimator.SetBool ("Bear Changing Direction", true);
			bearAnimator.SetBool ("Bear Walking", false);
			bearAnimator.SetBool ("Bear Attack", false);

			yield return new WaitForSeconds (changeDirectionTime);

			bearSpriteRenderer.flipX = false;

			// make Bear walk
			bearAnimator.SetBool ("Bear Changing Direction", false);
			bearAnimator.SetBool ("Bear Walking", true);
			bearAnimator.SetBool ("Bear Attack", false);

			// since now moving to the left, make the left side the danger zone where Gimmick can be attacked by the bear
			movingLeft = true;
			leftDangerZone.enabled = true;
			rightDangerZone.enabled = false;
			gimmickInDangerZone = false;
		}
	}

	private IEnumerator AttackGimmickCo(){
		if (bear) {
			bearAnimator.SetBool ("Bear Attack", true);
			bearAnimator.SetBool ("Bear Walking", false);
			bearAnimator.SetBool ("Bear Changing Direction", false);

			yield return new WaitForSeconds (bearAttackTime);
		}
		if (bear) {
			if (movingLeft) {
				gimmickRigidbody.velocity = new Vector3 (-gimmickBounceBackForce, gimmickBounceBackForce, 0f);
			} else {
				gimmickRigidbody.velocity = new Vector3 (gimmickBounceBackForce, gimmickBounceBackForce, 0f);
			}

			theLevelManager.flashTimer = timeInvincible;
			theLevelManager.HurtPlayer (damageToGive);
		}

		if (bear) {
			bearAnimator.SetBool ("Bear Attack", false);
			bearAnimator.SetBool ("Bear Walking", true);
			bearAnimator.SetBool ("Bear Changing Direction", false);

			yield return new WaitForSeconds (0.5f);
		}
		gimmickUnderAttack = false;
	}
}

// TODO:
// 		- create a left and right vision triggers that when activated and Bear is pointing right direction
//		set gimmickSeen of this script to true