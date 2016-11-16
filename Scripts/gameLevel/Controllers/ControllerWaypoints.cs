using UnityEngine;
using System.Collections;

public class ControllerWaypoints : MonoBehaviour {

	public Camera mainCamera;
	private float depth;
	private Vector3 pos;
	private float [] aux = new float[]{0.1f, 0.275f, 0.5f, 0.725f, 0.9f};
	public GameObject [] Waypoints;
	
	void Start () {
		depth = 0.82f - mainCamera.transform.position.y;

		for (int i = 0; i< Waypoints.Length / 5; i++) {
			for (int j = 0; j<Waypoints.Length / 5; j++) {
				pos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width*aux[4 - i], Screen.height*aux[j], depth ));
				Waypoints[i*5 + j].transform.position = new Vector3(pos.x, 0.82f, pos.z);
			}
		}
	}
	

}
