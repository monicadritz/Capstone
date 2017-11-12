using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/* when you add this to the scene add canvas from the prefabs folder and drag from under the canvas->heart holder move heart  into heart 1 heart(1) into heart 2 and heart (2) into heart 3 
 ** Also drag in canvas-> pointText into the PointText in the level manager
 */
public class LevelManager : MonoBehaviour
{

    public float waitToRespawn;//time the game waits to respawn
    public GimmickController Gimmick;//gives access to Gimmick
    public Text PointText;// UI points element
    public int currentScore = 0;

    //health sprites system
    public Image heart1;
    public Image heart2;
    public Image heart3;
	public Image heart4; // bonus heart
	public Image heart5; // bonus heart
	public Image heart6; // bonus heart
	public Image heart7; // bonus heart 
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;
	public Sprite invisibleItem; 

    public int maxHealth;//max health the player has
    public int healthCount;//health count how much health Gimmick currently has
    public bool Invincible;// has he just been hit?
	public float flashTimer;// time for which Gimmick will be flashing, or <= 0 if not flashing
	private SpriteRenderer spriteRenderer;// Gimmick's sprite renderer
    private bool respawning;// is he currently respawning?
    public GameObject deathsplosion;// particle effect when Gimmick dies
    public GameObject gameOverScreen;//ability to activeate the game over screen 
    //public AudioClip coinSound;
    //public AudioClip heartSound;
   // public AudioClip levelMusic;
    public AudioSource GameOverMusic;
    public AudioSource MainMusic;
	public AudioSource bossMusic;
	public GameObject levelEnd;



    private HighScore theHighScore;

	public string maxHealthKey = "MaxHealth";			//+ key used in PlayerPrefs to store max health of Gimmick

    // Use this for initialization
    void Start()
    {
        Gimmick = FindObjectOfType<GimmickController>();
		levelEnd = GameObject.Find ("LevelEnd");
		if (SceneManager.GetActiveScene ().name == "Forest Level")
			levelEnd.GetComponent<SpriteRenderer>().enabled = false;
        AudioManager.instance.PlayMusic(MainMusic);
        if (PlayerPrefs.HasKey(maxHealthKey)){
			maxHealth = PlayerPrefs.GetInt (maxHealthKey);
		}
		healthCount = maxHealth;

		flashTimer = 0f;

		spriteRenderer = Gimmick.gameObject.GetComponent<SpriteRenderer> ();

        PointText.text = "Current Score: " + currentScore;

		theHighScore = FindObjectOfType<HighScore> ();

		UpdateHeartMeter ();
    }

    // Update is called once per frame
    void Update()
    {
        /* This is if we decide to have checkpoints and or a lives system
        if (healthCount<=0 && !respawning)
        {
            Respawn();
            respawning = true;
        }
        */

		updateFlashEffect ();
    }
    //This adds points to the Current Score 
    public void addPoints(int pointsToAdd)
    {
       
        currentScore += pointsToAdd;
        PointText.text = "Current Score: " + currentScore;
    }
    //Starts Repawinging coroutine if gimmick dies
    public void Respawn()
    {

        StartCoroutine("RespawnCo");
    }
    // Deactivates Gimmick from the screen, Shows the particle effect of his death and loads the game over screen
    public IEnumerator RespawnCo()
    {
        Gimmick.gameObject.SetActive(false);
        AudioManager.instance.PlaySound2D("Explosion");
        Instantiate(deathsplosion, Gimmick.transform.position, Gimmick.transform.rotation);
        yield return new WaitForSeconds(waitToRespawn);
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
		theHighScore.SaveScore ();   // save the score of the player
		bossMusic.Stop();
        AudioManager.instance.ChangeMusic(GameOverMusic, 5);
       // levelMusic.Stop();
        //GameOverMusic.Play();
    }
    //If Gimmick runs into an enemy or danger and takes damage
    public void HurtPlayer(int damageToTake)
    {
        //If Gimmick isn't Invincible decrease health 
        if (!Invincible)
        {
            healthCount -= damageToTake;
            UpdateHeartMeter();
            //Gimmick.hurtSound.Play();
            // AudioManager.instance.PlaySound(Gimmick.hurtSound, transform.position);
            //AudioManager.instance.PlaySound("Hurt", transform.position);
            AudioManager.instance.PlaySound2D("Hurt");
            if (healthCount <= 0)
				Respawn ();
			Invincible = true;
			StartCoroutine ("HurtPlayerCo");
        }
    }

	//PRECONDITION: flashTimer must be set with the time to be invincible just before calling this.
	private IEnumerator HurtPlayerCo(){
		// make player immune to damage for a duration
		yield return new WaitForSeconds (flashTimer);
		Invincible = false;
	}
		
