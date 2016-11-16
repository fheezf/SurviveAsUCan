using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float speed;
	private float rotationSpeed;
	public float distanceSmooth = 1.5f;
	private Vector3 pos;
	private Quaternion qTo;
	private float distance;
	private static Animator anim;
	private static bool move;
	

	void OnEnable(){
		PowerUpController.pjstealth += setStealth;
	}
	
	void OnDisable(){
		PowerUpController.pjstealth -= setStealth;
	}
	
	public void getStarted(){
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero; // don't know if it fixes
		speed = GetComponent<PlayerModel> ().getMovementSpeed ();
		rotationSpeed = GetComponent<PlayerModel> ().getRotationSpeed ();
		pos = transform.position;
		qTo = transform.rotation;
		anim = transform.GetChild(0).gameObject.GetComponent<Animator> ();
		move = true;
	}

	void FixedUpdate() {
		if (move) {
			speed = GetComponent<PlayerModel> ().getMovementSpeed ();
			rotationSpeed = GetComponent<PlayerModel> ().getRotationSpeed ();

			if (Input.GetMouseButtonDown (0) || Input.GetMouseButton (0)) {
				pos = Input.mousePosition;
				pos.z = Camera.main.transform.position.y;
				pos = Camera.main.ScreenToWorldPoint (pos);
				pos.y = transform.position.y;
				

			} 
			Vector3 dir = pos - transform.position;
		
			
		
			//Calculate distance between mouse and object and smoothing
			distance = Vector3.Distance (transform.position, pos);
			if (distance > distanceSmooth) {
				setAnim ("Movement", true);
				transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
				qTo = Quaternion.FromToRotation(transform.forward, dir) * transform.rotation;
				transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, Time.deltaTime * rotationSpeed);
			} else {
				setAnim ("Movement", false);
				//transform.position = Vector3.Lerp(transform.position, pos, Mathf.SmoothStep(0.0f, speed, Time.deltaTime * speed));
			}
		}
	}

	public static void setAnim(string cadena, bool booleano){
		anim.SetBool (cadena, booleano);
	}

	public void characterDied(){
		this.gameObject.GetComponent<BoxCollider>().enabled = false;
		move = false;
		setAnim("Death", true);
	}

	private void setStealth(bool stea){
		setAnim ("Stealth", stea);
	}

	public void setBlink(float time){
		StartCoroutine (Blink (time));
	}

	IEnumerator Blink(float time){
		this.transform.position = new Vector3 (5.3535f, 0.19f, -2.2203f);
		var endTime=Time.time + time;
		while(Time.time<endTime){
			this.transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false;
			yield return new WaitForSeconds(0.2f);
			this.transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = true;
			yield return new WaitForSeconds(0.2f);
		}

	}
}