using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{

    public GameObject deathEffect;
    public int enemyHealth;

    // public GameObject deathEffect;

    public int pointsOnDeath;
    private LevelManager theLevelManager;

    // Use this for initialization
    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    // kills the enemy with a particle effect and adds points for killing the enemy
    public void KillEnemy()
    {
        Destroy(gameObject);
        AudioManager.instance.PlaySound2D("Explosion");
        Instantiate(deathEffect, transform.position, transform.rotation);
        theLevelManager.addPoints(pointsOnDeath);
    }
    // This decreases the enemies health so that there can be some stronger enemies in the game
    public void giveDamage(int damageToGive)
    {
        enemyHealth -= damageToGive;
		if (enemyHealth <= 0)
			KillEnemy ();
    }
}
