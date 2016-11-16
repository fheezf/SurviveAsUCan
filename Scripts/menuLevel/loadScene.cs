using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {

	
	void Update () {
		if(Application.GetStreamProgressForLevel(1) ==1){
            SceneManager.LoadScene(1);
		}
	}
}
