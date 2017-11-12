using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Add this to your enemy and add number to the damageToTake feild*/
public class DamageBounceBack : MonoBehaviour {
	
	public bool bounceUp;               // true if the object will generally bounce up on collision
	public bool bounceLeft;             // true if the object generally bounce left on collision
	public bool bounceRight;            // true if the object will generally bounce right on collision
	public bool bounceDown;             // true if the object will generally bounce down on collision

	public float bounceForce;           // the strength by which the object will bounce 
	private Vector3 bouncePosition;     // the area towards where the object will bounce to
	public float bounceSpeed;           // the speed at which the object will bounce

	public float noBounceTime;          // time to wait between bounce backs

	private GimmickController gimmick;  // used to control the object being bounced back
    private LevelManager theLevelManager;// gives access to the levelmangager 
    public int damageToTake;// damage the enemy will do to Gimmick

	// Use this for initialization
	void Start () {
		gimmick = FindObjectOfType<GimmickController> ();
        theLevelManager = FindObjectOfType<LevelManager>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Gimmick") {
			if (!gimmick.cantBounceBack) {
				gimmick.cantBounceBack = true;
                theLevelManager.HurtPlayer(damageToTake);
				StartCoroutine ("BounceBackCo");
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Gimmick") {
			if (!gimmick.cantBounceBack) {
				gimmick.cantBounceBack = true;

				StartCoroutine ("BounceBackCo");
			}
		}
	}

	private IEnumerator BounceBackCo(){
		bouncePosition = new Vector3 (gimmick.transform.position.x, gimmick.transform.position.y, 0f);

		// adjust the position to where gimmick should bounce towards
		if (bounceUp) {
			bouncePosition.y += bounceForce;
		} 
		if (bounceLeft) {
			bouncePosition.x -= bounceForce;
		}
		if (bounceRight) {
			bouncePosition.x += bounceForce;
		}
		if (bounceDown) {
			bouncePosition.y -= bounceForce;
		}

		// move towards position where gimmick should bounce
		gimmick.transform.position = Vector3.MoveTowards (gimmick.transform.position, bouncePosition, bounceSpeed);

		// elongate period when gimmick cannot bounce back
		yield return new WaitForSeconds (noBounceTime);

		// end period when gimmick cannot bounce back
		gimmick.cantBounceBack = false;

	}
}
