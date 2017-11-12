using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour {
   public static void FadeScreen(string sceneToLoad, Color colorFade, float damp)
    {
        GameObject init = new GameObject();
        init.name = "Fader";
        init.AddComponent<Fade>();
        Fade screen = init.GetComponent<Fade>();
        screen.fadeDamp = damp;
        screen.NextLevelScreen = sceneToLoad;
        screen.ColorToFade = colorFade;
        screen.start = true;
		Debug.Log ("end of fadescreen");
    }
}
