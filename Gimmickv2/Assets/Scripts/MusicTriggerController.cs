using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggerController : MonoBehaviour {

	public AudioSource bossMusic;
	public bool triggered;

	// Use this for initialization
	void Start () {
		triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Gimmick" && !triggered) {
			triggered = true;
			AudioManager.instance.ChangeMusic(bossMusic, 5);
		}
	}
}
