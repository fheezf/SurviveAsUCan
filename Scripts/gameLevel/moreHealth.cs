using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class moreHealth : MonoBehaviour {
	private RectTransform canvasRectT;
	private RectTransform mooreHealth;
	private Transform objectToFollow;

	private Camera camara;
	public float offset;

	void Awake () {
		camara = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		canvasRectT = GameObject.Find ("GameView").GetComponent<RectTransform>();
		objectToFollow = GameObject.Find("Jugador").transform;
		mooreHealth = GetComponent<RectTransform> ();
		this.transform.SetParent(GameObject.Find("GameView").transform);
		StartCoroutine (endthis (2.0f));
	}


	void Update () {
		Vector2 screenPointaux = RectTransformUtility.WorldToScreenPoint(camara, objectToFollow.position);
		Vector2 screenPoint = new Vector2 (screenPointaux.x, screenPointaux.y + offset);
		mooreHealth.anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;
	}

	IEnumerator endthis(float waitTime){
		yield return new WaitForSeconds (waitTime);
		Destroy (this.gameObject);
	}
}
