using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HighScoreLevelSelect : MonoBehaviour {
	public string caveHighScoreScene;
	public string seasideHighScoreScene;
	public string factoryHighScoreScene;
	public string forestHighScoreScene;
	public string mainMenuScene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoCave(){
		SceneManager.LoadScene (caveHighScoreScene);
	}

	public void GoSeaside() {
		SceneManager.LoadScene (seasideHighScoreScene);
	}

	public void GoFactory(){
		SceneManager.LoadScene (factoryHighScoreScene);
	}

	public void GoForest(){
		SceneManager.LoadScene (forestHighScoreScene);
	}

	public void GoBack(){
		SceneManager.LoadScene (mainMenuScene);
	}
}
