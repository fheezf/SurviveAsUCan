using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ViewController : MonoBehaviour {
	public Image heart;
	private GameObject[] hearts;

	public GameObject buttonNewGame;
	public GameObject buttonMainMenu;

	public Text GamePunctuation;
	public Text GameRecord;

	public Text EndPunctuation;
	public Text EndRecord;

	public Canvas GameCanvas;
	public Canvas EndCanvas;

	private Lang lang;
	void Start () {
		EndCanvas.enabled = false;
		drawHearts ();
        lang = new Lang((TextAsset)Resources.Load("Language/lang"), PlayerPrefs.GetString("language"));
    }

	public void startGame(){
		GamePunctuation.text = lang.getString("Punctuation") + PunctuationModel.getPuntos ();
		GameRecord.text = lang.getString("Record") + PunctuationModel.getRecord ();
		buttonNewGame.transform.GetChild(0).GetComponent<Text>().text = lang.getString("new_game");
		buttonMainMenu.transform.GetChild(0).GetComponent<Text>().text = lang.getString("Main Menu");
	}

	public void drawHearts(){
		hearts = GameObject.FindGameObjectsWithTag("Heart");
		foreach (GameObject ht in hearts) {
			Destroy(ht.gameObject);
		}

		for (int i = 0; i< GameObject.FindWithTag("Player").GetComponent<PlayerModel>().getLife(); i++) {
			Image ht = Instantiate(heart);
			ht.transform.SetParent(GameCanvas.transform);
			if (i < 6){
				ht.rectTransform.anchoredPosition = new Vector2 (ht.rectTransform.rect.width * (i + 1), - ht.rectTransform.rect.height);
			}else{
				ht.rectTransform.anchoredPosition = new Vector2 (ht.rectTransform.rect.width * (i - 5), - 2 * ht.rectTransform.rect.height);
			}
		}
	
	}

	public void actualizePunctuation(){
		GamePunctuation.text = lang.getString("Punctuation") + PunctuationModel.getPuntos ();
	}

	public void actualzeRecord(){
		GameRecord.text = lang.getString("Record") + PunctuationModel.getRecord ();
	}

	public void EndGame () {
		EndPunctuation.text = lang.getString("Your Punctuation") + PunctuationModel.getPuntos ();
		EndRecord.text = lang.getString("Record") + PunctuationModel.getRecord ();
		EndCanvas.enabled = true;
		GameCanvas.enabled = false;
	}


}