    //This is for Items that give Gimmick health
    public void GiveHealth(int healthToGive)
    {
       
        healthCount += healthToGive;
        //heartSound.Play();
        // AudioManager.instance.PlaySound(heartSound, transform.position);
        //AudioManager.instance.PlaySound("1-UP", transform.position);
        AudioManager.instance.PlaySound2D("1-UP");
        
        if (healthCount > maxHealth)
        {
            healthCount = maxHealth;
        }
        UpdateHeartMeter();

    }
    //This updates the heart images that represent health on the players screen
    public void UpdateHeartMeter()
    {

		if (PlayerPrefs.HasKey (maxHealthKey)) {
			switch (PlayerPrefs.GetInt (maxHealthKey)) {
			case 6:
				sixHealth ();
                maxHealth = 6;
				return;
			case 8:
				eightHealth ();
                maxHealth = 8;
				return;
			case 10:
				tenHealth ();
                maxHealth = 10;
                return;
			case 12:
				twelveHealth ();
                maxHealth = 12;
				return;
			case 14:
				fourteenHealth ();
                maxHealth = 14;
                return;
			default:
				sixHealth ();
				Debug.Log ("Default in UpdateHeartMeter() being called");
				return;
			}
		} else {
			sixHealth ();
			Debug.Log ("maxHealthKey not found");
		}
    }

	private void sixHealth(){
		switch (healthCount)
		{
		case 6:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = invisibleItem;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 5:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartHalf;
			heart4.sprite = invisibleItem;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 4:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartEmpty;
			heart4.sprite = invisibleItem;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 3:
			heart1.sprite = heartFull;
			heart2.sprite = heartHalf;
			heart3.sprite = heartEmpty;
			heart4.sprite = invisibleItem;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 2:
			heart1.sprite = heartFull;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = invisibleItem;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 1:
			heart1.sprite = heartHalf;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = invisibleItem;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 0:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = invisibleItem;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		default:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = invisibleItem;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		}
	}

	private void eightHealth(){
		switch (healthCount) {
		case 8:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 7:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartHalf;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 6:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartEmpty;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 5:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartHalf;
			heart4.sprite = heartEmpty;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 4:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 3:
			heart1.sprite = heartFull;
			heart2.sprite = heartHalf;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 2:
			heart1.sprite = heartFull;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 1:
			heart1.sprite = heartHalf;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 0:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		default:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = invisibleItem;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;

		}
	}

	private void tenHealth(){
		switch (healthCount) {
		case 10:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartFull;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 9:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartHalf;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 8:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 7:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartHalf;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 6:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 5: 
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartHalf;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 4:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 3:
			heart1.sprite = heartFull;
			heart2.sprite = heartHalf;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 2:
			heart1.sprite = heartFull;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 1:
			heart1.sprite = heartHalf;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		case 0:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		default:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = invisibleItem;
			heart7.sprite = invisibleItem;
			return;
		}
	}

	private void twelveHealth(){
		switch (healthCount) {
		case 12:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartFull;
			heart6.sprite = heartFull;
			heart7.sprite = invisibleItem;
			return;
		case 11:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartFull;
			heart6.sprite = heartHalf;
			heart7.sprite = invisibleItem;
			return;
		case 10:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartFull;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 9:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartHalf;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 8:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 7:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartHalf;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 6:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 5:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartHalf;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 4:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 3:
			heart1.sprite = heartFull;
			heart2.sprite = heartHalf;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 2:
			heart1.sprite = heartFull;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 1:
			heart1.sprite = heartHalf;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		case 0:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		default:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = invisibleItem;
			return;
		}
	}

	private void fourteenHealth(){
		switch (healthCount) {
		case 14:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartFull;
			heart6.sprite = heartFull;
			heart7.sprite = heartFull;
			return;
		case 13:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartFull;
			heart6.sprite = heartFull;
			heart7.sprite = heartHalf;
			return;
		case 12: 
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartFull;
			heart6.sprite = heartFull;
			heart7.sprite = heartEmpty;
			return;
		case 11:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartFull;
			heart6.sprite = heartHalf;
			heart7.sprite = heartEmpty;
			return;
		case 10:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartFull;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 9:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartHalf;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 8:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartFull;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 7:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartHalf;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 6:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 5:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartHalf;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 4:
			heart1.sprite = heartFull;
			heart2.sprite = heartFull;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 3:
			heart1.sprite = heartFull;
			heart2.sprite = heartHalf;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 2:
			heart1.sprite = heartFull;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 1:
			heart1.sprite = heartHalf;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		case 0:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		default:
			heart1.sprite = heartEmpty;
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			heart4.sprite = heartEmpty;
			heart5.sprite = heartEmpty;
			heart6.sprite = heartEmpty;
			heart7.sprite = heartEmpty;
			return;
		}
	}

	public void updateFlashEffect() {
		if (flashTimer > 0)
			flashTimer -= Time.deltaTime;
		if (flashTimer > 0) {
			float flash = (flashTimer * 4) - (Mathf.Floor (flashTimer * 4)) + 0.35f;
			if (flash > 0.85f)
				flash = 1.7f - flash;
			//Debug.Log ("Timer: " + flashTimer + ", Flash: " + flash);
			spriteRenderer.color = new Color (1f, 1f, 1f, flash);
		}
		else
			spriteRenderer.color = new Color (1f, 1f, 1f, 1f);

		//Debug.Log ("alpha: " + spriteRenderer.color.a);
	}

}
