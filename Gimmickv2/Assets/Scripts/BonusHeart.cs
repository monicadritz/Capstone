using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusHeart : MonoBehaviour {

	private LevelManager theLevelManager;
	public string levelName;				// used to build the bonusHeartKey
	private string bonusHeartKey;			// this is a key for a PlayerPref that stores whether this level's Bonus Heart has been collected


	// Use this for initialization
	void Start () {
		theLevelManager = FindObjectOfType<LevelManager> ();

		bonusHeartKey = levelName + "BonusHeartCollected";

		// Set the Bonus Heart to inactive if it has been collected before
		if (PlayerPrefs.HasKey (bonusHeartKey)) {
			if (PlayerPrefs.GetInt (bonusHeartKey) == 1) {
				gameObject.SetActive (false);
			} 
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		/* TODO: TENTATIVELY DONE
		 * 		1. increase max health
		 * 			-- get current max health
		 * 			-- increase it by 2
		 * 			-- if health over 14, then set max health to 14
		 * 		2. set collected for this level to true
		 * 		3. set gameobject inactive
		 * 		4. Set gimmick's current health = max health & update heart meter
		 */ 

		// increase max health
		if (other.tag == "Gimmick") {
			if (PlayerPrefs.HasKey (theLevelManager.maxHealthKey)) {
				int maxHealth;

				maxHealth = PlayerPrefs.GetInt (theLevelManager.maxHealthKey);

				maxHealth += 2;

				if (maxHealth > 14) {
					maxHealth = 14;
				}

				PlayerPrefs.SetInt (theLevelManager.maxHealthKey, maxHealth);

				theLevelManager.healthCount = maxHealth;
				theLevelManager.maxHealth = maxHealth;

				theLevelManager.UpdateHeartMeter ();
			} else {
				Debug.Log ("Max Health key for Gimmick never created");
			}

			// Mark the Bonus Heart as collected for this level
			PlayerPrefs.SetInt (bonusHeartKey, 1);

			// inactivate Bonus Heart so it cannot be collected again
			gameObject.SetActive (false);
		}
	}
}

/* TODO in NewGame():
 * 		- set all level's has collected key to false
 * 		- set MaxHealth PlayerPref to 6 
 */ 