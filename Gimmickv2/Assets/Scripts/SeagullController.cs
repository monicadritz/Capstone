using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullController : MonoBehaviour {

    public bool canMove;
    

    public float moveSpeed;
    private Rigidbody2D myRidgidBody;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startLocalScale;
 

    // Use this for initialization
    void Start()
    {

        myRidgidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the enemy is on the screen canMove will be true and this will start moving the enemy
        if (canMove)
        {
            myRidgidBody.velocity = new Vector3(-moveSpeed, myRidgidBody.velocity.y, 0f);

        }
        

    }
    //when the Enemy comes  onto the screen it will start moving
    void OnBecameVisible()
    {
        canMove = true;
     
    }
    //If the Enemy falls off of the platform and hits the killplane it will deactivate 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillPlane")
        {
            gameObject.SetActive(false);

            // Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        canMove = false;

    }
}
