using UnityEngine;
using System.Collections;

public class SpearController : MonoBehaviour {

	private Rigidbody rb;
	private GameObject pj;

	public float waitTime;
	public float strength;

	void Awake (){
		rb = GetComponent<Rigidbody> ();
		pj = GameObject.FindWithTag("Player");
		transform.LookAt(new Vector3(pj.transform.position.x, this.transform.position.y, pj.transform.position.z));
		StartCoroutine (moveSpear(waitTime));
	}
	

	IEnumerator moveSpear(float waitTime){
		Vector3 v3Force = strength*transform.forward;
		rb.AddForce(v3Force, ForceMode.Impulse);

		
		yield return new WaitForSeconds (waitTime);
		Destroy (this.gameObject);
	}
}
