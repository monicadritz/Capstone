using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

	public bool hasFired;
	public float fireTimer;
	public bool canShoot;

	public const float TIMETOFIRE = 0.7f;
	public const float FIRECYCLE = 2f;
	public const float BULLETSPEED = 8f;

	public SpriteRenderer myRenderer;
	public GameObject gimmick;				// ***Initialize this in Unity main window***
	public Transform bullet;
	public Animator myAnim;

	// Use this for initialization
	void Start () {
		hasFired = false;
		fireTimer = 0f;
		myRenderer = GetComponent<SpriteRenderer> ();
		myAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (canShoot) {
			fireTimer += Time.deltaTime;
			if (fireTimer >= FIRECYCLE) {
				fireTimer -= FIRECYCLE;
				hasFired = false;
			}
			if (!hasFired && fireTimer > TIMETOFIRE) {
				fireBullet ();
				hasFired = true;
			}
		}

		myAnim.SetBool ("Active", canShoot);
	}

	void OnBecameVisible()
	{
		canShoot = true;
	}

	void OnBecameInvisible()
	{
		canShoot = false;
	}

	void OnEnable()
	{
		canShoot = false;
	}

	public void fireBullet()
	{
		Transform bulletClone = (Transform) Instantiate (bullet, transform.position, Quaternion.identity);
		BulletController theBulletController = bulletClone.gameObject.GetComponent<BulletController> ();
		var fireVecRaw = gimmick.transform.position - transform.position;
		var fireVec = fireVecRaw / fireVecRaw.magnitude * BULLETSPEED;
		theBulletController.xVel = fireVec.x;
		theBulletController.yVel = fireVec.y;
	}
}
