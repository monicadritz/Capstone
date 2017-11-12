using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearRightDZone : MonoBehaviour {

	private BearController theBearController;

	// Use this for initialization
	void Start () {
		theBearController = GetComponentInParent<BearController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		//TODO:
		// 	set parent object's BearController component's gimmickInDanger to true
		if (other.tag == "Gimmick") {
			theBearController.gimmickInDangerZone = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Gimmick") {
			theBearController.gimmickInDangerZone = false;
		}
	}
}
