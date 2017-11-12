using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public enum AudioChannel { Master, SoundEffect, Music}
    public float masterVolumePercent { get; private set; }
    public float soundEffectsVolumePercent = 1.0f;
    public float musicVolumePercent = 1.0f; 
    AudioSource musicSources;
    AudioSource soundEffectSource;
   

    public static AudioManager instance;
    Transform audioListener;
    Transform GimmickTransform;

    SoundLibrary library;
    

    void Awake()
    {
        PlayerPrefs.GetFloat("Master Vol", masterVolumePercent);
        PlayerPrefs.GetFloat("SoundEffects Vol", masterVolumePercent);
        PlayerPrefs.GetFloat("Music Vol", masterVolumePercent);
      //  PlayerPrefs.Save();

        if (instance !=null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            library = GetComponent<SoundLibrary>();

            //musicSources = new AudioSource[2];
            //for (int i = 0; i < 2; i++)
            //{
            //    GameObject newMusicSource = new GameObject("Music source" + (i + 1));
            //    musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            //    newMusicSource.transform.parent = transform;
            //}
            GameObject newSoundEffectSource = new GameObject("SoundEffect source");
            soundEffectSource = newSoundEffectSource.AddComponent<AudioSource>();
            newSoundEffectSource.transform.parent = transform;
            audioListener = FindObjectOfType<AudioListener>().transform;
            if(FindObjectOfType<GimmickController>()!=null)
            {
                GimmickTransform = FindObjectOfType<GimmickController>().transform;
            }

            masterVolumePercent = PlayerPrefs.GetFloat("Master Vol", 1.0f);
            soundEffectsVolumePercent = PlayerPrefs.GetFloat("SoundEffects Vol", 1.0f);
            musicVolumePercent = PlayerPrefs.GetFloat("Music Vol", 1.0f);

        }    
        
    }
    void Update()
    {
        //if (GimmickTransform != null)
        //{
        //    audioListener.position = GimmickTransform.position;
        //}
    }

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
      //  musicSources[1].volume = musicVolumePercent * masterVolumePercent;
        PlayerPrefs.SetFloat("Master Vol", masterVolumePercent);
        PlayerPrefs.SetFloat("SoundEffects Vol", masterVolumePercent);
        PlayerPrefs.SetFloat("Music Vol", masterVolumePercent);
        PlayerPrefs.Save();
    }
    public void PlayMusic(AudioSource music)
    {
        musicSources = music;
        musicSources.Play();
    }
    public void ChangeMusic(AudioSource music, float fadeDuration=1)
    {
        //musicSources.Stop();
        music.Play();
        //PlayMusic(music);
        //MainMusic.Stop();
        //GameOver.Play();
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            music.volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent*100, percent);
            musicSources.volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent*100, 0, percent);

        }
    }
   

    public void PlaySound2D(string soundName)
    {
        soundEffectSource.PlayOneShot(library.GetClipFromName(soundName), soundEffectsVolumePercent * masterVolumePercent);
    }
   
}
