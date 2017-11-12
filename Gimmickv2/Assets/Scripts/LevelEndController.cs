using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndController : MonoBehaviour {

	private bool isWaving;
	public const float TIMEUNTILFREEZE = 1f;
	public const float FREEZETIME = 2f;
	public string nextLevel;
	public float fadeSpeed;
	public Animator myAnim;
	private HighScore theHighScore;
    public Color loadUsingColor = Color.white;
	private LevelManager theLevelManager;
	public AudioSource levelCompleteMusic;
	public AudioSource bossMusic;
	public GameObject levelEndEffect;
	public SpriteRenderer myRenderer;


	// Use this for initialization
	void Start () {
		isWaving = false;
		fadeSpeed = 1f;
		myAnim = GetComponent<Animator> ();

		theHighScore = FindObjectOfType<HighScore> ();

		theLevelManager = GameObject.Find ("Level Manager").GetComponent<LevelManager> ();
		myRenderer = GetComponent<SpriteRenderer> ();
        //levelMusic = GameObject.Find ("Level Music").GetComponent<AudioClip>();
 //     levelCompleteMusic = GameObject.Find ("Level Complete Music").GetComponent<AudioClip>();
     
    }
	
	// Update is called once per frame
	void Update () {
		myAnim.SetBool ("Waving", isWaving);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Gimmick" && !isWaving && myRenderer.enabled) {
			theLevelManager.Invincible = true;
			isWaving = true;
			Instantiate(levelEndEffect, transform.position, transform.rotation);
			StartCoroutine ("LevelEndCo", other);
		}
	}

	public IEnumerator LevelEndCo (Collider2D other) {
		theHighScore.SaveScore();			// save the score the player had at the end of the level

		yield return new WaitForSeconds (TIMEUNTILFREEZE);
        // AudioManager.instance.PlayMusic(levelCompleteMusic);
        //levelMusic.Stop ();
        //levelCompleteMusic.Play ();
		bossMusic.Stop();
        AudioManager.instance.ChangeMusic(levelCompleteMusic, 5);
        Time.timeScale = 0f;
		other.gameObject.GetComponent<GimmickController> ().canMove = false;
		float endPause = Time.realtimeSinceStartup + FREEZETIME;

        
		while (Time.realtimeSinceStartup < endPause) {
			yield return 0;
		}

		//yield return new WaitForSeconds (FREEZETIME); //CAN'T WAIT FOR TIME WHEN TIME'S FROZEN!
		theHighScore.LogScores();			// log all of the scores for that level
		theHighScore.LogTopFiveScores ();   // log the top five scores for that level

		//Debug.Log ("Got this far");
        //fade out of the game and load the Level complete screen which will load the next screen via the LevelTitleScreenScript
        ScreenTransition.FadeScreen(nextLevel, loadUsingColor, fadeSpeed);
		//Time.timeScale = 1f;
         
      //this will be done through the levelTitle Screen Script
       // SceneManager.LoadScene(nextLevel);
	}
}
