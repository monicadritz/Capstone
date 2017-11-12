using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingFishController : MonoBehaviour {

    public bool canMove;
    public bool goingUp;
    public Transform topPoint;
    public Transform bottomPoint;

    public float moveSpeed;
    private Rigidbody2D myRidgidBody;

    // Use this for initialization
    void Start()
    {
        myRidgidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == topPoint.position)
        {
          goingUp = true;
        }
        if (transform.position == bottomPoint.position)
        {
            goingUp = false;
        }
        if (goingUp && canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, bottomPoint.position, moveSpeed);

        }
        //if movingUp is false move up 
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, topPoint.position, moveSpeed);
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