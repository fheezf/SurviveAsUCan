using System.Collections;
using UnityEngine;

public class ObstacleModel : MonoBehaviour{
	public delegate void Catched();
	
	public static event Catched catched;

	void Awake (){
		StartCoroutine (autoDestroy(10f));
	}

	public void setAutoDestroy (){
		StartCoroutine (autoDestroy(10f));
	}

	IEnumerator autoDestroy(float waitTime){
		yield return new WaitForSeconds (waitTime);
		gameObject.SetActive (false);
	}

	private void OnTriggerEnter(Collider hit)
	{
		if (hit.gameObject.GetComponent<PlayerModel>().getStealth()){
			return;
		}
		if (catched != null)
		{
			catched();
		}
	}
}
