using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour {

	private TiendaEmpezar tiendaempezar;
    private SoundController soundController;

    void Start(){
        soundController = GameObject.Find("InstanceSounds").GetComponent<SoundController>();
        tiendaempezar = GetComponent<TiendaEmpezar> ();
		OpenWindow ();
	}


	void OnEnable(){
		TiendaEmpezar.menu += mainMenu;
	}

	void OnDisable(){
		TiendaEmpezar.menu -= mainMenu;
	}

	public void mainMenu() {
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("tienda").gameObject);
        StartCoroutine(GoToMainMenu(0.5f));
	}

	public void OpenWindow(){
		tiendaempezar.showShop ();
	}

    IEnumerator GoToMainMenu(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(0);
    }
	
}
