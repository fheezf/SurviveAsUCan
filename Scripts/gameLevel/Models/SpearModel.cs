using UnityEngine;
using System.Collections;

public class SpearModel : MonoBehaviour {
	public delegate void Catched();
	
	public static event Catched catched;

	void OnTriggerEnter (Collider col){
		if (col.gameObject.tag == "Player") {
			if (col.gameObject.GetComponent<PlayerModel>().getStealth()){
				return;
			}
			if (catched != null)
			{
				catched();
			}
		}

	}
}
