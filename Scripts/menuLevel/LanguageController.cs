using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class LanguageController : MonoBehaviour {

	public GameObject [] UIElements;
	private Lang lang;

	public void changeButtonsLanguage(){
        lang = new Lang((TextAsset)Resources.Load("Language/lang"), PlayerPrefs.GetString("language"));
        for (int i = 0; i< UIElements.Length; i++) {
			UIElements[i].transform.GetChild(0).GetComponent<Text>().text = lang.getString(UIElements[i].name);
		}
	}
}
