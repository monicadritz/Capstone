using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {

	// Left and right endpoints denoting Robot's patrol zone
	public Transform leftPoint;
	public Transform rightPoint;

	// Movement and Behavior Constants
	public const float MAXSPEED = 15f;					// The max speed (units/sec) (in either the x or y direction) in which the Robot can move while chasing
	public const float CHASEACCEL = 30f;				// The acceleration (units/sec^2) (in either the x or y direction) the Robot uses while chasing
	public const float TARGETDISTANCE = 4f;				// The horizontal distance (units) away from Gimmick the Robot will move towards while chasing
	public const float PATROLXSPEED = 2f;				// Robot's X-speed (units/sec) while patrolling
	public const float PATROLYSPEED = 0.5f;				// Robot's Y-speed (units/sec) while patrolling
	public const float SIGHTDISTANCEX = 8f;				// Horizontal distance (units) up to which the Robot can notice Gimmick while patrolling
	public const float SIGHTDISTANCEY = 1f;				// Vertical distance (units) up to which the Robot can notice Gimmick while patrolling
	public const float WIDTHFIRETOLERANCE = 4f;			// Horizontal distance (units) that the Robot must be away from Gimmick before firing
	public const float HEIGHTFIRETOLERANCE = 0.25f;		// Vertical distance (units) that the Robot must be within Gimmick before firing
	public const float FIRELENGTH = 1f;					// Time (sec) it takes for Robot to complete a single fire
	public const float TIMETOFIRE = 0.7f;				// Time (sec) during the countdown from FIRELENGTH at which the bullet is instantiated
	public const float BULLETSPEED = 12f;

	// Unity Objects
	private Rigidbody2D myRigidBody;
	public GameObject gimmick;			// **Initialize in Unity main window**
	private Animator myAnim;
	public Transform bullet;			// Prefab of bullet, for runtime instantiation

	// Enum for status variable
	private const int PATROLLING = 0;
	private const int CHASING = 1;
	private const int FIRING = 2;

	public int status;					// What the Robot is doing, use values from above enum
	public float patrolTime;			// Amount of time (modulo 1 second) the Robot has been patrolling, used to create hovering effect
	public float fireTimer;				// Timer for firing a bullet, starts at FIRELENGTH, counts down to 0, fires a bullet at TIMETOFIRE
	public bool fired;					// Indicates whether a bullet has been fired in a current firing cycle

	public bool canMove;

	//public AudioSource shootSound;
	//public AudioSource alertSound;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody2D> ();
		myAnim = GetComponent<Animator> ();
		status = PATROLLING;
		patrolTime = 0f;
		fireTimer = 0;
	}

	
	// Update is called once per frame
	void Update () {
		if (canMove) {
			// Handle movement and behavior based on status
			switch (status) {
			case PATROLLING:
				handlePatrolling ();
				break;
			case CHASING:
				handleChasing ();
				break;
			case FIRING:
				handleFiring ();
				break;
			default:
				break;
			}

			// Update animator variables
			myAnim.SetInteger ("Status", status); 
		}
	}


	/*
	 * Handles the movement and status update of the Robot when patrolling.
	 *    When patrolling, the Robot moves back and forth between leftPoint and rightPoint, hovering up and down slightly.
	 *    The Robot faces in the direction it is moving while patrolling.
	 *    If the Robot is facing Gimmick, and Gimmick is within SIGHTDISTANCEX-by-SIGHTDISTANCEY of the Robot, the Robot begins chasing.
	 */
	public void handlePatrolling() {

		// Update patrolTime, incrementing it by deltaTime and taking fractional part
		patrolTime += Time.deltaTime;
		while (patrolTime > 1f)
			patrolTime -= 1f;

		// Turn Robot around if it moves past either endpoint
		if (transform.position.x < leftPoint.position.x)
			transform.localScale = new Vector3 (1f, 1f, 1f);
		else if (transform.position.x > rightPoint.position.x)
			transform.localScale = new Vector3 (-1f, 1f, 1f);

		// Decide whether Robot is moving up or down in it's hovering effect.  The robot hovers up and down regularly at the rate of 2 cycles per second.
		float yVelDecider = 4f * (patrolTime - Mathf.Floor (patrolTime));
		float yVel;
		if (yVelDecider < 1)
			yVel = PATROLYSPEED;
		else if (yVelDecider < 2)
			yVel = -PATROLYSPEED;
		else if (yVelDecider < 3)
			yVel = PATROLYSPEED;
		else
			yVel = -PATROLYSPEED;

		// Update velocity, and check if Robot can see Gimmick
		float gimmickRelX = gimmick.transform.position.x - transform.position.x;
		float gimmickRelY = gimmick.transform.position.y - transform.position.y;
		if (transform.localScale.x > 0) {
			// Facing right
			myRigidBody.velocity = new Vector3 (PATROLXSPEED, yVel, 0f);
			if (gimmickRelX >= 0 && gimmickRelX <= SIGHTDISTANCEX && Mathf.Abs (gimmickRelY) <= SIGHTDISTANCEY)
				// Gimmick is to the right of Robot, and is within the x- and y-sight distance
				status = CHASING;
		} else {
			// Facing left
			myRigidBody.velocity = new Vector3 (-PATROLXSPEED, yVel, 0f);
			if (gimmickRelX <= 0 && gimmickRelX >= -SIGHTDISTANCEX && Mathf.Abs (gimmickRelY) <= SIGHTDISTANCEY) {
				// Gimmick is to the left of Robot, and is within the x- and y-sight distance
				status = CHASING;
                //alertSound.Play ();
                AudioManager.instance.PlaySound2D("Robot Alert");
            }
		}
	}


	/*
	 * Handles the movement and status update of the Robot when chasing.
	 *    If the Robot is to the right of Gimmick, it accelerates toward (at a 45-degree angle) the point TARGETDISTANCE units to the right of Gimmick.
	 *       Mirror the above if the Robot is to the left of Gimmick.
	 *    If the Robot is at the same height as Gimmick (up to HEIGHTFIRETOLERANCE) and the Robot is at least WIDTHFIRETOLERANCE units away from Gimmick (horizontally),
	 *       the Robot begins firing (initializes fireTimer to FIRELEGNTH as well)
	 *    The Robot always faces Gimmick while chasing.
	 *    The Robot's x- and y-speed will not exceed MAXSPEED while chasing.
	 */
	public void handleChasing() {

		// Check if firing conditions are met
		if (Mathf.Abs(transform.position.x - gimmick.transform.position.x) > WIDTHFIRETOLERANCE && Mathf.Abs (transform.position.y - gimmick.transform.position.y) < HEIGHTFIRETOLERANCE) {
			status = FIRING;
			fireTimer = FIRELENGTH;
		} else {
			float newXVel = myRigidBody.velocity.x;
			float newYVel = myRigidBody.velocity.y;

			// Get the x-coordinate of the point the Robot is accelerating toward, and make the Robot face Gimmick
			float targetX;
			if (transform.position.x >= gimmick.transform.position.x) {
				// Robot is to right of Gimmick, target position is to the right of Gimmick
				transform.localScale = new Vector3 (-1f, 1f, 1f);
				targetX = gimmick.transform.position.x + TARGETDISTANCE;
			} else {
				// Robot is to left of Gimmick, target position is to the left of Gimmick
				transform.localScale = new Vector3 (1f, 1f, 1f);
				targetX = gimmick.transform.position.x - TARGETDISTANCE;
			}

			// Make Robot accelerate (at a 45-degree angle) toward the target position, capping x- and y-speed with MAXSPEED
			if (transform.position.x > targetX) {
				// Robot is to the right of target position
				newXVel -= CHASEACCEL * Time.deltaTime;
				newXVel = Mathf.Max (newXVel, -MAXSPEED);
			} else {
				// Robot is to the left of target position
				newXVel += CHASEACCEL * Time.deltaTime;
				newXVel = Mathf.Min (newXVel, MAXSPEED);
			}
			if (transform.position.y > gimmick.transform.position.y) {
				// Robot is above target position
				newYVel -= CHASEACCEL * Time.deltaTime;
				newYVel = Mathf.Max (newYVel, -MAXSPEED);
			} else {
				// Robot is below target position
				newYVel += CHASEACCEL * Time.deltaTime;
				newYVel = Mathf.Min (newYVel, MAXSPEED);
			}

			// Update velocity
			myRigidBody.velocity = new Vector3 (newXVel, newYVel, 0f);
		}
	}


	/*
	 * Handles the movement and status update of the Robot while firing. 
	 * The Robot is stationary (and cannot change the direction it's facing) while firing.
	 * The full firing cycle lasts FIRELENGTH seconds.  A timer counts from then to 0, and the bullet spawns when the timer hits TIMETOFIRE.
	 * The Robot resumes chasing when the timer hits 0.
	 * The Bullet is fired in the direction the Robot is facing
	 */
	public void handleFiring() {
		// Update timer
		fireTimer -= Time.deltaTime;

		// Check if timer has expired
		if (fireTimer <= 0) {
			// Timer expired, resume chasing
			fired = false;
			status = CHASING;
		} else {
			// Timer has not expired
			// Make Robot stationary
			myRigidBody.velocity = new Vector3 (0f, 0f, 0f);
			// Fire bullet if timer crosses over TIMETOFIRE
			if (!fired && fireTimer < TIMETOFIRE) {
                //shootSound.Play ();
                AudioManager.instance.PlaySound2D("Robot Shoot");
                fired = true;
				Transform bulletClone = (Transform) Instantiate (bullet, transform.position, Quaternion.identity);
				// Make bullet face/move same direction as the Robot
				BulletController theBulletController = bulletClone.gameObject.GetComponent<BulletController> ();
				theBulletController.xVel = BULLETSPEED * transform.localScale.x;
				theBulletController.yVel = 0f;
			}
		}
	}


	void OnBecameVisible()
	{
		canMove = true;
	}

	void OnBecameInvisible()
	{
		if (status != PATROLLING)
			gameObject.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "KillPlane") {
			gameObject.SetActive (false);
		} else if (other.tag == "StarGround" && canMove && status == PATROLLING) {
			status = CHASING;
            //alertSound.Play ();
            AudioManager.instance.PlaySound2D("Robot Alert");
        }
	}
	void OnEnable()
	{
		canMove = false;
	}
}
