using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelDoor : MonoBehaviour
{

    public string levelToLoad;//string of the scene name for the level to load next
    public bool unlocked;// true or false value that tells if the player has unlocked the next level
    public Sprite doorBottomOpen;// sprite holder for the open bottom door
    public Sprite doorTopOpen;// sprite holder for the open top door
    public Sprite doorBottomClosed;// sprite holder for the closed bottom door
    public Sprite doorTopClosed;// sprite holder for the closed top door
    public SpriteRenderer doorTop;// spriteRenderer for the scene gameobject  
    public SpriteRenderer doorBottom;// spriteRenderer for the scene gameobject 
    public Color loadToColor = Color.white;
    public int fadeSpeed;

    // Use this for initialization
    void Start()
    {
        //Sandboxes are holders for right now 
        PlayerPrefs.SetInt("Cave Level", 1);//sets the Cave level to unlocked
        PlayerPrefs.SetInt("Seaside Level", 1);//sets seaside levelto unlocked
        PlayerPrefs.SetInt("Factory Level", 1);// sets Factory level to unlocked 
        PlayerPrefs.SetInt("Forest Level", 1);// sets Forest level to unlocked 
        PlayerPrefs.SetInt("Main Menu", 1);//sets main menu to unlocked 
        //actually sets the bool value 
        if (PlayerPrefs.GetInt(levelToLoad) == 1)
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }
        //checks the bool value and the correct sprites 
        //if open open door sprite
        if (unlocked)
        {
            doorTop.sprite = doorTopOpen;
            doorBottom.sprite = doorBottomOpen;
        }
        //if closed closed door sprite
        else
        {
            doorTop.sprite = doorTopClosed;
            doorBottom.sprite = doorBottomClosed;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    //each door has a collider that is a trigger 
    void OnTriggerStay2D(Collider2D other)
    {
        //if gimmick hits the doors trigger 
        if (other.tag == "Gimmick")
        {
            //and if gimmick jumps while triggering the doors trigger
            if (Input.GetButtonDown("Go into Door"))
            {
                //load to the coresponding level
                //SceneManager.LoadScene(levelToLoad);
                ScreenTransition.FadeScreen(levelToLoad, loadToColor, fadeSpeed);
            }
        }
    }
}
