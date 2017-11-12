using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/* place first level into first level and the select level scene into levelSelect */
public class MainMenu : MonoBehaviour
{

    public string firstLevel;
    public string levelSelect;
	public string HSLevelSelect;
    public string[] levelNames;
    public Color loadToColor = Color.white;
    public int fadeSpeed;
    public Slider[] volumeSliders;
	private const string maxHealthKey = "MaxHealth";    // has to match whats in LevelManager.cs
    public AudioSource MainMusic;
 



    // Must match what gets saved in BonusHeart.cs script
    private const string caveBHFoundKey = "CaveLevelBonusHeartCollected";	
	private const string factoryBHFoundKey = "Factory LevelBonusHeartCollected";
	private const string seasideBHFoundKey = "Seaside LevelBonusHeartCollected";
	private const string forestBHFoundKey = "Forest LevelBonusHeartCollected";


    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject ControlMenuHolder;
    // Use this for initialization
    void Start()
    {
        AudioManager.instance.PlayMusic(MainMusic);
        volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
        volumeSliders[2].value = AudioManager.instance.soundEffectsVolumePercent;

         
       
        AudioManager.instance.SetVolume(volumeSliders[0].value, AudioManager.AudioChannel.Master);
        AudioManager.instance.SetVolume(volumeSliders[1].value, AudioManager.AudioChannel.Music);
        AudioManager.instance.SetVolume(volumeSliders[2].value, AudioManager.AudioChannel.SoundEffect);


    }

    // Update is called once per frame
    void Update()
    {
       
    }
    // This loads the first level and locks all but the first level for the level select menu
    // In this instence we are unlocking all levels
    public void NewGame()
    {
        //SceneManager.LoadScene(firstLevel);// loads the first level
        ScreenTransition.FadeScreen(firstLevel, loadToColor, fadeSpeed);
        Time.timeScale = 1f;
        // For regular gaming set the int to 0 to lock all but the first level
        for (int i = 0; i < levelNames.Length; i++)
        {
            PlayerPrefs.SetInt(levelNames[i], 1);
        }
			
		// set max health to six and Bonus Hearts collected to false (zero) for each level
		PlayerPrefs.SetInt (maxHealthKey, 6);
		PlayerPrefs.SetInt (caveBHFoundKey, 0);
		PlayerPrefs.SetInt (factoryBHFoundKey, 0);
		PlayerPrefs.SetInt (seasideBHFoundKey, 0);
		PlayerPrefs.SetInt (forestBHFoundKey, 0);

    }
    //This loads the Level Select menu
    public void Continue()
    {
        //SceneManager.LoadScene(levelSelect);
        ScreenTransition.FadeScreen(levelSelect, loadToColor, fadeSpeed);
        Time.timeScale = 1f;
    }
    //This Quits the application
    public void QuitGame()
    {
        Application.Quit();

    }

	public void HighScores(){
        SceneManager.LoadScene (HSLevelSelect);
        //ScreenTransition.FadeScreen(HSLevelSelect, loadToColor, fadeSpeed);
    }
    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }
    public void MainMenuTrue()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }
    public void MainFromControl()
    {
        mainMenuHolder.SetActive(true);
        ControlMenuHolder.SetActive(false);
    }
    public void ControlsMenu()
    {
        mainMenuHolder.SetActive(false);
        ControlMenuHolder.SetActive(true);
    }
   

    public void SetMasterVolume(float value)
    {
        value = volumeSliders[0].value;
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }
    public void SetMusicVolume(float value)
    {
        value = volumeSliders[1].value;
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }
    public void SetSoundEffectsVolume(float value)
    {
        value = volumeSliders[2].value;
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.SoundEffect);
    }
}
