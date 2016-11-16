using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using Soomla.Store;
using Soomla;

public class Options : MonoBehaviour {

    private SoundController soundController;

    public Sprite normalRectSprite;
    public Sprite pressedRectSprite;
    public Sprite normalSquareSprite;
    public Sprite pressedSquareSprite;

    public GameObject howToPlayPanel;
	public GameObject PlayerSkinsPanel;
	public GameObject OptionsPanel;
	public GameObject PlayerSkinsButton;
	public string[] howToPlayTexts;
	public Sprite[] howToPlayImages;

	private GameObject howToPanel;
	private GameObject optionsPanel;
	private GameObject playerPanel;
	private int howToPlayActualPage;

	public delegate void BackButton(bool boolean);
	public static event BackButton backButton;
	private ColorBlock colorPressed;
	private ColorBlock colorNormal;

	private Lang lang;
	void Start () {

        soundController = GameObject.Find("InstanceSounds").GetComponent<SoundController>();
        colorPressed = ColorBlock.defaultColorBlock;
		colorNormal = ColorBlock.defaultColorBlock;
		colorPressed.normalColor = new Color(0.96f, 0.96f, 0.96f, 1);
		colorPressed.disabledColor = new Color (0.478f, 0.478f, 0.478f, 1);
		colorNormal.normalColor = new Color(0.639f, 0.603f, 0.631f, 1);
		colorNormal.disabledColor = new Color (0.478f, 0.478f, 0.478f, 1);
		instantiateHowToPlay ();
		instantiatePlayerSkins ();
		instantiateOptions ();
		enterHowToPlay ();



	}

	public void setLanguage(Dropdown target){
		if (target.value == 0) { // Español
			PlayerPrefs.SetString ("language", "Spanish");
			GameObject.Find ("Language").GetComponent<LanguageController> ().changeButtonsLanguage (); // No lo puedo hacer todo de uno porque los labels son hijos de un gameObject que instancio via runtime.
			changeLanguageLabelsOptionsMenu ();
		} else {
			PlayerPrefs.SetString ("language", "English");
			GameObject.Find ("Language").GetComponent<LanguageController> ().changeButtonsLanguage ();
			changeLanguageLabelsOptionsMenu ();
		}
		
	}

	void changeLanguageLabelsOptionsMenu (){
        
        // (GetChild(0).GetChild(0) son las etiquetas y GetChild(0).GetChild(1) son los dropdown
        optionsPanel.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Text>().text = lang.getString("OptionsLanguageLabel");
	}

	void instantiateOptions(){

        lang = new Lang((TextAsset)Resources.Load("Language/lang"), PlayerPrefs.GetString("language"));
        optionsPanel = (GameObject)Instantiate (OptionsPanel);
		optionsPanel.transform.SetParent (this.transform, false);
		optionsPanel.transform.localScale = new Vector3 (1, 1, 1);
		//Configuramos la etiqueta del language 
		changeLanguageLabelsOptionsMenu ();
		//Configuramos el dropDown del language
		optionsPanel.transform.GetChild (0).GetChild (1).GetChild (0).GetComponent<Dropdown> ().options.Clear (); // limpiamos los Option del DropDown
		for (int i = 0; i < lang.getNumberOfLanguages ((TextAsset)Resources.Load("Language/lang")); i++) { // Colocamos todos los nuevos valores
			optionsPanel.transform.GetChild (0).GetChild (1).GetChild (0).GetComponent<Dropdown> ().options.Add (new Dropdown.OptionData() {text=lang.getString("DropdownLanguageOption"+i)});
		}
		optionsPanel.transform.GetChild (0).GetChild (1).GetChild (0).GetComponent<Dropdown> ().value = 1;
		if (PlayerPrefs.GetString("language") == "Spanish"){
			optionsPanel.transform.GetChild (0).GetChild (1).GetChild (0).GetComponent<Dropdown> ().value = 0; // movemos la opcion del dropdown para que se refresque el valor del primero.
		}
		optionsPanel.transform.GetChild (0).GetChild (1).GetChild (0).GetComponent<Dropdown> ().onValueChanged.AddListener (delegate {
			setLanguage (optionsPanel.transform.GetChild (0).GetChild (1).GetChild (0).GetComponent<Dropdown> ());
		});
	}

