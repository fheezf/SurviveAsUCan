using UnityEngine;
using System.Collections;

public class PanelDialogCorrect : MonoBehaviour {

	void OnEnable () {
		transform.SetAsLastSibling ();
	}
}
