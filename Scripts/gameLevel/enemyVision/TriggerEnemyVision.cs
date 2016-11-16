using UnityEngine;
using System.Collections;

public class TriggerEnemyVision : MonoBehaviour {

	void OnTriggerEnter (Collider col){
		if (col.gameObject.tag == "Player") {
			if (!GameObject.FindWithTag("Player").GetComponent<PlayerModel>().getStealth()){
				this.transform.parent.gameObject.GetComponent<EnemyIA>().setChasing();
			}
		}
	}
}
