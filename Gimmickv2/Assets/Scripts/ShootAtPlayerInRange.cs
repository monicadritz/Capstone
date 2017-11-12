using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayerInRange : MonoBehaviour {

    public float playerRange;
    public GameObject enemyItem;
    public GimmickController Gimmick;
    public Transform launchPoint;
    public float waitBetweenShots;
    private float ShotCounter;
   // private Animator myAnim;
    private Rigidbody2D myRigidBody;
    



    // Use this for initialization
    void Start () {
        Gimmick = FindObjectOfType<GimmickController>();
        myRigidBody = GetComponent<Rigidbody2D>();
        ShotCounter = waitBetweenShots;
       // myAnim = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        // shows the range of when the item will be thrown
        Debug.DrawLine(new Vector3(transform.position.x - playerRange, transform.position.y, transform.position.z), new Vector3(transform.position.x + playerRange, transform.position.y, transform.position.z));
        ShotCounter -= Time.deltaTime;
       
        //check if the player is on the right side and trigger the throwing item
       if (transform.localScale.x < 0 && Gimmick.transform.position.x > transform.position.x && Gimmick.transform.position.x < transform.position.x + playerRange && ShotCounter<0)
        {
           // myAnim.SetFloat("Speed", 0);
         //   myAnim.SetBool("Player in Range", true);
            Instantiate(enemyItem, launchPoint.position, launchPoint.rotation);
            ShotCounter = waitBetweenShots;
        }
       //else
       // {
       //     //myAnim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
       //    // myAnim.SetBool("Player in Range", false);
       // }

       // checks if the player is on the left side of the enemy
        if (transform.localScale.x > 0 && Gimmick.transform.position.x < transform.position.x && Gimmick.transform.position.x > transform.position.x - playerRange && ShotCounter < 0)
        {
         
           // myAnim.SetBool("Player in Range", true);
            Instantiate(enemyItem, launchPoint.position, launchPoint.rotation);
            ShotCounter = waitBetweenShots;
        }
        //else
        //{
          
        //    myAnim.SetBool("Player in Range", false);
        //}

    }
}
