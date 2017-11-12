//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopulateHighScores : MonoBehaviour {

	public Text highScoreList;					// Text object that will hold the high scores

	public string levelName;					// name of the level who's high score are to be populated
	public string goBackScene;					// scene that will be loaded when the "Go Back" button will be clicked

	private string levelScoresKey;				// the name of the key in PlayerPrefs where the score will be saved
	private string levelScoresCountKey;			// the key of the value in PlayerPrefs that says how many scores have been saved

	// Use this for initialization
	void Start () {
		// create keys that were used in the prefabs for saving the scores
		levelScoresKey = levelName + "Scores";
		levelScoresCountKey = levelName + "Scores Count";

		// obtain top five scores that were saved in the prefabs 
		int[] topFiveScores = GetTopFiveScores ();

		string textOfTopScores;

		textOfTopScores = "1. " + topFiveScores[0] + "\n";
		textOfTopScores += "2. " + topFiveScores [1] + "\n";
		textOfTopScores += "3. " + topFiveScores [2] + "\n";
		textOfTopScores += "4. " + topFiveScores [3] + "\n";
		textOfTopScores += "5. " + topFiveScores [4] + "\n";

		// populate the scene with the high scores
		highScoreList.text = textOfTopScores;


	}
	
	// Update is called once per frame
	void Update () {
		
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

	public void GoBack(){
		SceneManager.LoadScene (goBackScene);
	}
}
