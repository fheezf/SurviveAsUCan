using UnityEngine;
using System.Collections;

public class ControllerObjetive : MonoBehaviour {
	public Camera mainCamera;
	private float depth;
	private Vector3 upperLeftScreen;
	private Vector3 lowerRightScreen;
	private bool again;
	private bool doitagain;


	void Start () {
		again = true;
		doitagain = false;
		depth = 0.82f - mainCamera.transform.position.y;
		upperLeftScreen = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width*0.1f, Screen.height*0.1f, depth ));
		lowerRightScreen = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width*0.9f, Screen.height*0.9f, depth));
		Generate ();
	}

	
	public Vector3 Generate() {
		again = true;
		//GameObject go = Instantiate(objetivo);
		Vector3 go = new Vector3(0,0,0);
		GameObject[] obs = GameObject.FindGameObjectsWithTag ("obstacle");
		while (again) {
			go = new Vector3 (Random.Range (upperLeftScreen.x, lowerRightScreen.x), 0.82f, Random.Range (upperLeftScreen.z, lowerRightScreen.z));
			if (obs.Length > 0){
				foreach (GameObject obss in obs) {
					doitagain = false;
					if ((Mathf.Abs(go.x - obss.transform.position.x) < 2.5f) ||
					    (Mathf.Abs(go.z - obss.transform.position.z) < 2.5f)){
						again = true;
						doitagain = true;
						break;
					}
				}
			}
			if (!doitagain){
				again = false;
			}
		}
		return go;
	}
}
