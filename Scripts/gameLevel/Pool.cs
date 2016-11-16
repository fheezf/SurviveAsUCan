using UnityEngine;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

	public GameObject[] objects;
	public int[] numbers;

	public List<GameObject>[] pool;
	// Use this for initialization
	void Start () {
	
	}
	
	public void instantiate(){
		GameObject temp;
		pool = new List<GameObject>[objects.Length];
		for (int i = 0; i<objects.Length; i++) {
			pool[i] = new List<GameObject>();
			for (int j=0; j<numbers[i];j++){
				temp = (GameObject)Instantiate(objects[i]);
				temp.transform.parent = this.transform;
				pool[i].Add(temp);
			}
		}
	}

	public GameObject activate(int id){
		for (int i = 0; i< pool[id].Count; i++) {
			if (!pool[id][i].activeSelf){
				pool[id][i].SetActive(true);
				return pool[id][i];
			}
		} 
		pool [id].Add ((GameObject)Instantiate (objects [id]));

		pool [id] [pool [id].Count - 1].transform.parent = this.transform;
		pool [id] [pool [id].Count - 1].SetActive (true);
		return pool[id][pool[id].Count - 1];

	}

	public GameObject activate(int id, Vector3 position, Quaternion rotation){
		for (int i = 0; i< pool[id].Count; i++) {
			if (!pool[id][i].activeSelf){
				pool[id][i].SetActive(true);
				pool[id][i].transform.position = position;
				pool[id][i].transform.rotation = rotation;
				return pool[id][i];
			}
		} 
		pool [id].Add ((GameObject)Instantiate (objects [id]));
		pool[id][pool[id].Count - 1].transform.position = position;
		pool[id][pool[id].Count - 1].transform.rotation = rotation;
		pool [id] [pool [id].Count - 1].transform.parent = this.transform;
		pool [id] [pool [id].Count - 1].SetActive (true);
		return pool[id][pool[id].Count - 1];
		
	}


	public void deActivate(GameObject deActivateObject){
		deActivateObject.SetActive (false);
	}
}
