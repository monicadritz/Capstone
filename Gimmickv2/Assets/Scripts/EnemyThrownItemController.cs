using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrownItemController : MonoBehaviour {
    public float speed;
    public GimmickController Gimmick;
    public GameObject impactEffect;
    public float rotationSpeed;
    public int damageToGive;
    private Rigidbody2D rigidbody2D;
    private LevelManager theLevelManager;

    // Use this for initialization
    void Start () {
        Gimmick = FindObjectOfType<GimmickController>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        theLevelManager = FindObjectOfType<LevelManager>();

        if (Gimmick.transform.position.x < transform.position.x)
        {
            speed = -speed;
            rotationSpeed = -rotationSpeed;
        }
    }
	
	// Update is called once per frame
	void Update () {
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        rigidbody2D.angularVelocity = rotationSpeed;

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Gimmick")
        {
            theLevelManager.HurtPlayer(damageToGive);
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

       
    }
}