	void instantiateHowToPlay (){
		howToPanel = (GameObject)Instantiate (howToPlayPanel);
		howToPanel.transform.SetParent (this.transform, false);
		howToPanel.transform.localScale = new Vector3(1, 1, 1);
		howToPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => howToPlayMinusPage());
		howToPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => howToPlayPlusPage());
	}

	void instantiatePlayerSkins (){
		int auxHeight = 0;
		int auxWidth = 0;
		playerPanel = (GameObject)Instantiate (PlayerSkinsPanel);
		playerPanel.transform.SetParent (this.transform, false);
		playerPanel.transform.localScale = new Vector3(1, 1, 1);
		GameObject goButton = (GameObject)Instantiate(PlayerSkinsButton);
		goButton.transform.SetParent(playerPanel.transform, false);
		goButton.transform.localScale = new Vector3(1, 1, 1);
		goButton.GetComponent<RectTransform>().sizeDelta = new Vector2 (-1024, goButton.GetComponent<RectTransform>().rect.height);
		goButton.GetComponent<RectTransform>().localPosition = new Vector3(0,goButton.GetComponent<RectTransform>().localPosition.y,goButton.GetComponent<RectTransform>().localPosition.z);
		goButton.GetComponent<RectTransform>().anchoredPosition = new Vector2 (auxWidth*goButton.GetComponent<RectTransform>().rect.width - 512,  - auxHeight*goButton.GetComponent<RectTransform>().rect.height - goButton.GetComponent<RectTransform>().rect.height/2);
		goButton.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/base_skin");
        goButton.transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced;
        if (PlayerPrefs.GetString ("skin") == "base_skin") {
			goButton.GetComponent<Image>().sprite = pressedSquareSprite;
		}
		AddListener(goButton.GetComponent<Button>(), "base_skin");
		auxWidth++;
		foreach (string vg in StoreInfo.Categories[0].GoodItemIds) {
			goButton = (GameObject)Instantiate(PlayerSkinsButton);
			goButton.transform.SetParent(playerPanel.transform, false);
			goButton.transform.localScale = new Vector3(1, 1, 1);
			goButton.GetComponent<RectTransform>().sizeDelta = new Vector2 (-1024, goButton.GetComponent<RectTransform>().rect.height);
			goButton.GetComponent<RectTransform>().localPosition = new Vector3(0,goButton.GetComponent<RectTransform>().localPosition.y,goButton.GetComponent<RectTransform>().localPosition.z);
			goButton.GetComponent<RectTransform>().anchoredPosition = new Vector2 (auxWidth*goButton.GetComponent<RectTransform>().rect.width - 512,  - auxHeight*goButton.GetComponent<RectTransform>().rect.height - goButton.GetComponent<RectTransform>().rect.height/2);
			goButton.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/"+vg);
            goButton.transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced;
            if (PlayerPrefs.GetString ("skin") == vg) {
                goButton.GetComponent<Image>().sprite = pressedSquareSprite;
            }
			if (StoreInventory.GetItemBalance(vg) == 0){
                goButton.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                goButton.GetComponent<Button>().interactable = false;
			}else{
				AddListener(goButton.GetComponent<Button>(), vg);
			}
			auxWidth++;
			if (auxWidth == 5){
				auxWidth = 0;
				auxHeight++;
			}
		}
	}

	void AddListener (Button goButton, string vg){
		goButton.onClick.AddListener (() => setSkin (vg));
		
	}

	void setSkin(string vg){
		PlayerPrefs.SetString ("skin", vg);
		foreach (Transform child in playerPanel.transform) {
			if (child.GetChild(0).GetComponent<Image>().sprite.ToString() == vg+" (UnityEngine.Sprite)"){
                soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("Menu").gameObject);
                child.GetComponent<Image>().sprite = pressedSquareSprite;
			}else{
				if (child.GetChild(0).GetComponent<Image>().sprite.ToString() == "base_skin (UnityEngine.Sprite)"){
					child.GetComponent<Image>().sprite = normalSquareSprite;
				}else{
					if (StoreInventory.GetItemBalance(child.GetChild(0).GetComponent<Image>().sprite.ToString().Substring(0, child.GetChild(0).GetComponent<Image>().sprite.ToString().Length - 21)) > 0){
						child.GetComponent<Image>().sprite = normalSquareSprite;
					}
				}
			}
		}
	}

	public void enterOptionsPanel(){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("Menu").gameObject);
        changePanel (3);
	}

	public void enterHowToPlay(){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("Menu").gameObject);
        howToPlayActualPage = 1;
		changePanel (1);
	}

	public void enterPlayerSkins(){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("Menu").gameObject);
        changePanel (2);
	}
	
	void OnEnable(){
		TouchUpdatePanel.minusPage += howToPlayMinusPage;
		TouchUpdatePanel.plusPage += howToPlayPlusPage;
	}

	void OnDisable(){
		TouchUpdatePanel.minusPage -= howToPlayMinusPage;
		TouchUpdatePanel.plusPage -= howToPlayPlusPage;
	}

	public void backToMainMenu(){
		if (backButton != null) {
            backButton(false);
		}
	}

	public void changePanel(int index){
		switch (index) {
			case 1:
                transform.GetChild(1).transform.GetChild(1).GetComponent<Image>().sprite = pressedRectSprite;
                transform.GetChild(1).transform.GetChild(2).GetComponent<Image>().sprite = normalRectSprite;
                transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = normalRectSprite;

                optionsPanel.SetActive(false);
				playerPanel.SetActive(false);
				howToPanel.SetActive(true);
				howToPanel.GetComponent<TouchUpdatePanel>().startChecks();
				showHowToPlay();
				break;
			case 2:
                transform.GetChild(1).transform.GetChild(2).GetComponent<Image>().sprite = pressedRectSprite;
                transform.GetChild(1).transform.GetChild(1).GetComponent<Image>().sprite = normalRectSprite;
                transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = normalRectSprite;
                optionsPanel.SetActive(false);
				howToPanel.SetActive(false);
				playerPanel.SetActive(true);
				break;
			case 3:
                transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = pressedRectSprite;
                transform.GetChild(1).transform.GetChild(2).GetComponent<Image>().sprite = normalRectSprite;
                transform.GetChild(1).transform.GetChild(1).GetComponent<Image>().sprite = normalRectSprite;
                howToPanel.SetActive(false);
				playerPanel.SetActive(false);
				optionsPanel.SetActive(true);
				break;
			default:
				break;
		}
	}


	public void showHowToPlay (){
        lang = new Lang ((TextAsset)Resources.Load("Language/lang"), PlayerPrefs.GetString("language"));

		howToPanel.transform.GetChild(0).GetComponent<Text>().text = lang.getString(howToPlayTexts[howToPlayActualPage - 1]);
		howToPanel.transform.GetChild (1).GetComponent<Image> ().sprite = howToPlayImages [howToPlayActualPage - 1];
		howToPanel.transform.GetChild(2).GetComponent<Text>().text = howToPlayActualPage.ToString() + " / " + howToPlayTexts.Length.ToString();
		if (howToPlayActualPage == 1) {
			howToPanel.transform.GetChild (3).GetComponent<Button> ().interactable = false;
		} else {
			howToPanel.transform.GetChild (3).GetComponent<Button> ().interactable = true;
		}
		if (howToPlayActualPage == howToPlayTexts.Length) {
			howToPanel.transform.GetChild (4).GetComponent<Button> ().interactable = false;
		} else {
			howToPanel.transform.GetChild (4).GetComponent<Button> ().interactable = true;
		}
	}

	public void howToPlayMinusPage(){
		if (howToPlayActualPage > 1) {
            soundController.PlaySFX((AudioClip)Resources.Load("Sounds/howToPlayPages"), GameObject.Find("Menu").gameObject);
            howToPlayActualPage--;
		}
		showHowToPlay ();
	}

	public void howToPlayPlusPage(){
		if (howToPlayActualPage < howToPlayTexts.Length) {
            soundController.PlaySFX((AudioClip)Resources.Load("Sounds/howToPlayPages"), GameObject.Find("Menu").gameObject);
            howToPlayActualPage++;
		}
		showHowToPlay ();
	}
}
