using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour {
	/* OLD code
	public string levelName;					// name of the level whose high score is being saved
	private LevelManager theLevelManager;		// manages the levels variuous aspects

	private int levelScore = 0;					// new score at the end of the level or when Gimmick dies		
	private int [] oldLevelScores;				// all of the scores that have been saved
	private int [] newLevelScores;				// all of the scores that have been saved with the addition of the new one

	private string highScorePrefName;
	*/

	public string levelName;					// the name of the level who's score is to be saved
	private LevelManager theLevelManager;		// manages various aspects of the level, notably score
	private string levelScoresKey;				// the name of the key in PlayerPrefs where the score will be saved
	private string levelScoresCountKey;			// the key of the value in PlayerPrefs that says how many scores have been saved

	// Use this for initialization
	void Start () {
		theLevelManager = FindObjectOfType<LevelManager> ();
		levelScoresKey = levelName + "Scores";
		levelScoresCountKey = levelName + "Scores Count";

		/* OLD code
		theLevelManager = FindObjectOfType<LevelManager> ();
		newLevelScores = new int[500];
		highScorePrefName = levelName + " Scores";
		LogScores ();
		*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void SaveScore(){
		if (!PlayerPrefs.HasKey (levelScoresKey + 0)) {
			InitializeScores ();
			Debug.Log ("--- saving to new score list ---");
		} else {
			SaveScoreToExistingList ();
			Debug.Log ("--- saving to existing score list ---");
		}


		/* OLD code
		levelScore = theLevelManager.currentScore;

		// if we have an array of scores saved already, then add this score to them and save the new array
		// else save a new array with just the one score
		if (PlayerPrefs.HasKey (highScorePrefName)) {
			Debug.Log ("New score being saved -- previous scores exist");
			oldLevelScores = PlayerPrefsX.GetIntArray (highScorePrefName);
		

			int i = 0;	
			foreach (int oldScore in oldLevelScores) {
				if (i < 500) {
					newLevelScores [i] = oldScore;
					++i;
				} else {
					Debug.Log ("Not enough room to save more scores");
				}
			}
			if (i < 500) {
				newLevelScores [i] = levelScore;
			} else {
				Debug.Log ("Not enough room to save more scores");
			}

			// save array of new scores
			PlayerPrefsX.SetIntArray (highScorePrefName, newLevelScores);
		} else {
			Debug.Log ("New score being saved -- no previous scores");
			newLevelScores [0] = levelScore;

			// save array of new scores
			PlayerPrefsX.SetIntArray (highScorePrefName, newLevelScores);
		}
		*/
	}


	/* This function will look in the PlayerPrefs and determine the top five scores and return
	 * them in an array.  The array has them in descending order, where the 0 index has the highest score
	 * and the 4 index has the lowest score.
	 */ 
	public int[] GetTopFiveScores(){
		int[] topFive = { 0, 0, 0, 0, 0 };

		if (PlayerPrefs.HasKey (levelScoresCountKey)) {
			int scoreCount = PlayerPrefs.GetInt (levelScoresCountKey);

			for (int i = 0; i < scoreCount; ++i) {
				if (PlayerPrefs.HasKey(levelScoresKey + i)){
					int score = PlayerPrefs.GetInt (levelScoresKey + i);

					if (topFive [4] < score) {
						topFive [4] = score;
					}
					if (topFive [3] < topFive [4]) {
						int temp = topFive [3];
						topFive [3] = topFive [4];
						topFive [4] = temp;
					}
					if (topFive [2] < topFive [3]) {
						int temp = topFive [2];
						topFive [2] = topFive [3];
						topFive [3] = temp;
					}
					if (topFive [1] < topFive [2]) {
						int temp = topFive [1];
						topFive [1] = topFive [2];
						topFive [2] = temp;
					}
					if (topFive [0] < topFive [1]) {
						int temp = topFive [0];
						topFive [0] = topFive [1];
						topFive [1] = temp;
					}

				} // has specific score bracket
			} // looping through scores bracket
		} // has level scores count bracket

		return topFive;
	}

	/* This will initialize the list of scores. */
	private void InitializeScores(){
		PlayerPrefs.SetInt (levelScoresKey + 0, theLevelManager.currentScore);
		PlayerPrefs.SetInt (levelScoresCountKey, 1);
		PlayerPrefs.Save ();
	}

	/* This will be called when there exists previously saved scores for this level.
	 * It will save the score and update the count of scores.
	 */
	private void SaveScoreToExistingList(){
		int scoresCount = PlayerPrefs.GetInt (levelScoresCountKey);  

		PlayerPrefs.SetInt (levelScoresKey + scoresCount, theLevelManager.currentScore);

		scoresCount = scoresCount + 1;

		PlayerPrefs.SetInt (levelScoresCountKey, scoresCount);

		PlayerPrefs.Save ();
	}

	public void LogScores(){
		Debug.Log ("############ ALL SCORES ###########");
		if (PlayerPrefs.HasKey (levelScoresCountKey)) {
			int scoreCount = PlayerPrefs.GetInt (levelScoresCountKey);
			Debug.Log ("level scores count == " + scoreCount);

			for (int i = 0; i < scoreCount; ++i) {
				if (PlayerPrefs.HasKey (levelScoresKey + i)) {
					Debug.Log (PlayerPrefs.GetInt (levelScoresKey + i));
				} else {
					Debug.Log ("No such score key: " + levelScoresKey + i);
				}
			}
		}
		/* OLD code
		if (PlayerPrefs.HasKey(highScorePrefName)){
			Debug.Log ("Found key with name: " + highScorePrefName);
			oldLevelScores = PlayerPrefsX.GetIntArray (highScorePrefName);
//			foreach (int oldScore in oldLevelScores) {
//				Debug.Log (oldScore);
//			}
		} 
		else {
			Debug.Log("No such list of scores");
		}
		*/
	}

	public void LogTopFiveScores(){
		Debug.Log ("########### TOP FIVE SCORES ############");
		int[] topFive = GetTopFiveScores ();
		foreach (int score in topFive) {
			Debug.Log (score);
		}
	}
}
