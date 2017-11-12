using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public int coinValue;
	private LevelManager theLevelManager;

	// Use this for initialization
	void Start () {
		theLevelManager = FindObjectOfType<LevelManager> ();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Gimmick") {
            
			theLevelManager.addPoints (coinValue);
            //theLevelManager.coinSound.Play();
            // AudioManager.instance.PlaySound(theLevelManager.coinSound, transform.position);
            //AudioManager.instance.PlaySound("coin", transform.position);
            AudioManager.instance.PlaySound2D("Coin");

            Destroy (gameObject);
		}
	}
}
