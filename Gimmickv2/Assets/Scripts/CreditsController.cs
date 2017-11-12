using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour {
	public AudioSource creditsMusic;

	public GameObject background;
	public GameObject ground;
	public GameObject gimmickOnSkateboard;
	public GameObject canvas;
	public Rigidbody2D myRigidbody;

	public const float GROUNDMOVESPEED = 6f;
	public const float GROUNDMINX = -3f;
	public const float GROUNDLOOPSIZE = 4f;

	public Sprite sprite1, sprite2, sprite3, sprite4;
	public SpriteRenderer renderer;

	public const float BACKMOVESPEED = .35f;
	public int currentSprite;
	public const float SPRITE2TRANS = -3.6875f;
	public const float SPRITE2DIFF = 11.8125f;
	public const float SPRITE3TRANS = -5.6875f;
	public const float SPRITE3DIFF= 9.5625f;
	public const float SPRITE4TRANS = -2.1875f;
	public const float SPRITE4DIFF = 9.8125f;

	public float creditsTimer;
	public int nextLine;
	public const float TIMEBETWEENLINES = 0.6f;
	public const float WAITFORNEWBLOCK = 0.9f;
	public const float TIMETOTHEENDTEXT = 2.5f;
	public float timeToNextLine;
	public string[] credits;
	public int creditsSize;
	public GameObject textBox;

	public float globalTimer;
	public const float TIMETOSCROLL = 110f;
	public const float TIMETOEXIT = 115f;
	public bool gimmickMoved;

	public string postCreditsSceneName;

	// Use this for initialization
	void Start () {
		creditsMusic = GameObject.Find ("Credits Music").GetComponent<AudioSource> ();
		creditsMusic.volume *= (PlayerPrefs.GetFloat("Master Vol") * PlayerPrefs.GetFloat("Music Vol"));
		creditsMusic.Play ();
		myRigidbody = gimmickOnSkateboard.GetComponent<Rigidbody2D> ();
		renderer = background.GetComponent<SpriteRenderer> ();
		renderer.sprite = sprite1;
		currentSprite = 1;
		nextLine = 0;
		timeToNextLine = TIMEBETWEENLINES;
		creditsTimer = TIMEBETWEENLINES;
		credits = new string[102] { "CONGRATULATIONS!", 
			"You've defeated the vile Squirrelbot", 
			"and restored peace to the world.",
			"Consider yourself a hero.",
			"----------CREDITS----------", 
			"-------APPLICATION DEVELOPMENT-------",
			"Robert McGuigan",
			"Monica Pineda",
			"Bilal Saleem",
			"-------CORE GAMEPLAY CONCEPTS-------",
			"Sunsoft (Mr. Gimmick)",
			"-------SRITES AND BACKGROUNDS-------",
			"---ALPHADREAM, GOOD-FEEL---",
			"Crab (Mario & Luigi: Dream Team)",
			"---ANGEVON'S RO SPRITES ARCHIVE---",
			"Kraken Tentacle",
			"---BEN BAKKER---",
			"Cave Exterior",
			"---CAPCOM---",
			"Explosions (Bionic Commando)",
			"Credits Background (Final Fight)",
			"Conveyor Belt (Mega Man 2)",
			"Various Stage Elements (Mega Man 6)",
			"---EMROX (NEWGROUNDS)---",
			"Title Art",
			"---EVERPLAY INTERACTIVE---",
			"Forest Level Background (Spellsword)",
			"---GAME GARDEN---",
			"Palm Tree (Fairy Farm)",
			"---HAL---",
			"Cloud (Kirby Super Star)",
			"---LJN---",
			"Skateboard (Town & Country Surf Design: Wood & Water Rage)",
			"---MFO WIKI---",
			"Jumping Seagull",
			"---NAMCO---",
			"Turret (Xevious)",
			"---NINTENDO---",
			"Blue Shell (Mario Kart Super Circuit)",
			"City Street (Mike Tyson's Punch-Out!!)",
			"Bullet (StarTropics)",
			"Mushroom (Super Mario All-Stars)",
			"Koopa Troopa, Fire Bar Concept (Super Mario Bros)",
			"Spiky Spikes (Super Mario Bros 2)",
			"Various Stage Elements (Super Mario Bros 3)",
			"---OPENGAMEART---",
			"Various Stage Elements (Plee the Bear)",
			"Water",
			"---PRIVATIA, PUMPCHI STUDIO---",
			"Flying Fish (Trickster Online Revolution)",
			"---RARE---",
			"Blue Robot (Battletoads and Double Dragon)",
			"---RYKY (DEVIANTART)---",
			"Water Droplets",
			"---SEGA---",
			"Fireball (Ghostbusters)",
			"Seaside Level Background (Sonic Lost World)",
			"Factory Level Background, Various Stage Elements (Sonic the Hedgehog)",
			"---SIPUT SCUBA---",
			"Flying Seagull",
			"---SUNSOFT---",
			"Gimmick, Star, Title Art (Mr. Gimmick)",
			"SquirrelBot (Zero the Kamikaze Squirrel)",
			"---TRIBUTE GAMES---",
			"Bear (Curses N' Chaos)",
			"---UDEMY---",
			"Launcher, Moving Platform, Spikes, Stalactite, Coin, Heart, Fish, Various Stage Elements:",
			"\"Learn To Code By Making a 2D Platformer\"",
			"---VIZOR INTERACTIVE---",
			"Tree (Zombie Island)",
			"-------MUSIC-------",
			"---RARE---",
			"Cave Level, Victory (Battletoads)",
			"---SQUARE---",
			"Credits (Rad Racer II)",
			"---SUNSOFT---",
			"Boss Theme (Batman: The Video Game)",
			"Factory Level (Journey to Silius)",
			"Forest Level (Ufouria: The Saga)",
			"---TREVOR LENTZ---",
			"Seaside Level (Guinea Pig Hero)",
			"-------SOUND EFFECTS-------",
			"---COMPILE---",
			"Explosions and Gunfire (Gun-Nac)",
			"---KONAMI---",
			"Explosions (Contra)",
			"---NINTENDO---",
			"Jump, Health Get (Super Mario World)",
			"---SUNSOFT---",
			"Star Impact (Ufouria: The Saga)",
			"---UDEMY---",
			"Hurt, Explosions, Coin Get",
			"-------SPECIAL THANKS-------",
			"---UDEMY---",
			"Their \"Learn to code by making a 2D Platformer (Unity)\" course really got us going!",
			"---WIIGUY'S 8BITSTEREO---",
			"For the awesome stereo remixes of classic 8-bit tunes!",
			"---STIMPY789---",
			"SquirrelBot Concept!",
			"---YOU---",
			"Thanks for playing!",
			"THE END"
		};
		creditsSize = credits.Length;
		globalTimer = 0f;
		gimmickMoved = false;
	}
	
	// Update is called once per frame
	void Update () {
		globalTimer += Time.deltaTime;
		if (globalTimer < TIMETOSCROLL) {
			moveGround ();
			moveBackground ();
		} else if (!gimmickMoved) {
			moveGimmick ();
			gimmickMoved = true;
		} else if (globalTimer > TIMETOEXIT) {
			SceneManager.LoadScene (postCreditsSceneName);
		}
		updateCredits ();
	}

	public void moveGround () {
		float newGroundX = ground.transform.position.x - GROUNDMOVESPEED * Time.deltaTime;
		if (newGroundX < GROUNDMINX)
			newGroundX += GROUNDLOOPSIZE;
		ground.transform.position = new Vector3 (newGroundX, ground.transform.position.y, 0f);
	}

	public void moveBackground() {
		float newBackX = background.transform.position.x - BACKMOVESPEED * Time.deltaTime;
		if (currentSprite == 1 && newBackX < SPRITE2TRANS) {
			newBackX += SPRITE2DIFF;
			renderer.sprite = sprite2;
			currentSprite++;
		} else if (currentSprite == 2 && newBackX < SPRITE3TRANS) {
			newBackX += SPRITE3DIFF;
			renderer.sprite = sprite3;
			currentSprite++;
		} else if (currentSprite == 3 && newBackX < SPRITE4TRANS) {
			newBackX += SPRITE4DIFF;
			renderer.sprite = sprite4;
			currentSprite++;
		}
		background.transform.position = new Vector3 (newBackX, background.transform.position.y, 0f);
	}

	public void moveGimmick () {
		myRigidbody.velocity = new Vector3 (GROUNDMOVESPEED, 0f, 0f);
	}

	public void updateCredits () {
		if (nextLine < creditsSize) {
			creditsTimer += Time.deltaTime;
			if (creditsTimer > timeToNextLine) {
				creditsTimer -= timeToNextLine;
				GameObject textBoxClone = UnityEngine.Object.Instantiate (textBox, canvas.transform);
				textBoxClone.transform.Find ("CreditsText").gameObject.GetComponent<UnityEngine.UI.Text> ().text = credits [nextLine];
				nextLine++;
				if (nextLine < creditsSize) {
					if (credits [nextLine] [0] == '-')
						timeToNextLine = TIMEBETWEENLINES + WAITFORNEWBLOCK;
					else if (nextLine != creditsSize - 1)
						timeToNextLine = TIMEBETWEENLINES;
					else
						timeToNextLine = TIMETOTHEENDTEXT;
				}
			}
		}
	}
}
