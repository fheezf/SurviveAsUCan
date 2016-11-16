using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour {

	public GameObject mainMenuCanvas;
	public GameObject optionsCanvas;

    private SoundController soundController;
	
	void Start(){

        soundController = GameObject.Find("InstanceSounds").GetComponent<SoundController>();

        if (!PlayerPrefs.HasKey ("record")) {
			PlayerPrefs.SetInt ("record", 0);
		}
		if (!PlayerPrefs.HasKey ("skin")) {
			PlayerPrefs.SetString("skin","base_skin");
		}
		if (!PlayerPrefs.HasKey ("coins")) {
			PlayerPrefs.SetInt("coins",0);
		}
		if (!PlayerPrefs.HasKey ("language")) {
			PlayerPrefs.SetString("language", "Spanish");
		}
		GameObject.Find ("Language").GetComponent<LanguageController> ().changeButtonsLanguage ();

        GameObject.Find("InstanceSounds").GetComponent<SoundController>().PlayMusic((AudioClip)Resources.Load("Music/menu"), true);
		
		//PlayerPrefs.DeleteAll ();

	}

	void OnEnable(){
		Options.backButton += Option;
	}

	void OnDisable(){
		Options.backButton -= Option;
	}

	public void newGame(){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("Menu").gameObject);
        StartCoroutine (changeScene(0.5f, 3));
	}

	public void Option(bool boolean){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("Menu").gameObject);
        if (boolean) {
			mainMenuCanvas.SetActive (false);
			optionsCanvas.SetActive (true);
		} else {
			mainMenuCanvas.SetActive (true);
			optionsCanvas.SetActive (false);
		}
	}

    IEnumerator changeScene(float waitTime, int index) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(index);
    }

	public void shop(){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("Menu").gameObject);
        StartCoroutine(changeScene(0.5f, 2));
    }

	public void quit(){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("Menu").gameObject);
        Application.Quit ();
	}
	
}
