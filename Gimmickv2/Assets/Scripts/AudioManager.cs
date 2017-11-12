using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public enum AudioChannel { Master, SoundEffect, Music}
    public float masterVolumePercent { get; set; }
    public float soundEffectsVolumePercent { get; set; }
    public float musicVolumePercent { get; set; }
    AudioSource musicSources;
    AudioSource soundEffectSource;
   

    public static AudioManager instance;
    Transform audioListener;
    Transform GimmickTransform;

    SoundLibrary library;
    

    void Awake()
    {
        //loads the saved volume settings 
        PlayerPrefs.GetFloat("Master Vol", masterVolumePercent);
        PlayerPrefs.GetFloat("SoundEffects Vol", soundEffectsVolumePercent);
        PlayerPrefs.GetFloat("Music Vol", musicVolumePercent);
   
        //checks to see if there is already an audio manager in the scene
        if (instance !=null)
        {
            Destroy(gameObject);
        }
        //if not it sets up the audio manager
        else
        {
            //allows it to be visible by other scripts 
            instance = this;
            //doesn't destroy this audio manager if there isn't one in the new scene
            DontDestroyOnLoad(gameObject);
            //finds the sound library
            library = GetComponent<SoundLibrary>();
            //sets up a new game opject for the sound effects
            GameObject newSoundEffectSource = new GameObject("SoundEffect source");
            //sets the new game object to the audiosource in the scene
            soundEffectSource = newSoundEffectSource.AddComponent<AudioSource>();
            newSoundEffectSource.transform.parent = transform;

            //finds and assigns the audiolistener
            audioListener = FindObjectOfType<AudioListener>().transform;
            //checks if gimmick is in the scene if its not it doesnt set him 
            if(FindObjectOfType<GimmickController>()!=null)
            {
                GimmickTransform = FindObjectOfType<GimmickController>().transform;
            }
            //sets the volumes from the player prefs 
            masterVolumePercent = PlayerPrefs.GetFloat("Master Vol", 1.0f);
            soundEffectsVolumePercent = PlayerPrefs.GetFloat("SoundEffects Vol", 1.0f);
            musicVolumePercent = PlayerPrefs.GetFloat("Music Vol", 1.0f);
        }          
    }
    
    //sets the volumes
    public void SetVolume(float volumePercent, AudioChannel channel)
    {       

        switch (channel) {
            case AudioChannel.Master:
                 masterVolumePercent = volumePercent;
                break;

            case AudioChannel.SoundEffect:
                soundEffectsVolumePercent = volumePercent;
                break;

            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
        }
        musicSources.volume = musicVolumePercent * masterVolumePercent;
        // saves the players volume levels
        PlayerPrefs.SetFloat("Master Vol", masterVolumePercent);
        PlayerPrefs.SetFloat("SoundEffects Vol",soundEffectsVolumePercent);
        PlayerPrefs.SetFloat("Music Vol", musicVolumePercent);
        PlayerPrefs.Save();
    }
    // plays the level music at the level specified by the user
    public void PlayMusic(AudioSource music)
    {
        music.volume = musicVolumePercent * masterVolumePercent;
        musicSources = music;
        
        musicSources.Play();
    }
    //changes the music when called 
    public void ChangeMusic(AudioSource music, float fadeDuration=1)
    {
        musicSources.Stop();
        music.Play();
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            music.volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources.volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);

        }
    }
   
    //plays the sound effects when called
    public void PlaySound2D(string soundName)
    {
        soundEffectSource.PlayOneShot(library.GetClipFromName(soundName), soundEffectsVolumePercent * masterVolumePercent);
    }
   
}
