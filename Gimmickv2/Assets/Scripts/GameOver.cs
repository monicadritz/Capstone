using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour
{
    public string levelSelect;
    public string mainMenu;
    private LevelManager theLevelManager;
    public Color loadToColor = Color.white;
    public int fadeSpeed;
    private BonusHeart bonusHeartKey;
    private EventSystem theEventSystem;

    // Use this for initialization
    void Start()
    {
        theEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        theLevelManager = FindObjectOfType<LevelManager>();
        theEventSystem.SetSelectedGameObject(null);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Restart")&& isActiveAndEnabled)
        {
            Restart();
        }
        if(Input.GetButton("LevelSelect") && isActiveAndEnabled)
        {
            LevelSelect();
        }
        if(Input.GetButton("Quit") && isActiveAndEnabled)
        {
            QuitToMainMenu();
        }
        if (theEventSystem.currentSelectedGameObject == null )
        {
            theEventSystem.SetSelectedGameObject(GameObject.Find("Restart"));
        }
       

    }
    // restarts the player from the begining of the level
    public void Restart()
    {


        PlayerPrefs.SetInt("Current Score: ", 0);//sets the current score back to 0
        ScreenTransition.FadeScreen(SceneManager.GetActiveScene().name, loadToColor, fadeSpeed);
    
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);// gets the current scenes name
        Time.timeScale = 1f;
    }
    //loads the level select menu 
    public void LevelSelect()
    {
        PlayerPrefs.SetInt("Current Score: ", 0);//sets the current score back to 0
        ScreenTransition.FadeScreen(levelSelect, loadToColor, fadeSpeed);
       // SceneManager.LoadScene(levelSelect);// loads the levelSelect level
		Time.timeScale = 1f;
    }

    // loads the Main Menu screen
    public void QuitToMainMenu()
    {
        
       //ScreenTransition.FadeScreen(mainMenu, loadToColor, fadeSpeed);
        SceneManager.LoadScene(mainMenu);//loads the main menu scene
    }
}
