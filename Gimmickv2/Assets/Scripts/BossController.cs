using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	public bool canMove;

	public const int IDLE = 0;
	public const int RUNNING = 1;
	public const int JUMPING = 2;
	public const int FALLING = 3;
	public const int THROWING = 4;
	public const int HURT = 5;
	public const int DIEING = 6;
	public int status;

	private float prevXVel;

	public List<GameObject> grounds;
	public GameObject ground;
	public bool isGrounded;
	public const float FLOATTOLERANCE = 0.00001f;

	private System.Random randObj;

	public const float HIGHJUMPODDS = 0.5f;
	public const float LOWJUMPODDS = 0.9f;

	public double throwTimer;
	public const double THROWTIMERMIN = 2;
	public const double THROWTIMERMAX = 4;

	public double idleTimer;
	public const double IDLETIMERMIN = 0.5;
	public const double IDLETIMERMAX = 2;

	public float runDestination;
	public const float RUNSPEED = 10f;
	public const float MINRUNDISTANCE = 2f;
	public float MINRUNX;
	public float MAXRUNX;

	public float jumpTimer;
	public const float JUMPTIME = 0.5f;
	public const float MINJUMPXVEL = -10f;
	public const float MAXJUMPXVEL = 10f;
	public const float MINJUMPYVEL = 17f;
	public const float MAXJUMPYVEL = 25f;

	public float throwingTimer;
	public const float BLUESHELLTIME = 0.25f;
	public const float THROWINGTIME = 0.5f;
	public const float THROWSPEED = 8f;
	public bool shellThrown;
	public float gravity;

	public const float HURTSPEED = 12f;
	public const float HURTHEIGHT = 15f;
	public int health;

	public const float DEATHTIME = 3f;
	public float deathTimer;
	public float explosionCounter;
	public const float EXPLOSIONSPERSECOND = 4f;
	public bool dead;

	public const float STARPOWERX = 0.75f;
	public const float STARPOWERY = 2.5f;
	public const float MAXHITYVEL = 25f;

	private Rigidbody2D myRigidBody;
	private BoxCollider2D[] boxes;
	private BoxCollider2D solidBox;
	private BoxCollider2D triggerBox;
	public GameObject gimmick;
	private LevelManager theLevelManager;
	public GameObject deathEffect;
	public GameObject hurtEffect;
	public Animator myAnim;
	public Transform blueShell;
	public Transform blueExplosion;
	public GameObject levelEnd;

	//public AudioSource deathSound;

	// Use this for initialization
	void Start () {
		canMove = false;
		status = IDLE;
		prevXVel = 0f;
		ground = null;
		randObj = new System.Random ();
		throwTimer = (THROWTIMERMAX - THROWTIMERMIN) * randObj.NextDouble () + THROWTIMERMIN;
		idleTimer = 0;
		jumpTimer = 0f;
		throwingTimer = 0f;
		shellThrown = false;
		dead = false;
		explosionCounter = DEATHTIME * EXPLOSIONSPERSECOND;
		health = 2;
		myRigidBody = GetComponent<Rigidbody2D> ();
		gravity = myRigidBody.gravityScale * Physics.gravity.y;
		boxes = GetComponents<BoxCollider2D> ();
		if (boxes [0].isTrigger) {
			solidBox = boxes [1];
			triggerBox = boxes [0];
		} else {
			solidBox = boxes [0];
			triggerBox = boxes [1];
		}
		theLevelManager = FindObjectOfType<LevelManager> ();
		myAnim = GetComponent<Animator> ();
		levelEnd = GameObject.Find ("LevelEnd");
	}
	
	// Update is called once per frame
	void Update () {
		if (canMove) {
			prevXVel = myRigidBody.velocity.x;
			if (throwTimer < 0) {
				if (status != HURT && status != DIEING)
					setStatus (THROWING);
				throwTimer = (THROWTIMERMAX - THROWTIMERMIN) * randObj.NextDouble () + THROWTIMERMIN;
			} else
				throwTimer -= Time.deltaTime;

			if (status == IDLE)
				handleIdle ();
			else if (status == RUNNING)
				handleRunning ();
			else if (status == JUMPING)
				handleJumping ();
			else if (status == FALLING)
				handleFalling ();
			else if (status == THROWING)
				handleThrowing ();
			else if (status == HURT)
				handleHurt ();
			else if (status == DIEING)
				handleDieing ();

			faceSprite ();
			myAnim.SetInteger ("Status", status);
		}
	}

	public void handleIdle() {
		if (idleTimer < 0) {
			//exit idle to either run or jump
			double runJumpDecider = randObj.NextDouble(); 
			if ((transform.position.y - 2f >= gimmick.transform.position.y && runJumpDecider < HIGHJUMPODDS) || (transform.position.y - 2f < gimmick.transform.position.y && runJumpDecider < LOWJUMPODDS)) {
				myRigidBody.velocity = getJumpVelocity ();
				transform.position = new Vector3 (transform.position.x, transform.position.y + 0.01f, 0f);
				setStatus (JUMPING);
			} else {
				setRunDestination ();
				setStatus (RUNNING);
			}
		} else {
			idleTimer -= Time.deltaTime;
		}
	}

	public void handleRunning() {
		if (myRigidBody.velocity.x > 0) {
			myRigidBody.velocity = new Vector3 (RUNSPEED, 0f, 0f);
			if (transform.position.x >= runDestination)
				setStatus (IDLE);
		} else {
			myRigidBody.velocity = new Vector3 (-RUNSPEED, 0f, 0f);
			if (transform.position.x <= runDestination)
				setStatus (IDLE);
		}
	}

	public void handleJumping() {
		if (isGrounded)
			setStatus (IDLE);
		else if (jumpTimer < 0)
			setStatus (FALLING);
		else
			jumpTimer -= Time.deltaTime;
	}

	public void handleFalling() {
		if (isGrounded)
			setStatus (IDLE);
	}

	public void handleThrowing() {
		if (throwingTimer < 0) {
			if (isGrounded)
				setStatus (IDLE);
			else
				setStatus (FALLING);
		} else if (throwingTimer < BLUESHELLTIME && !shellThrown) {
			throwBlueShell ();
			shellThrown = true;
			throwingTimer -= Time.deltaTime;
		} else
			throwingTimer -= Time.deltaTime;
	}

	public void handleHurt() {
		if (transform.position.y - Camera.main.transform.position.y > HURTHEIGHT) {
			solidBox.enabled = true;
			transform.position = new Vector3 (gimmick.transform.position.x, transform.position.y - 1, 0f);
			myRigidBody.velocity = new Vector3 (0f, 0f, 0f);
			setStatus (FALLING);
		} else {
			myRigidBody.velocity = new Vector3 (0f, HURTSPEED, 0f);
		}
	}

	public void handleDieing() {
		if (deathTimer < 0 && !dead) {
			levelEnd.GetComponent<SpriteRenderer>().enabled = true;
			dead = true;
            //deathSound.Play();
            AudioManager.instance.PlaySound2D("Boss Death");
            Instantiate (deathEffect, transform.position, transform.rotation);
			Destroy (gameObject);
		} else {
			deathTimer -= Time.deltaTime;
			if (deathTimer * EXPLOSIONSPERSECOND < explosionCounter && explosionCounter > 0) {
				explosionCounter--;
				float expX = transform.position.x + ((float)randObj.NextDouble ()) * 1.5f - 0.75f;
				float expY = transform.position.y + ((float)randObj.NextDouble ()) * 2.625f - 1.3125f;
				Transform blueExplosionClone = (Transform)UnityEngine.Object.Instantiate (blueExplosion, new Vector3 (expX, expY, 0f) , Quaternion.identity);
				AudioManager.instance.PlaySound2D("Blue Shell"); 
			}
		}
		
	}

	public void faceSprite() {
		if (status == RUNNING) {
			if (myRigidBody.velocity.x > 0)
				transform.localScale = new Vector3 (1f, 1f, 1f);
			else
				transform.localScale = new Vector3 (-1f, 1f, 1f);
		} else {
			if (transform.position.x < gimmick.transform.position.x)
				transform.localScale = new Vector3 (1f, 1f, 1f);
			else
				transform.localScale = new Vector3 (-1f, 1f, 1f);
		}
	}

	public void setStatus(int newStatus) {
		idleTimer = 0;
		jumpTimer = 0f;
		throwingTimer = 0f;
		shellThrown = false;//test
		deathTimer = 0f;
		if (newStatus == IDLE)
			idleTimer = (IDLETIMERMAX - IDLETIMERMIN) * randObj.NextDouble () + IDLETIMERMIN;
		else if (newStatus == JUMPING)
			jumpTimer = JUMPTIME;
		else if (newStatus == THROWING) {
			throwingTimer = THROWINGTIME;
			if (status == RUNNING) {
				myRigidBody.velocity = new Vector3 (0f, 0f, 0f);
			}
		}
		else if (newStatus == HURT) {
			Instantiate (hurtEffect, transform.position, transform.rotation);
			solidBox.enabled = false;
		}
		else if (newStatus == DIEING) {
			deathTimer = DEATHTIME;
		}
		status = newStatus;
	}

	public void throwBlueShell() {
		float gimmX = gimmick.transform.position.x;
		float gimmY = gimmick.transform.position.y;
		float handX;
		if (transform.localScale.x > 0)
			handX = transform.position.x + 1f;
		else
			handX = transform.position.x - 1f;
		float handY = transform.position.y - 0.5f;
		Vector3 handPos = new Vector3 (handX, handY, 0f);
		float a = gravity * (gimmX - handX) * (gimmX - handX) / (2f * THROWSPEED * THROWSPEED);
		Vector2 tangents = solveQuadratic (a, THROWSPEED, a - (gimmY - handY));
		float xVel, yVel;
		if (tangents.x == 0 && tangents.y == 0) {
			xVel = transform.localScale.x * THROWSPEED * 1.4f;
			yVel = THROWSPEED * 1.4f;
		} else {
			float tangent;
			if (transform.localScale.x > 0)
				tangent = Mathf.Max (tangents.x, tangents.y);
			else
				tangent = Mathf.Min (tangents.x, tangents.y);
			float hyp = Mathf.Sqrt (tangent * tangent + 1);
			xVel = transform.localScale.x * THROWSPEED / hyp;
			yVel = tangent * THROWSPEED / hyp;
		}

		Transform blueShellClone = (Transform)UnityEngine.Object.Instantiate (blueShell, handPos, Quaternion.identity);
		blueShellClone.GetComponent<Rigidbody2D> ().velocity = new Vector3 (xVel, yVel, 0f);
		if (transform.localScale.x > 0)
			blueShellClone.GetComponent<BlueShellController> ().targetX = gimmX + 1f;
		else
			blueShellClone.GetComponent<BlueShellController> ().targetX = gimmX - 1f;
		blueShellClone.GetComponent<BlueShellController> ().targetY = gimmY - 1f;
	}

	public Vector2 solveQuadratic (float a, float b, float c) {
		float disc = b * b - 4 * a * c;
		if (disc > 0)
			return new Vector2 ((-b + Mathf.Sqrt (disc)) / (a * 2), (-b - Mathf.Sqrt (disc)) / (a * 2));
		else
			return new Vector2 (0, 0);
	}

	void OnCollisionEnter2D(Collision2D other) {
		Debug.Log ("Tag: " + other.transform.tag);
		if (other.transform.tag == "Star" && status != DIEING && status != HURT) {
			Debug.Log ("Star Hit");
			Vector3 starVel = other.gameObject.GetComponent<Rigidbody2D> ().velocity;
			Vector3 pushVecRaw = getProjection (starVel, gimmick.transform.position - transform.position);
			float newXVel = pushVecRaw.x * STARPOWERX;
			float newYVel = Mathf.Min (pushVecRaw.y * STARPOWERY, MAXHITYVEL);
			myRigidBody.velocity = new Vector3 (newXVel, newYVel, 0f);
			setStatus (FALLING);
			other.gameObject.SetActive (false);
			if (gimmick.GetComponent<GimmickController> ().joint) {
				Destroy (gimmick.GetComponent<SliderJoint2D> ());
				gimmick.GetComponent<GimmickController> ().joint = null;
			}
		} else if (other.transform.tag == "BossSpikes") {
			health--;
			if (health == 0)
				setStatus (DIEING);
			else
				setStatus (HURT);
		} else if (other.transform.tag == "Ground" || other.transform.tag == "BossWall") {
			if (other.contacts.Length >= 2) {
				// When two box colliders collide, contacts (normally?) has the two endpoints of the "line segment" where they intersect
				float y1 = other.contacts [0].point.y;
				float y2 = other.contacts [1].point.y;
				float groundHeight = other.gameObject.transform.position.y + other.collider.offset.y; // Height of the center of ground's collider box
				float myHeight = transform.position.y + other.otherCollider.offset.y; // Height of center of Gimmick's collider box
				if ((Mathf.Abs (y1 - y2) < FLOATTOLERANCE) && groundHeight < myHeight && myRigidBody.velocity.y <= FLOATTOLERANCE) {
					// Touched top or bottom of ground's box, and are above box (so touched top of box)
					//groundChanged = true;
					isGrounded = true;
					// Add ground object to list if it isn't already there, update ground
					if (!grounds.Find (g => UnityEngine.Object.ReferenceEquals (g.gameObject, other.gameObject))) {
						grounds.Add (other.gameObject);
						ground = grounds [0];
					}
				} else if (other.transform.tag == "BossWall" && Mathf.Abs (y1 - y2) >= FLOATTOLERANCE) {
					//Touched side of bosswall, rebound
					myRigidBody.velocity = new Vector3 (-prevXVel, myRigidBody.velocity.y, 0f);
				}
			}
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.transform.tag == "Ground" || other.transform.tag == "BossWall") {
			if (other.contacts.Length >= 2) {
				// When two box colliders collide, contacts (normally?) has the two endpoints of the "line segment" where they intersect
				float y1 = other.contacts [0].point.y;
				float y2 = other.contacts [1].point.y;
				float groundHeight = other.gameObject.transform.position.y + other.collider.offset.y; // Height of the center of ground's collider box
				float myHeight = transform.position.y + other.otherCollider.offset.y; // Height of center of Gimmick's collider box
				if ((Mathf.Abs (y1 - y2) < FLOATTOLERANCE) && groundHeight < myHeight && myRigidBody.velocity.y <= FLOATTOLERANCE) {
					// Touched top or bottom of ground's box, and are above box (so touched top of box)
					//groundChanged = true;
					isGrounded = true;
					// Add ground object to list if it isn't already there, update ground
					if (!grounds.Find (g => UnityEngine.Object.ReferenceEquals (g.gameObject, other.gameObject))) {
						grounds.Add (other.gameObject);
						ground = grounds [0];
					}
				} 
			}
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		// Check if leaving a ground object
		if (grounds.Find(g => UnityEngine.Object.ReferenceEquals (g.gameObject, other.gameObject))) {
			// Remove this ground from the list, update ground
			//groundChanged = true;
			grounds.RemoveAll(g => UnityEngine.Object.ReferenceEquals (g.gameObject, other.gameObject));
			if (grounds.Count > 0)
				ground = grounds [0];
			else {
				isGrounded = false;
				ground = null;
				if (status != JUMPING)
					setStatus (FALLING);
			}
		}
	}

	public Vector3 getProjection (Vector3 source, Vector3 target) {
		float mag = (target.x * source.x + target.y * source.y) / (target.x * target.x + target.y * target.y);
		return new Vector3 (target.x * mag, target.y * mag, 0f);
	}

	public Vector3 getJumpVelocity() {
		float xVel;
		if (transform.position.x > (3 * MINRUNX + MAXRUNX) / 4)
			xVel = (float)(randObj.NextDouble () * MINJUMPXVEL);
		else
			xVel = (float)(randObj.NextDouble () * (MAXJUMPXVEL - MINJUMPXVEL) + MINJUMPXVEL);
		float yVel = (float)(randObj.NextDouble () * (MAXJUMPYVEL - MINJUMPYVEL) + MINJUMPYVEL);
		return new Vector3 (xVel, yVel, 0f);
	}

	public void setRunDestination() {
		int runDirection;
		float runDistance;
		if (transform.position.x > MAXRUNX - MINRUNDISTANCE)
			runDirection = -1;
		else if (transform.position.x < MINRUNX + MINRUNDISTANCE)
			runDirection = 1;
		else {
			double leftRightDecider = randObj.NextDouble () * (MAXRUNX - MINRUNX) + MINRUNX;
			if (transform.position.x > leftRightDecider)
				runDirection = -1;
			else
				runDirection = 1;
		}
			
		if (runDirection == -1) {
			runDistance = (float)(randObj.NextDouble () * (transform.position.x - MINRUNX - MINRUNDISTANCE) + MINRUNDISTANCE);
			runDestination = transform.position.x - runDistance;
			myRigidBody.velocity = new Vector3 (-RUNSPEED, 0f, 0f);
		} else {
			runDistance = (float)(randObj.NextDouble () * (MAXRUNX - transform.position.x - MINRUNDISTANCE) + MINRUNDISTANCE);
			runDestination = transform.position.x + runDistance;
			myRigidBody.velocity = new Vector3 (RUNSPEED, 0f, 0f);
		}
			
	}

	void OnBecameVisible()
	{
		canMove = true;
	}
}
