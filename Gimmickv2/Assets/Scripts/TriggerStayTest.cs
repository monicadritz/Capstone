using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStayTest : MonoBehaviour {

	private bool isTrueNow = false;
	private bool outToIn = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isTrueNow) {
			Debug.Log ("Is true now");
		}

		if (Input.GetKeyDown ("z")) {
			outToIn = true;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		Debug.Log ("Gimmick stay in collider");
		StartCoroutine ("TestingCos");
	}

	IEnumerator TestingCos(){
		isTrueNow = true;
		yield return new WaitForSeconds(10f);
		if (outToIn) {
			Debug.Log ("outToIn possible");
		} else {
			Debug.Log ("outToIn not possible");
		}
	}
}
