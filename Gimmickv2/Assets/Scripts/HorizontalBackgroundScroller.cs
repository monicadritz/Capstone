using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBackgroundScroller : MonoBehaviour {

	public float scrollRate;
	private float xUnitsPerLoop;
	private float xOffset;
	public float leftLoopPoint;
	public float rightLoopPoint;

	private GameObject camera;

	// Use this for initialization
	void Start () {
		xUnitsPerLoop = rightLoopPoint - leftLoopPoint;
		camera = GameObject.Find ("Main Camera");
		transform.parent = camera.transform;
		transform.localPosition = new Vector3 (transform.localPosition.x, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		float newXPos = xOffset + scrollRate * Camera.main.transform.position.x;
		while (Camera.main.transform.position.x - newXPos < leftLoopPoint) {
			xOffset -= xUnitsPerLoop;
			newXPos -= xUnitsPerLoop;
		}
		while (Camera.main.transform.position.x - newXPos > rightLoopPoint) {
			xOffset += xUnitsPerLoop;
			newXPos += xUnitsPerLoop;
		}

		transform.position = new Vector3 (newXPos, transform.position.y, 0f);
	}
}
