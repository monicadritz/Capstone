using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsTextController : MonoBehaviour {

	public float moveSpeed;
	public float minHeight;
	public float maxHeight;
	public RectTransform trans;
	public UnityEngine.UI.Text text;
	public RectTransform background;

	// Use this for initialization
	void Start () {
		maxHeight = 1.1f * (Screen.height / 2);
		minHeight = -maxHeight;
		moveSpeed = 2f * maxHeight / 12.5f;

		trans = GetComponent<RectTransform> ();
		trans.localPosition = new Vector3 (0f, minHeight, 0f);
		text = transform.Find ("CreditsText").gameObject.GetComponent<UnityEngine.UI.Text> ();
		background = transform.Find ("TextBackground").gameObject.GetComponent<RectTransform> ();
		background.sizeDelta = new Vector2 (text.preferredWidth, background.sizeDelta.y);
	}
	
	// Update is called once per frame
	void Update () {
		if (trans.localPosition.y > maxHeight)
			Destroy (gameObject);
		else {
			trans.localPosition = new Vector3 (trans.localPosition.x, trans.localPosition.y + moveSpeed * Time.deltaTime, 0f);
			//Debug.Log (trans.localPosition.y);
		}
	}
}
