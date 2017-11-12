using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShellController : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	public float targetX;
	public float targetY;
	public Transform blueExplosion;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((myRigidbody.velocity.x >= 0 && transform.position.x > targetX) || (myRigidbody.velocity.x < 0 && transform.position.x < targetX) || (myRigidbody.velocity.y < 0 && transform.position.y < targetY)) {
			Transform blueExplosionClone = Instantiate (blueExplosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
