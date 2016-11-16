using UnityEngine;
using System.Collections;
using Soomla.Store;
using Soomla;

public class InstanceShop : MonoBehaviour {
	private static InstanceShop instance = null;

	void Start () {
		if(instance == null){ 	//making sure we only initialize one instance.
			SoomlaStore.Initialize(new Tienda());
			instance = this;
			GameObject.DontDestroyOnLoad(this.gameObject);
		} else {					//Destroying unused instances.
			GameObject.Destroy(this.gameObject);
		}

		if (PlayerPrefs.GetInt("coins") > 0){
			StoreInventory.GiveItem("game_coin", PlayerPrefs.GetInt("coins"));
			PlayerPrefs.SetInt("coins", 0);
		}
	}

}
