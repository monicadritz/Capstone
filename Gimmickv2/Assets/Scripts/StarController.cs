using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour {

	public const float MAXFALLSPEED = 25f;

	private Rigidbody2D myRigidbody;
    public int damangeToGive;
    private int damageToGive;
	public float contrailTimeout;
	public const float TIMEOUT = 0.5f;
	public const float XVELFORTIMEOUT = 3f;
	public const float YVELFORTIMEOUT = 20f;
	public float contrailTimer;
	public const float TIMEBETWEENCONTRAILS = 0.05f;
	public Transform starContrail;

	public GameObject starHitEffect;

	public bool canGiveDamage;  // ++++


    // Use this for initialization
    void Start () {
		contrailTimer = 0f;
		myRigidbody = GetComponent<Rigidbody2D> ();
		canGiveDamage = true; // ++++
	}


	// Update is called once per frame
	void Update () {
		contrailTimer += Time.deltaTime;
		updateContrailTimeout ();
		if (contrailTimer >= TIMEBETWEENCONTRAILS) {
			contrailTimer -= TIMEBETWEENCONTRAILS;
			if (contrailTimeout < TIMEOUT) {
				Transform starContrailClone = (Transform)Instantiate (starContrail, transform.position, transform.rotation);
			}
		}
		float yVel = getYVelocity ();
		myRigidbody.velocity = new Vector3 (myRigidbody.velocity.x, yVel, 0f);

		// +++++++++++++++++++++++++++++++++
		if (myRigidbody.velocity.x == 0f) {
			canGiveDamage = false;
		} else {
			canGiveDamage = true;
		}
		// +++++++++++++++++++++++++++++++++
	}


	public float getYVelocity() {
		float yVel = myRigidbody.velocity.y;
		return Mathf.Max (yVel, -MAXFALLSPEED);
	}

	void OnBecameInvisible()
	{
		gameObject.SetActive (false);
	}

	public void updateContrailTimeout() {
		if (Mathf.Abs(myRigidbody.velocity.x) < XVELFORTIMEOUT && Mathf.Abs(myRigidbody.velocity.y) < YVELFORTIMEOUT) {
			contrailTimeout += Time.deltaTime;
			contrailTimeout = Mathf.Min (contrailTimeout, TIMEOUT);
		}
		else
			contrailTimeout = 0f;
	}

	public void createHitEffecct() {
		Instantiate(starHitEffect, transform.position, transform.rotation);
	}

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag =="Enemy")
    //    {

    //        other.GetComponent<EnemyHealthManager>().giveDamage(this.damageToGive);
    //    }
       
    //    Destroy(gameObject);
    //}

}
