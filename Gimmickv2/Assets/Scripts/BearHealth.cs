using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearHealth : MonoBehaviour {

	public int damageToReceive;
	private EnemyHealthManager theHealthManager;
	public GameObject theStar;
	private StarController theStarController;
	private LevelManager theLevelManager;
	public float timeInvincible;

	// Use this for initialization
	void Start () {
		theHealthManager = GetComponent<EnemyHealthManager> ();
		theStarController = theStar.GetComponent<StarController> ();
		theLevelManager = FindObjectOfType <LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Star") {
			if (theStarController.canGiveDamage) {
				//theLevelManager.flashTimer = timeInvincible;
				theHealthManager.giveDamage (damageToReceive);
			}
		}
	}
}
