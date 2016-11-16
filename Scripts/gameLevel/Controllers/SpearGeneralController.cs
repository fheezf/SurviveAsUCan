using UnityEngine;
using System.Collections;

public class SpearGeneralController : MonoBehaviour {

	public GameObject spear;

	public void generate(Transform tnsf){
		spear.transform.position = new Vector3(tnsf.position.x, 4.60f, tnsf.position.z);
		Instantiate(spear);
	}
}
