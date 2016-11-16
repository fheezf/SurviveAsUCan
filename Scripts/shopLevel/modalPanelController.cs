using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using Soomla.Store;
using System;
using Soomla;

public class modalPanelController : MonoBehaviour {

	public Text questionPanelConfirm;
    public Text headerPanelConfirm;
	public Button yesButtonPanelConfirm;
	public Button noButtonPanelConfirm;
	public GameObject ModalPanelConfirmObject;

	public Text questionPanelCorrect;
    public Text headerPanelCorrect;
	public Button yesButtonPanelCorrect;
	public GameObject ModalPanelCorrectObject;

	public delegate void ConfirmYesButtonCP(VirtualCurrencyPack cp);
	public delegate void ConfirmYesButtonVG(VirtualGood vg);

	public static event ConfirmYesButtonCP confirmyesButtoncp;
	public static event ConfirmYesButtonVG confirmyesButtonvg;

	private static modalPanelController modalPanel;
	
	public static modalPanelController Instance () {
		if (!modalPanel) {
			modalPanel = FindObjectOfType(typeof (modalPanelController)) as modalPanelController;
			if (!modalPanel)
				Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
		}
		
		return modalPanel;
	}

	public void ChoiceCorrect (string question, string title){
		ModalPanelCorrectObject.SetActive (true);

		yesButtonPanelCorrect.onClick.RemoveAllListeners();
		yesButtonPanelCorrect.onClick.AddListener (ClosePanelCorrect);

        headerPanelCorrect.text = title;
		questionPanelCorrect.text = question;

		yesButtonPanelCorrect.gameObject.SetActive (true);
	}

	public void ChoiceConfirm (string question, string title, VirtualCurrencyPack cp) {
		ModalPanelConfirmObject.SetActive (true);
		
		yesButtonPanelConfirm.onClick.RemoveAllListeners();
		yesButtonPanelConfirm.onClick.AddListener(() => yesEvent(cp));
		yesButtonPanelConfirm.onClick.AddListener (ClosePanelConfirm);
		
		noButtonPanelConfirm.onClick.RemoveAllListeners();
		noButtonPanelConfirm.onClick.AddListener (ClosePanelConfirm);

        headerPanelConfirm.text = title;
		questionPanelConfirm.text = question;

		yesButtonPanelConfirm.gameObject.SetActive (true);
		noButtonPanelConfirm.gameObject.SetActive (true);
	}

	public void ChoiceConfirm (string question, string title, VirtualGood vg) {
		ModalPanelConfirmObject.SetActive (true);
		
		yesButtonPanelConfirm.onClick.RemoveAllListeners();
		yesButtonPanelConfirm.onClick.AddListener(() => yesEvent(vg));
		yesButtonPanelConfirm.onClick.AddListener (ClosePanelConfirm);
		
		noButtonPanelConfirm.onClick.RemoveAllListeners();
		noButtonPanelConfirm.onClick.AddListener (ClosePanelConfirm);

        headerPanelConfirm.text = title;
		questionPanelConfirm.text = question;
		
		yesButtonPanelConfirm.gameObject.SetActive (true);
		noButtonPanelConfirm.gameObject.SetActive (true);
	}

	void yesEvent (VirtualCurrencyPack cp){
		if (confirmyesButtoncp != null){
			confirmyesButtoncp(cp);
		}

	}

	void yesEvent (VirtualGood vg){
		if (confirmyesButtonvg != null){
			confirmyesButtonvg(vg);
		}
		
	}

	void ClosePanelConfirm () {
		ModalPanelConfirmObject.SetActive (false);
	}

	void ClosePanelCorrect(){
		ModalPanelCorrectObject.SetActive (false);
	}
}
