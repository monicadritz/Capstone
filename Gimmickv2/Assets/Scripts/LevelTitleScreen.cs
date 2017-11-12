using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTitleScreen : MonoBehaviour {
    //next  level scene we want to load
    public string nextScene;
    //color to use to load
    public Color loadToColor = Color.white;
    public int fadeSpeed;
    public float timeToWait;
	
    void Update()
    {
        if(timeToWait>0)
        {
            timeToWait -= Time.deltaTime;
        }
        else
        {
            ScreenTransition.FadeScreen(nextScene, loadToColor, fadeSpeed);
        }
        
    }
}
