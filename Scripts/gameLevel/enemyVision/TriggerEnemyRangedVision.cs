using UnityEngine;
using System.Collections;

public class TriggerEnemyRangedVision : MonoBehaviour {
	private float time = 0f;
	public float maxTime;

	void OnTriggerEnter (Collider col){
		if (col.gameObject.tag == "Player") {
			if (!GameObject.FindWithTag("Player").GetComponent<PlayerModel>().getStealth()){
				time = 0f;
			}
		}
	}

	void OnTriggerStay (Collider col){
		if (col.gameObject.tag == "Player") {
			if (!GameObject.FindWithTag("Player").GetComponent<PlayerModel>().getStealth() && this.GetComponentInParent<EnemyRangedIA>().getState() == "Looking"){
				time += Time.deltaTime;
				if (time > maxTime){
					this.transform.parent.gameObject.GetComponent<EnemyRangedIA>().Shoot();
					time = 0f;
				}
			}else{
				time = 0f;
			}
		}
	}
}
