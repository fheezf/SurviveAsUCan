using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;
using System;
using System.IO;
using Soomla;


public class TiendaEmpezar : MonoBehaviour {

    private SoundController soundController;


    public GameObject prefabButton;
	public GameObject mainMenuButton;
	public GameObject panelGameCoins;
	public RectTransform parentButton;


	public static List<VirtualGood> VirtualGoods = null;
	private Boolean isPair;
	private float backHeight;
	private GameObject goPanel;

	private modalPanelController modalPanel;

	private Lang lang;

	public delegate void mainMenu();
	
	public static event mainMenu menu;

	private int aux;
	// Use this for initialization
	void Start(){
        lang = new Lang((TextAsset)Resources.Load("Language/lang"), PlayerPrefs.GetString("language"));
        soundController = GameObject.Find("InstanceSounds").GetComponent<SoundController>();
    }

	void OnEnable (){
		modalPanelController.confirmyesButtoncp += confirmYesAction;
		modalPanelController.confirmyesButtonvg += confirmYesAction;
	}

	void OnDisable (){
		modalPanelController.confirmyesButtoncp -= confirmYesAction;
		modalPanelController.confirmyesButtonvg -= confirmYesAction;
	}

	void Awake(){
		modalPanel = modalPanelController.Instance ();
	}

	void throwEvent (){
		if (menu != null) {
			menu();
		}
	}

	IEnumerator actualizeCoins (float waitTime){
		yield return new WaitForSeconds(waitTime);
		goPanel.transform.GetChild (0).GetComponent<Text> ().text = StoreInventory.GetItemBalance ("game_coin").ToString ();
	}

	IEnumerator actualizeButtons (float waitTime, VirtualGood vg){
		yield return new WaitForSeconds(waitTime);
		int a = 0;
		foreach (Transform child in GameObject.Find("ScrollPanel").transform) {
			if (a > 1){
				if (child.transform.GetChild(0).GetComponent<Image>().sprite.ToString() == vg.ID.ToLower() + " (UnityEngine.Sprite)"){
					child.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
					child.GetComponent<Button>().interactable = false;
				}
			}
			a++;
		}
	}
	

