using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour {
    
    //public GameObject deathSplosion; this was moved into the enemy health manager
    public int damageToGive;
    private LevelManager theLevelManager;


    // Use this for initialization
    void Start () {
        theLevelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        ////check if the triggered box collider is connected to an enemy tag or not and if it has health. If it has health it takes damage
		if (other.tag == "Enemy" && !other.isTrigger)
        {
			other.gameObject.GetComponent<EnemyHealthManager>().giveDamage(damageToGive);
        }
    }
}
