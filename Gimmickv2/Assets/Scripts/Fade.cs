using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Fade : MonoBehaviour {

    public bool start = false;
    public float fadeDamp = 0.0f;
    public string NextLevelScreen;
    public float alpha = 0.0f;
    public Color ColorToFade;
    public bool isFadingIn = false;
	public float globalTimer; //globalTimer and prevTimer record the real-time on consecutive OnGUI events; their difference is used for lerping the alpha
	public float prevTimer;


	
	void OnEnable () {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
		globalTimer = Time.realtimeSinceStartup;
	}
	
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
	

	void OnGUI() {
        if(!start)
        {
            return;
        }

		prevTimer = globalTimer;
		globalTimer = Time.realtimeSinceStartup;

        //assign the color with variable alpha
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        //create a temp texture
        Texture2D newTex;
        newTex = new Texture2D(1, 1);
        newTex.SetPixel(0, 0, ColorToFade);
        newTex.Apply();
        //print texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), newTex);

        if (isFadingIn)
        {
			Debug.Log ("Timer: " + (globalTimer - prevTimer));
			alpha = Mathf.Lerp(alpha, -0.1f, fadeDamp * (globalTimer - prevTimer));
         }
        else
        {
			alpha = Mathf.Lerp(alpha, 1.1f, fadeDamp * (globalTimer - prevTimer));
        
        }
        if(alpha >=1 && !isFadingIn)
        {
            SceneManager.LoadScene(NextLevelScreen);
			Time.timeScale = 1f;
            DontDestroyOnLoad(gameObject);
        }
        else if (alpha<=0 && isFadingIn)
        {
            Destroy(gameObject);
        }
        

    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //We can now fade in
		globalTimer = Time.realtimeSinceStartup; //refresh globalTimer after long GUI interrupt from level load
        isFadingIn = true;
    }

}
