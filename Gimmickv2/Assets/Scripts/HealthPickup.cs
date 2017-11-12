using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {
    public int healthToGive;
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
        if( other.tag =="Gimmick")
        {
            theLevelManager.GiveHealth(healthToGive);
            gameObject.SetActive(false);
        }
    }
}
