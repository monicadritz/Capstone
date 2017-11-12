using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

	public float scrollRateX;
	public float scrollRateY;
	private float xUnitsPerLoop;
	private float yUnitsPerLoop;
	private float xOffset;
	private float yOffset;

	public float leftLoopPoint;
	public float rightLoopPoint;
	public float bottomLoopPoint;
	public float topLoopPoint;

	// Use this for initialization
	void Start () {
		xUnitsPerLoop = rightLoopPoint - leftLoopPoint;
		yUnitsPerLoop = topLoopPoint - bottomLoopPoint;
	}
	
	// Update is called once per frame
	void Update () {
		float newXPos = xOffset + scrollRateX * Camera.main.transform.position.x;
		while (Camera.main.transform.position.x - newXPos < leftLoopPoint) {
			xOffset -= xUnitsPerLoop;
			newXPos -= xUnitsPerLoop;
		}
		while (Camera.main.transform.position.x - newXPos > rightLoopPoint) {
			xOffset += xUnitsPerLoop;
			newXPos += xUnitsPerLoop;
		}

		float newYPos = yOffset + scrollRateY * Camera.main.transform.position.y;
		while (Camera.main.transform.position.y - newYPos < bottomLoopPoint) {
			yOffset -= yUnitsPerLoop;
			newYPos -= yUnitsPerLoop;
		}
		while (Camera.main.transform.position.y - newYPos > topLoopPoint) {
			yOffset += yUnitsPerLoop;
			newYPos += yUnitsPerLoop;
		}

		transform.position = new Vector3 (newXPos, newYPos, 0f);
	}
}
