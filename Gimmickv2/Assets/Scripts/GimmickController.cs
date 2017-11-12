// Name:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickController : MonoBehaviour {

	//Physics-related stuff
	public const float MAXSPEED = 10f;				// The max horizontal speed (units/sec) at which Gimmick can travel
	public const float NORMALACCEL = 20f;			// The acceleration (units/sec^2) experienced by Gimmick when speeding up
	public const float SKIDDECEL = 40f;				// The acceleration (units/sec^2) experienced by Gimmick when slowing down
	public const float COASTDECEL = 20f;			// The deceleration (units/sec^2) experienced by Gimmick while grounded and while left/right input is neutral
	public const float INITIALJUMPSPEED = 15f;		// Gimmick's initial upward velocity (units/sec) at the start of a jump
	public const float MAXJUMPDURATION = 0.3f;		// The maximum amount of time (sec) for which the player can hold the jump button after a jump to keep gravity at GRAVITYWHILEJUMPING
	public const float GRAVITYWHILEJUMPING = 3f;	// The gravity scale for Gimmick while the player holds the jump button after a jump, for up to MAXJUMPDURATION time
	public const float GRAVITYWHILEFALLING = 8f;	// The normal gravity scale for Gimmick, for when the conditions for GRAVITYWHILEJUMPING do not apply
	public const float MAXFALLSPEED = 25f;			// The max speed (units/sec) at which Gimmick can fall
	public const float THROWSPEEDX = 10f;			// The horizontal throw speed (relative to Gimimck) of the Star
	public const float THROWSPEEDY = 15f;			// The downwards throw speed (relative to Gimimck) of the Star


	// Other constants
	public const float FLOATTOLERANCE = 0.00001f;	// Tolerance for comparing two floating point numbers, to accomodate for blips in Unity's physics engine
	public const float COLLISIONVARIANCE = 0.01f;	// Tolerance for heights of the two collision points returned when landing on a flat surface

	// Enum for xInput (left/right input) values
	public const int LEFT = -1;
	public const int NEUTRAL = 0;
	public const int RIGHT = 1;

	//Character status-related variables
	public int xInput;					// Indicates whether the player is pressing left, right, or neither; uses above LEFT/NEUTRAL/RIGHT enum
	public bool isGrounded;				// Indicates whether Gimmick is standing on something
	public float currentJumpDuration;	// The amount of time (sec) for which the player has held the jump button following a jump to decrease gravity.  Is 0 when normal gravity applies.
	public float yVelBeforeCollisions;  // The Y-Velocity returned by getYVelocity(), before Unity collides objects and adjusts their velocities.  Used to help detect ground collision.
	public bool cantBounceBack;          // When true the player cannot bounce back when collides with certain objects

	// Variables for checking if grounded
	public Transform groundCheck;		// Center of ground-collision box, **is initialized in Unity main window**
	public Vector2 groundCheckDim;		// Dimensions of ground-collision box, **is initialized in Unity main window**
	public LayerMask whatIsGround;		// LayerMask for ground, **is initialized in Unity main window**
	public LayerMask whatIsStarGround;  // LayerMask for the ground-collision child of the Star
	public GameObject ground;			// A ground Gimmick is currently standing on, or dummyGround if Gimmick is airborne.  Gimmick is a child of this GameObject
	public List<GameObject> grounds;	// All ground objects Gimmick is currently standing on, or empty if Gimmick is airborne.
	public bool groundChanged;			// Indicates whether the ground has just changed
	public GameObject dummyGround;		// A constant, immobile ground object, that Gimmick will be a child of while airborne.  This is to simplify code, so that Gimmick is always a child of some object.  **Initialized in Unity main window**

	// Various Unity components
	private Rigidbody2D myRigidbody;
	public SliderJoint2D joint;
	private Animator myAnim;

	// Other game objects
	public GameObject star;				// **Initialized in Unity main window**

    public LevelManager theLevelManager; //gives access to the level mangager script
    public Vector3 respawnPosition;//where the character will respawn after death;
    public bool canMove;

    public AudioClip jumpSound;
    public AudioClip hurtSound;

    // Use this for initialization
    void Start () {
		// Initialize variables to default values, get components
		transform.parent = null;
		xInput = NEUTRAL;
		currentJumpDuration = 0f;
		yVelBeforeCollisions = 0f;
		isGrounded = false;
		ground = dummyGround;
		groundChanged = false;
		//transform.parent = dummyGround.transform;
		myRigidbody = GetComponent<Rigidbody2D> ();
		myRigidbody.gravityScale = GRAVITYWHILEFALLING;
		myAnim = GetComponent<Animator> ();
		star.SetActive (false); // *****This really should go in the level manager script, when that gets made*****
        respawnPosition = transform.position;
        theLevelManager = FindObjectOfType<LevelManager>();
        canMove = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (canMove) {
			// Check if the ground Gimmick is on has changed
			if (groundChanged) {
				handleGroundChange ();
				groundChanged = false;
			}

			// Calculate new X- and Y-velocity, set gravity, update velocity
			float newXVel = getXVelocity ();
			float newYVel = getYVelocity ();
			yVelBeforeCollisions = newYVel;
			myRigidbody.velocity = new Vector3 (newXVel, newYVel, 0f);

			// Make sprite face correct left/right direction
			faceSprite ();

			// Handle throwing of star
			if (Input.GetButtonDown ("Throw Star"))
				throwStar ();

			// Update animator's variables
			myAnim.SetInteger ("xInput", xInput);
			myAnim.SetBool ("Grounded", isGrounded);
			// Update Gimmick's walking speed
			if (xInput != 0 && isGrounded)
				myAnim.speed = Mathf.Abs (newXVel) / 5f;
			else
				myAnim.speed = 1f;
		}
	}


	/*
	 * DEPRECATED: Ground-collision is now handled in OnCollisionEnter2D()
	 * Returns whether Gimmick is on the ground.  To be grounded, Gimmick must be colliding with something in the Ground
	 *   layer directly beneath him, and must have the same (up to FLOATTOLERANCE) velocity as that ground.
	 */
	public bool checkIfGrounded() {
		// Check for collision with Ground-layer object using ground check box
		Collider2D other = Physics2D.OverlapBox (groundCheck.position, groundCheckDim, 0, whatIsGround);
		if (other) {
			// Possible ground detected
			if (Object.ReferenceEquals (ground, other.gameObject)) {
				// Still on same ground as before
				return true;
			} else {
				// Not on this ground yet, check if Gimmick's velocity is less than the ground's modulo FLOATTOLERANCE
				Rigidbody2D otherBody = other.gameObject.GetComponent<Rigidbody2D> ();
				if (otherBody) {
					// Candidate ground is moving
					if (myRigidbody.velocity.y - otherBody.velocity.y < FLOATTOLERANCE) {
						// Landing on this ground
						ground = other.gameObject;
						return true;
					} else {
						// Not landing on this ground
						return false;
					}
				} else {
					// Candidate ground is not moving
					if (myRigidbody.velocity.y < FLOATTOLERANCE) {
						// Landing on this ground
						ground = other.gameObject;
						return true;
					} else {
						// Not landing on this ground
						return false;
					}
				}
			}
		} else {
			// Check for star as ground
			other = Physics2D.OverlapBox (groundCheck.position, groundCheckDim, 0, whatIsStarGround);
			if (other) {
				// Star below Gimmick
				// Convert to using the parent collider box
				if (other.gameObject.transform.parent != null)
					other = other.gameObject.transform.parent.gameObject.GetComponent<BoxCollider2D> ();
				
				if (Object.ReferenceEquals (ground, other.gameObject)) {
					// Still on star from before
					//myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0f, 0f);
					//Debug.Log ("set y vel to 0");
					return true;
				} else {
					// Not on the star yet, check if Gimmick's velocity is less than the star's modulo FLOATTOLERANCE
					Rigidbody2D otherBody = other.gameObject.GetComponentInParent<Rigidbody2D> (); // The star's Gimmick-collider is in a gameObject that is a child of the Star
					if (myRigidbody.velocity.y - otherBody.velocity.y < FLOATTOLERANCE) {
						// Landing on the star
						ground = other.gameObject;
						return true;
					} else {
						// Not landing on the star
						return false;
					}
				}
			} else {
				// No ground or star below Gimmick
				ground = dummyGround;
				return false;
			}
		}
	}


	/*
	 * Returns the X-velocity Gimmick will have on this frame, based on current situation and input.
	 *    If speeding up, use NORMALACCEL
	 *    If slowing down, use SKIDDECEL
	 *    If grounded with no left/right input, use COASTDECEL
	 *    If airborne with no left/right input, velocity unchanged
	 *    Speed is capped by MAXSPEED
	 *    Accounts for being on a moving platform if that moving platform contains a Rigidbody2D component
	 */
	public float getXVelocity () {
		// Get velocity of any ground Gimmick is standing on
		Rigidbody2D groundBody = ground.GetComponent<Rigidbody2D> ();
		SurfaceEffector2D groundEffect = ground.GetComponent<SurfaceEffector2D> ();
		Vector3 parentVel;
		if (groundBody)
			parentVel = groundBody.velocity;
		else if (groundEffect)
			parentVel = new Vector3 (groundEffect.speed * ground.transform.localScale.x, 0f, 0f);
		else
			parentVel = new Vector3 (0f, 0f, 0f);

		// Convert to local velocity
		Vector3 localVel = ((Vector3)myRigidbody.velocity) - parentVel;
		float xVel = localVel.x;

		if (Input.GetAxisRaw ("Horizontal") > 0 && canMove) {
			// Player inputting "right"
			xInput = RIGHT;
			if (myRigidbody.velocity.x > 0) {
				// Speeding up
				xVel = Mathf.Min (xVel + NORMALACCEL * Time.deltaTime, MAXSPEED);
			} else {
				// Slowing down
				xVel += SKIDDECEL * Time.deltaTime;
			}
		} else if (Input.GetAxisRaw ("Horizontal") < 0 && canMove) {
			// Player inputting "left"
			xInput = LEFT;
			if (myRigidbody.velocity.x < 0) {
				// Speeding up
				xVel = Mathf.Max (xVel - NORMALACCEL * Time.deltaTime, -MAXSPEED);
			} else {
				// Slowing down
				xVel -= SKIDDECEL * Time.deltaTime;
			}
		} else {
			// No left/right input
			if (isGrounded) {
				// Coasting to a stop
				xInput = NEUTRAL;
				if (xVel >= 0)
					xVel = Mathf.Max (xVel - COASTDECEL * Time.deltaTime, 0f);
				else
					xVel = Mathf.Min (xVel + COASTDECEL * Time.deltaTime, 0f);
			} // else airborne with no left/right input, velocity unchanged
		}

		localVel.x = xVel;
		return xVel + parentVel.x;
	}


	/*
	 * Returns the Y-velocity Gimmick will have on this frame, given the current situation and jump input.
	 * Also responsible for setting Gimmick's gravity scale based on whether a jump is being held or not
	 *    When player jumps, Y-velocity is INITIALJUMPSPEED, gravity scale is GRAVITYWHILEJUMMPING
	 *    After a jump, if jump button is still held, gravity scale stays at GRAVITYWHILEJUMPING, when released it becomes GRAVITYWHILEFALLING
	 *       If a jump is held for MAXJUMPDURATION, gravity is automatically set to GRAVITYWHILEFALLING
	 */
	public float getYVelocity () {
		float yVel = myRigidbody.velocity.y;

		if (currentJumpDuration > 0) {
			// Was jumping on previous frame
			if (Input.GetButton ("Jump") && currentJumpDuration < MAXJUMPDURATION) {
				// Still jumping
				currentJumpDuration += Time.deltaTime;
                


            } else {
                // Ending a jump
               
                currentJumpDuration = 0;
				myRigidbody.gravityScale = GRAVITYWHILEFALLING;
			}
		} else if (isGrounded && Input.GetButtonDown ("Jump")) {
            // Starting a jump this frame
            //jumpSound.Play();
            // AudioManager.instance.PlaySound(jumpSound, transform.position);
            // AudioManager.instance.PlaySound("GimmickJump", transform.position);
            AudioManager.instance.PlaySound2D("Jump");
            if (transform.gameObject.GetComponent<SliderJoint2D> ()) {
             
                Destroy (transform.gameObject.GetComponent<SliderJoint2D> ());
				joint = null;
			}
			yVel = INITIALJUMPSPEED;
			currentJumpDuration += Time.deltaTime;
			myRigidbody.gravityScale = GRAVITYWHILEJUMPING;
		} else {
			// Not jumping on this frame or the previous frame
			yVel = Mathf.Max (yVel, -MAXFALLSPEED);
		}
		return yVel;
	}


	/*
	 * Makes Gimmick face in the same direction as the user's left/right input.
	 * If there is no left/right input, the direction is left unchanged.
	 */
	public void faceSprite () {
		if (xInput == RIGHT)
			transform.localScale = new Vector3 (1f, 1f, 1f);
		else if (xInput == LEFT)
			transform.localScale = new Vector3 (-1f, 1f, 1f);
	}


	/*
	 * Causes Gimmick to throw a star, removing the previous star if it exists.
	 * Precondition: faceSprite() is called before this method is called.
	 */
	public void throwStar() {

		// Deactivate current star if it is active
		if (star.activeSelf)
			star.SetActive (false);

		// Move star directly above Gimmick's head
		star.transform.position = new Vector3 (transform.position.x, transform.position.y + 1f, 0f);

		// Calculate star's initial velocity, using THROWSPEEDX/Y and Gimmick's velocity
		float starXVel;
		if (transform.localScale.x == 1f)
			starXVel = myRigidbody.velocity.x + THROWSPEEDX;
		else
			starXVel = myRigidbody.velocity.x - THROWSPEEDX;
		float starYVel = myRigidbody.velocity.y - THROWSPEEDY;

		// Reactivate Star
		star.SetActive (true);
		star.GetComponent<Rigidbody2D>().velocity = new Vector3 (starXVel, starYVel, 0f);
	}

	/*
	 * Sets the parent of Gimmick to whatever ground he is on.  Makes Gimmick initially stick to the star when he lands on it.
	 * Precondition: this should only be called when the ground Gimmick is on changes.
	 */
	public void handleGroundChange() {
		//transform.parent = ground.transform;
		if (ground.tag == "MovingPlatform")
			transform.parent = ground.transform;
		else
			transform.parent = null;
		GameObject starGround = grounds.Find (g => Object.ReferenceEquals (g.gameObject, star));
		if (starGround && !GetComponent<SliderJoint2D>()) {
			// Just landed on the star, initially stick to it
			Rigidbody2D starBody = star.GetComponent<Rigidbody2D> ();
			var localVel = myRigidbody.velocity - starBody.velocity;
			localVel.x = 0f;
			myRigidbody.velocity = localVel + starBody.velocity;

			// Create slider joint between Gimmick and Star, forcing Gimmick to be 1 unit above Star
			transform.gameObject.AddComponent<SliderJoint2D> ();
			joint = transform.GetComponent<SliderJoint2D> ();
			joint.enableCollision = true;
			joint.connectedBody = star.GetComponent<Rigidbody2D> ();
			joint.anchor = new Vector2 (0f, 0f);
			joint.autoConfigureConnectedAnchor = false;
			joint.connectedAnchor = new Vector2 (0f, 1f);
			joint.autoConfigureAngle = false;
			joint.angle = 0f;
			joint.useMotor = false;

			// Move Gimmick 1 unit above Star (might not be necessary, just being careful)
			transform.position = new Vector3 (transform.position.x, star.transform.position.y + 1, 0f);
		} else if (!starGround || grounds.Count > 1) {
			// Either landed on not-Star or became airborne, remove slider joint if it exists
			if (transform.gameObject.GetComponent<SliderJoint2D> ()) {
				Destroy (transform.gameObject.GetComponent<SliderJoint2D> ());
				joint = null;
			}
		}
	}

	/*
	 * Handles the entering of collisions between Gimmick's box collider and other colliders.
	 *    If colliding with ground or the Star, we determine if Gimmick is landing on the star, and update isGrounded and ground accordingly.
	 */
	void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.tag == "Ground" || other.transform.tag == "Star" || other.transform.tag == "StarGround" || other.transform.tag == "MovingPlatform" || other.transform.tag == "BossWall") {
			if (other.contacts.Length >= 2) {
				// When two box colliders collide, contacts (normally?) has the two endpoints of the "line segment" where they intersect
				float y1 = other.contacts [0].point.y;
				float y2 = other.contacts [1].point.y;
				float groundHeight = other.gameObject.transform.position.y + other.collider.offset.y; // Height of the center of ground's collider box
				float myHeight = transform.position.y + other.otherCollider.offset.y; // Height of center of Gimmick's collider box
				if ((Mathf.Abs (y1 - y2) < FLOATTOLERANCE) && groundHeight < myHeight) {
					// Touched top or bottom of ground's box, and are above box (so touched top of box)
					if (yVelBeforeCollisions - myRigidbody.velocity.y <= 0) {
						// Collision with ground's box pushed us upward modulo FLOATTOLERANCE, so we have indeed landed on ground
						//groundChanged = true;
						isGrounded = true;
						// Add ground object to list if it isn't already there, update ground
						if (!grounds.Find (g => Object.ReferenceEquals (g.gameObject, other.gameObject))) {
							groundChanged = true;
							grounds.Add (other.gameObject);
							ground = grounds [0];
						}
					}
				} 
			}
		} 
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.transform.tag == "Ground" || other.transform.tag == "Star" || other.transform.tag == "StarGround" || other.transform.tag == "MovingPlatform" || other.transform.tag == "BossWall") {
			if (other.contacts.Length >= 2) {
				// When two box colliders collide, contacts (normally?) has the two endpoints of the "line segment" where they intersect
				float y1 = other.contacts [0].point.y;
				float y2 = other.contacts [1].point.y;
				float groundHeight = other.gameObject.transform.position.y + other.collider.offset.y; // Height of the center of ground's collider box
				float myHeight = transform.position.y + other.otherCollider.offset.y; // Height of center of Gimmick's collider box
				if ((Mathf.Abs (y1 - y2) < FLOATTOLERANCE) && groundHeight < myHeight) {
					// Touched top or bottom of ground's box, and are above box (so touched top of box)
					if (yVelBeforeCollisions - myRigidbody.velocity.y <= 0) {
						// Collision with ground's box pushed us upward modulo FLOATTOLERANCE, so we have indeed landed on ground
						//groundChanged = true;
						isGrounded = true;
						// Add ground object to list if it isn't already there, update ground
						if (!grounds.Find (g => Object.ReferenceEquals (g.gameObject, other.gameObject))) {
							groundChanged = true;
							grounds.Add (other.gameObject);
							ground = grounds [0];
						}
					}
				} 
			}
		}
	}


	/*
	 * Handles the exiting of collisions between Gimmick's box collider and other colliders.
	 *    If leaving the ground we were standing on, isGrounded and ground are updated accordingly.
	 */
	void OnCollisionExit2D(Collision2D other) {
		// Check if leaving a ground object
		if (grounds.Find(g => Object.ReferenceEquals (g.gameObject, other.gameObject))) {
			// Remove this ground from the list, update ground
			groundChanged = true;
			grounds.RemoveAll(g => Object.ReferenceEquals (g.gameObject, other.gameObject));
			if (grounds.Count > 0)
				ground = grounds [0];
			else {
				isGrounded = false;
				ground = dummyGround;
			}
		}
	}


    /*
     * Handles if Gimmick falls off the plateform and hits the kill plane 
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag =="KillPlane")
        {                     
            theLevelManager.Respawn();
        }
    }
}
	