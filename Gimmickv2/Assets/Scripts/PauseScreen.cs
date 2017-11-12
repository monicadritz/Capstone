using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseScreen : MonoBehaviour {
    public string levelSelect;
    public string mainMenu;
    private LevelManager theLevelManager;
    public GameObject thePauseScreen;
    private GimmickController thePlayer;
    public GameObject theGameOverScreen;
    private EventSystem theEventSystem;


    // Use this for initialization
    void Start () {
        theLevelManager = FindObjectOfType<LevelManager>();
        thePlayer = FindObjectOfType<GimmickController>();
        theEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

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
             
                ResumeGame();
            }
            // if timescale is not 0 pause the game 
            else
            {
                // theLevelManager.levelMusic.volume = theLevelManager.levelMusic.volume / 2f;// old way of halfing the volume
                // new way to half the volume             
                
                PauseGame();
            }

        }
        // this is looking for restart because that is the input that was created for the gameover screen
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

        if (theEventSystem.currentSelectedGameObject == null)
        {
            theEventSystem.SetSelectedGameObject(GameObject.Find("Resume"));
        }

    }
    //This pauses the game and sets the screen to be active, slows down the time of the level and freezes the player
    public void PauseGame()
    {
        
        Time.timeScale = 0;
        thePauseScreen.SetActive(true);
        thePlayer.canMove = false;
        float p = AudioManager.instance.masterVolumePercent;
        AudioManager.instance.SetVolume(p / 2, AudioManager.AudioChannel.Master);
        theEventSystem.SetSelectedGameObject(null);
       

    }
    //This Resumes the game, unfreezes the player and sets the timescale back to one 
    public void ResumeGame()
    {
        
       Time.timeScale = 1f;
        thePauseScreen.SetActive(false);
        thePlayer.canMove = true;
        float p = AudioManager.instance.masterVolumePercent;
        AudioManager.instance.SetVolume(p * 2, AudioManager.AudioChannel.Master);



    }
    public void LevelSelect()
    {
        PlayerPrefs.SetInt("CurrentScore", theLevelManager.currentScore);

        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelect);
        float p = AudioManager.instance.masterVolumePercent;
        AudioManager.instance.SetVolume(p * 2, AudioManager.AudioChannel.Master);

    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
        float p = AudioManager.instance.masterVolumePercent;
        AudioManager.instance.SetVolume(p * 2, AudioManager.AudioChannel.Master);
    }
}
