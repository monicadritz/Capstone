using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour {

	private RectTransform trans;
	private Transform trans2;
	public float scaleFactor;

	// Use this for initialization
	void Start () {
		trans = GetComponent<RectTransform> ();
		trans2 = GetComponent<Transform> ();
		float scaling = ((float)Screen.height) / scaleFactor;
		if (trans)
			trans.localScale = new Vector3 (scaling, scaling, scaling);
		else if (trans2)
			trans2.localScale = new Vector3 (scaling, scaling, scaling);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
