using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueExplosionController : MonoBehaviour {

	public const float LIFETIME = 0.5f;

	//public AudioSource explosionSound;

	// Use this for initialization
	void Start () {
		StartCoroutine ("MainCo");
        //explosionSound = GetComponent<AudioSource> ();
        //explosionSound.Play ();
        AudioManager.instance.PlaySound2D("Blue Shell"); 

    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator MainCo () {
		yield return new WaitForSeconds (LIFETIME);
		Destroy (gameObject);
	}
}
