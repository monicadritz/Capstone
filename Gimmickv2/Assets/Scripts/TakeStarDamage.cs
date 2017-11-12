using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeStarDamage : MonoBehaviour {

	public int damageToTake;
	private LevelManager theLevelManager;
	private EnemyHealthManager myEnemyHealthManager;
	private Rigidbody2D myRigidbody;
	public const float STARPOWER = 1f;
	//public AudioSource starHitSound;
	
	// Use this for initialization
	void Start () {
		theLevelManager = GetComponent<LevelManager> ();
		myEnemyHealthManager = GetComponent<EnemyHealthManager> ();
		myRigidbody = GetComponent<Rigidbody2D> ();
		//starHitSound = GameObject.Find ("Star Hit Sound").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("Something enter trigger zone");
		if (other.tag == "StarGround") {
			Debug.Log ("Star enter trigger zone");
			myEnemyHealthManager.giveDamage (damageToTake);
			if (myEnemyHealthManager.enemyHealth > 0) {
                //starHitSound.Play ();
                AudioManager.instance.PlaySound2D("Star Hit");
				other.transform.parent.gameObject.GetComponent<StarController> ().createHitEffecct ();
				if (myRigidbody) {
					var starVel = other.transform.parent.gameObject.GetComponent<Rigidbody2D> ().velocity;
					var diffVec = transform.position - other.transform.parent.gameObject.transform.position;
					var extraVel = diffVec / diffVec.magnitude * starVel.magnitude * STARPOWER;
					myRigidbody.velocity = new Vector3 (myRigidbody.velocity.x + extraVel.x, myRigidbody.velocity.y + extraVel.y, 0f);
				}

				other.transform.parent.gameObject.SetActive (false);
			}
		}
	}

//	void OnCollisionEnter2D(Collision2D other){
//		if (other.gameObject.tag == "Star") {
//			myEnemyHealthManager.giveDamage (damageToTake);
//		}
//	}
}
