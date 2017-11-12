using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public const float xTolerance = 2f;		// The maximum number of units Gimmick can deviate left/right from the center of the screen
	public const float yTolerance = 1.5f;	// The maximum number of units Gimmick can deviate up/down from the center of the screen

	public GameObject gimmick;				// Gimmick, the main character
	public BoxCollider2D killPlane;			// The kill plane for the level
	public float bottomHeight;				// The lowest height the camera can attain so that the kill plane is never above the bottom of the screen

	// Use this for initialization
	void Start () {
		killPlane = GameObject.Find ("Kill Plane").GetComponent<BoxCollider2D> ();
		bottomHeight =  killPlane.transform.position.y + killPlane.size.y  + Camera.main.orthographicSize;
	}

	
	// Update is called once per frame
	void Update () {
		// Move the camera a minimum distance from its previous location so that Gimmick is within xTolerance by yTolerance of the center of the screen
		float newX = Mathf.Max (Mathf.Min (transform.position.x, gimmick.transform.position.x + xTolerance), gimmick.transform.position.x - xTolerance);
		float newY = Mathf.Max (Mathf.Min (transform.position.y, gimmick.transform.position.y + yTolerance), gimmick.transform.position.y - yTolerance);
		//*****The following line assumes the Camera is a child of the Level Starter Pack.
		newY = Mathf.Max (newY, bottomHeight);
		transform.position = new Vector3 (newX, newY, -10f);
	}

}
