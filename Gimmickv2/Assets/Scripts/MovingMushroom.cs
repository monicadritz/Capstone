using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMushroom : MonoBehaviour {

    public bool canMove;
    private Rigidbody2D myRidgidBody;
    public float moveSpeed;

    // Use this for initialization
    void Start () {
        myRidgidBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

       
        if (canMove)
        {
            myRidgidBody.velocity = new Vector3(-moveSpeed, myRidgidBody.velocity.y, 0f);

        }
    }
    void OnBecameVisible()
    {
        canMove = true;
    }
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
