using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {
    public string levelSelect;
    public string mainMenu;
    private LevelManager theLevelManager;
    public GameObject thePauseScreen;
    private GimmickController thePlayer;
    public GameObject theGameOverScreen;


    // Use this for initialization
    void Start () {
        theLevelManager = FindObjectOfType<LevelManager>();
        thePlayer = FindObjectOfType<GimmickController>();
        

    }
	
	// Update is called once per frame
	void Update () {
        //if the esc key is pressed this script is activated 
        if (Input.GetKeyDown(KeyCode.Escape) && !theGameOverScreen.active)
        {
            //if the timescale has already been set to 0 then resume the game 
            if (Time.timeScale == 0)
            {
                //  theLevelManager.levelMusic.volume = theLevelManager.levelMusic.volume*2; // old way of returning sound to full volume
                //new way to half the volume
                float p = AudioManager.instance.masterVolumePercent;
                AudioManager.instance.SetVolume(p * 2, AudioManager.AudioChannel.Master);
                ResumeGame();
            }
            // if timescale is not 0 pause the game 
            else
            {
                // theLevelManager.levelMusic.volume = theLevelManager.levelMusic.volume / 2f;// old way of halfing the volume
                // new way to half the volume             
                float p = AudioManager.instance.masterVolumePercent;
                AudioManager.instance.SetVolume(p / 2, AudioManager.AudioChannel.Master);
                PauseGame();
            }

        }
        if (Input.GetButton("Restart")&& Time.timeScale==0)
        {
            ResumeGame();
        }
        if (Input.GetButton("LevelSelect") && Time.timeScale == 0)
        {
            LevelSelect();
        }
        if (Input.GetButton("Quit") && Time.timeScale == 0)
        {
            QuitToMainMenu();
        }

    }
    //This pauses the game and sets the screen to be active, slows down the time of the level and freezes the player
    public void PauseGame()
    {
      
      
        Time.timeScale = 0;
        thePauseScreen.SetActive(true);
        thePlayer.canMove = false;
  
    }
    //This Resumes the game, unfreezes the player and sets the timescale back to one 
    public void ResumeGame()
    {
      
       Time.timeScale = 1f;
        thePauseScreen.SetActive(false);
        thePlayer.canMove = true;
       


    }
    public void LevelSelect()
    {
        PlayerPrefs.SetInt("CurrentScore", theLevelManager.currentScore);

        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelect);

    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }
}
