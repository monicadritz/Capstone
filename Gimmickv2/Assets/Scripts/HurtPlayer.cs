using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour {

	public int damageToGive;                        // damage that the object attached to this script will give
	public float timeInvincible;					// the duration after getting hurt the player will be invincible
	public bool hurtFromTop;
	public const float FLOATTOLERANCE = 0.001f;

	private LevelManager theLevelManager;			

	// Use this for initialization
	void Start () {
		theLevelManager = FindObjectOfType<LevelManager> ();
		if (theLevelManager == null) {
			Debug.Log ("theLevelManager == null");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (!theLevelManager.Invincible && other.tag == "Gimmick") {
			if (hurtFromTop || !touchedTop (other)) {
				theLevelManager.flashTimer = timeInvincible;
				theLevelManager.HurtPlayer (damageToGive);
			}
		
		}
	}


	void OnTriggerStay2D(Collider2D other) {
		if (!theLevelManager.Invincible && other.tag == "Gimmick") {
			if (hurtFromTop || !touchedTop (other)) {
				theLevelManager.flashTimer = timeInvincible;
				theLevelManager.HurtPlayer (damageToGive);
			}
		}
	}

	// Need to use collider2d's offsets here?
	public bool touchedTop(Collider2D other) {
		Vector2 rayStart = new Vector2 (transform.position.x, other.transform.position.y + other.offset.y);
		Vector2 down = new Vector2 (0f, -1f);
		int enemyMask = LayerMask.GetMask ("Enemy");
		RaycastHit2D rh = Physics2D.Raycast (rayStart, down, 1.5f, enemyMask);
		//Debug.Log("hit point: (" + rh.point.x + ", " + rh.point.y + ")");
		if (rh.point.x == 0f && rh.point.y == 0f)
			return true;
		else {
			float topOfBox = rh.collider.transform.position.y + rh.collider.offset.y + rh.collider.bounds.extents.y;
			return (rh && topOfBox - rh.point.y < FLOATTOLERANCE);
		}
	}
}
