using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudElevatorController : MonoBehaviour {
    public Transform topPoint;
    public Transform bottomPoint;
    public float moveSpeed;
     public bool movingUp;

    // Use this for initialization
    void Start () { 

       
    }
	
	// Update is called once per frame
	void Update () {
        // checks to see where the platform is 
        if(transform.position==topPoint.position)
        {
            movingUp = true;
        }
        if(transform.position==bottomPoint.position)
        {
            movingUp = false; 
        }

        //if movingUp is true, move back down 
        if(movingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, bottomPoint.position, moveSpeed);

        }
        //if movingUp is false move up 
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, topPoint.position, moveSpeed);
        }
    }

}
