using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpikesController : MonoBehaviour {

	private GimmickController gimmick;
	private LevelManager theLevelManager;


	// Use this for initialization
	void Start () {
		gimmick = FindObjectOfType<GimmickController> ();
		theLevelManager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.transform.tag == "Gimmick") {
			theLevelManager.Invincible = false;
			theLevelManager.HurtPlayer (100);
		} else if (other.transform.tag == "Enemy") {
			EnemyHealthManager enemyHealth = other.gameObject.GetComponent<EnemyHealthManager> ();
			if (enemyHealth)
				enemyHealth.giveDamage(100);
		} else if (other.transform.tag == "Boss") {
			//hurt boss, fill this in once boss script is more developed
		}

	}
}
