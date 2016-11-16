using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {
	public Camera mainCamera;
	private GameObject pj;
	private static float depth;
	private static Vector3 upperLeftScreen;
	private static Vector3 lowerRightScreen;
	private bool again;
	
	void Start () {
		pj = GameObject.FindWithTag("Player");
		depth = 0.82f - mainCamera.transform.position.y;
		upperLeftScreen = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width*0.1f, Screen.height*0.1f, depth ));
		lowerRightScreen = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width*0.9f, Screen.height*0.9f, depth));
		again = true;
	}

	public void generate(GameObject obs){
		again = true;
		while (again) { //Generamos la posicion del obstaculo de manera iterativa para que no se genere cerca del jugador.
			obs.transform.position = new Vector3(Random.Range(upperLeftScreen.x,lowerRightScreen.x),0.50f,Random.Range(upperLeftScreen.z,lowerRightScreen.z));
			if (GameObject.Find("PowerUp") != null){
				if ((Mathf.Abs(obs.transform.position.x - pj.transform.position.x) < 4.5f) ||
			  	  (Mathf.Abs(obs.transform.position.z - pj.transform.position.z) < 4.5f) ||
			  	  (Mathf.Abs(obs.transform.position.x - GameObject.FindWithTag("Objetive").transform.position.x) < 2.5f) ||
			  	  (Mathf.Abs(obs.transform.position.z - GameObject.FindWithTag("Objetive").transform.position.z) < 2.5f) ||
			  	  (Mathf.Abs(obs.transform.position.x - GameObject.Find("PowerUp").transform.position.x) < 2.5f) ||
			  	  (Mathf.Abs(obs.transform.position.z - GameObject.Find("PowerUp").transform.position.z) < 2.5f)){
					again = true;
				}else{
					float rnd1 = Random.Range (1f,10f);
					float rnd2 = Random.Range (1f, 10f-rnd1);
					float rndRot = Random.Range(0f, 360f);
					obs.transform.localScale = new Vector3 (rnd1, obs.transform.localScale.y, rnd2);
					obs.transform.localEulerAngles = new Vector3 (obs.transform.localRotation.x, rndRot, obs.transform.localRotation.z);
					again = false;
				}
			}else{
				if ((Mathf.Abs(obs.transform.position.x - pj.transform.position.x) < 4.5f) ||
				    (Mathf.Abs(obs.transform.position.z - pj.transform.position.z) < 4.5f) ||
				    (Mathf.Abs(obs.transform.position.x - GameObject.FindWithTag("Objetive").transform.position.x) < 2.5f) ||
				    (Mathf.Abs(obs.transform.position.z - GameObject.FindWithTag("Objetive").transform.position.z) < 2.5f)){
					again = true;
				}else{
					float rnd1 = Random.Range (1f,10f);
					float rnd2 = Random.Range (1f, 10f-rnd1);
					float rndRot = Random.Range(0f, 360f);
					obs.transform.localScale = new Vector3 (rnd1, obs.transform.localScale.y, rnd2);
					obs.transform.localEulerAngles = new Vector3 (obs.transform.localRotation.x, rndRot, obs.transform.localRotation.z);
					again = false;
				}
			}
		}

	}
}