	void buyEvent (VirtualCurrencyPack cp){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("tienda").gameObject);
        modalPanel.ChoiceConfirm (lang.getString("Choice Confirm") + cp.Name + " ?", lang.getString("Choice Confirm Title"), cp );
	}

	void buyEvent (VirtualGood vg){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("tienda").gameObject);
        modalPanel.ChoiceConfirm (lang.getString("Choice Confirm") + vg.Name + " ?", lang.getString("Choice Confirm Title"), vg );
	}

	void confirmYesAction (VirtualGood vg){
		Debug.Log("SOOMLA/UNITY Wants to buy: " + vg.Name);
		try {
			StoreInventory.BuyItem(vg.ItemId);
			StartCoroutine (actualizeCoins (0.1f));
			StartCoroutine (actualizeButtons (0.1f, vg));
            if (vg.PurchaseType.GetType().ToString() != "Soomla.Store.PurchaseWithMarket")
            {
                modalPanel.ChoiceCorrect(lang.getString("Choice Correct"), lang.getString("Choice Correct Title"));
            }
		} catch (Exception e) {
			Debug.Log ("SOOMLA/UNITY " + e.Message);
			if (e.GetType().ToString() == "Soomla.Store.InsufficientFundsException"){
                soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonError"), GameObject.Find("tienda").gameObject);
                modalPanel.ChoiceCorrect (lang.getString("Choice Incorrect1"), lang.getString("Choice Incorrect Title"));
			}else{
                soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonError"), GameObject.Find("tienda").gameObject);
                modalPanel.ChoiceCorrect (lang.getString("Choice Incorrect2"), lang.getString("Choice Incorrect Title"));
			}
		}


	}

	void confirmYesAction(VirtualCurrencyPack cp){
		Debug.Log("SOOMLA/UNITY Wants to buy: " + cp.Name);
		PurchasableVirtualItem pvi = StoreInfo.GetPurchasableItemWithProductId(cp.ItemId);
		MarketItem mi = ((PurchaseWithMarket)pvi.PurchaseType).MarketItem;
		Debug.Log("title:   " + mi.MarketTitle + "  - Desc:   " + mi.MarketDescription + "  - Price   " + mi.Price);
		try {
			StoreInventory.BuyItem(cp.ItemId);
			StartCoroutine (actualizeCoins (0.1f));
		} catch (Exception e) {
			Debug.Log ("SOOMLA/UNITY " + e.Message);
            soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonError"), GameObject.Find("tienda").gameObject);
            modalPanel.ChoiceCorrect (lang.getString("Choice Incorrect2"), lang.getString("Choice Incorrect Title"));
		}


	}



	void AddListener (Button goButton, VirtualCurrencyPack cp){
		goButton.onClick.AddListener (() => buyEvent (cp));

	}

	void AddListener (Button goButton, VirtualGood vg){
		goButton.onClick.AddListener (() => buyEvent (vg));

	}

	public void showShop(){

        soundController.PlayMusic((AudioClip)Resources.Load("Music/shop"), true);


        goPanel = (GameObject)Instantiate (panelGameCoins);
		goPanel.transform.SetParent(parentButton, false);
		goPanel.transform.localScale = new Vector3(1, 1, 1);
		goPanel.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (250, - goPanel.GetComponent<RectTransform> ().rect.height / 2);
		goPanel.transform.GetChild (0).GetComponent<Text> ().text = StoreInventory.GetItemBalance ("game_coin").ToString ();
		aux = 0;
		GameObject goButton = (GameObject)Instantiate(mainMenuButton);
		goButton.transform.SetParent(parentButton, false);
		goButton.transform.localScale = new Vector3(1, 1, 1);
		goButton.GetComponent<RectTransform>().anchoredPosition = new Vector2 (goButton.GetComponent<RectTransform>().rect.width / 2,  - aux*goButton.GetComponent<RectTransform>().rect.height - goButton.GetComponent<RectTransform>().rect.height/2);
		backHeight = goButton.GetComponent<RectTransform> ().rect.height;
		goButton.transform.GetChild (0).GetComponent<Text>().text = lang.getString ("ShopBack");
		goButton.GetComponent<Button>().onClick.AddListener(() => throwEvent());
		isPair = true;
		foreach (VirtualCurrencyPack cp in StoreInfo.CurrencyPacks) {
			if (isPair){
				aux++;
			}
			goButton = (GameObject)Instantiate(prefabButton);
			goButton.transform.SetParent(parentButton, false);
			goButton.transform.localScale = new Vector3(1, 1, 1);
			goButton.GetComponent<RectTransform>().sizeDelta = new Vector2 (-360, goButton.GetComponent<RectTransform>().rect.height);
			goButton.GetComponent<RectTransform>().localPosition = new Vector3(- 180,goButton.GetComponent<RectTransform>().localPosition.y,goButton.GetComponent<RectTransform>().localPosition.z);
			if (isPair){
				goButton.GetComponent<RectTransform>().anchoredPosition = new Vector2 (- 180,  - (aux - 1)*goButton.GetComponent<RectTransform>().rect.height - backHeight - goButton.GetComponent<RectTransform>().rect.height/2);
			}else{
				goButton.GetComponent<RectTransform>().anchoredPosition = new Vector2 ( 180,  - (aux - 1)*goButton.GetComponent<RectTransform>().rect.height - backHeight - goButton.GetComponent<RectTransform>().rect.height/2);
			}
			goButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = lang.getString(cp.Name);
			goButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = (Mathf.Round(float.Parse(cp.PurchaseType.GetPrice()) * 100f) / 100f).ToString();
			goButton.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/"+cp.ID.Substring(cp.ID.IndexOf('g')));
            goButton.transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced;

            if (cp.PurchaseType.GetType().ToString() == "Soomla.Store.PurchaseWithMarket"){
				goButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text += " €";
			}else{
				goButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text += " Coins";
			}
			AddListener(goButton.GetComponent<Button>(), cp);
			isPair = !isPair;
		}
		foreach (VirtualGood vg in StoreInfo.Goods) { 
			if (isPair){
				aux++;
			}
			goButton = (GameObject)Instantiate(prefabButton);
			goButton.transform.SetParent(parentButton, false);
			goButton.transform.localScale = new Vector3(1, 1, 1);
			goButton.GetComponent<RectTransform>().sizeDelta = new Vector2 (- 360, goButton.GetComponent<RectTransform>().rect.height);
			goButton.GetComponent<RectTransform>().localPosition = new Vector3(- 180,goButton.GetComponent<RectTransform>().localPosition.y,goButton.GetComponent<RectTransform>().localPosition.z);
			if (isPair){
				goButton.GetComponent<RectTransform>().anchoredPosition = new Vector2 (- 180,  - (aux - 1)*goButton.GetComponent<RectTransform>().rect.height - backHeight - goButton.GetComponent<RectTransform>().rect.height/2);
			}else{
				goButton.GetComponent<RectTransform>().anchoredPosition = new Vector2 ( 180,  - (aux - 1)*goButton.GetComponent<RectTransform>().rect.height - backHeight - goButton.GetComponent<RectTransform>().rect.height/2);
			}
			goButton.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = lang.getString(vg.Name);
			goButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = (Mathf.Round(float.Parse(vg.PurchaseType.GetPrice()) * 100f) / 100f).ToString();
			if (vg.PurchaseType.GetType().ToString() == "Soomla.Store.PurchaseWithMarket"){
				goButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text += " €";
			}else{
				goButton.transform.GetChild(1).GetChild(1).GetComponent<Text>().text += " Coins";
			}
			goButton.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/"+vg.ID);
            goButton.transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced;
            if (StoreInventory.GetItemBalance(vg.ID) > 0){
                goButton.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                goButton.GetComponent<Button>().interactable = false;
			}else{
				AddListener(goButton.GetComponent<Button>(), vg);
			}

			isPair = !isPair;
		}
		parentButton.GetComponent<RectTransform> ().sizeDelta = new Vector2 (parentButton.GetComponent<RectTransform> ().rect.width, backHeight + aux * goButton.GetComponent<RectTransform> ().rect.height);
	}

}
