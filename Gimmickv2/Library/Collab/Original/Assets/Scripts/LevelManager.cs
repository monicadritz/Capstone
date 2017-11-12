using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public int coinCount = 0;
    public GimmickController theGimmick//gives access to gimmick
    public float waitToRespawn;// time to wait for respawn
    public GameObject deathsplosion;//particle effect
    private bool respawning;
    public ResetOnRespawn[] objectsToReset;
    public bool isAlive;
    public int maxHealth;

    public int healthCount;

    // Use this for initialization
    void Start () {
        theGimmick = FindObjectOfType<GimmickController>();
        healthCount = maxHealth;
        objectsToReset = FindObjectsOfType<ResetOnRespawn>();
        isAlive = true;
        if (PlayerPrefs.HasKey("CoinCount"))
        {
            coinCount = PlayerPrefs.GetInt("CoinCount");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (healthCount <= 0 && !respawning)
        {
            isAlive = false;
            Respawn();
            respawning = true;
        }



    }

	public void addCoins(int coinsToAdd){
		coinCount += coinsToAdd;
	}
    public void Respawn()
    {
        isAlive = true;
        if(!isAlive)
        {
            StartCoroutine("RespawnCo");
        }
        else
        { 
            theGimmick.gameObject.SetActive(false);
           // gameOverScreen.SetActive(true);
            //levelMusic.Stop();
            //gameOverMusic.Play();
           // levelMusic.volume = levelMusic.volume / 2f;

        }
    }
    public IEnumerator RespawnCo()
    {

        theGimmick.gameObject.SetActive(false);
        Instantiate(deathsplosion, theGimmick.transform.position, theGimmick.transform.rotation);

        yield return new WaitForSeconds(waitToRespawn);

        healthCount = maxHealth;
        respawning = false;
       // UpdateHeartMeter();
        coinCount = 0;
        //coinBonusLifeCount = 0;
        //coinText.text = "Coins: " + coinCount;
        theGimmick.transform.position = theGimmick.respawnPosition;

        theGimmick.gameObject.SetActive(true);

        for (int i = 0; i < objectsToReset.Length; i++)
        {

            objectsToReset[i].gameObject.SetActive(true);
            objectsToReset[i].ResetObject();
        }
    }


}
