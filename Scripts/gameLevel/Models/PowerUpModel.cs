using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PowerUpModel : MonoBehaviour
{
	public delegate void PowerUpTaken();

	public static event PowerUpTaken poweruptaken;
	
	private void OnTriggerEnter(Collider hit)
	{
		if (hit.gameObject.tag == "Player")
		{
			if (poweruptaken != null){
				poweruptaken();
			}
			gameObject.SetActive(false);
		}
	}
}
