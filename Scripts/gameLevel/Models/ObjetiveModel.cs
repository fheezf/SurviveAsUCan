using System;
using System.Collections;
using UnityEngine;

public class ObjetiveModel : MonoBehaviour {

	public delegate void ObjetiveAdquired();
	public static event ObjetiveAdquired adquired;
	

	void OnTriggerEnter(Collider hit){
		if (hit.gameObject.tag == "Player") {
			if (adquired != null){
				adquired();
			}
			gameObject.SetActive(false);
		}
	}
}
