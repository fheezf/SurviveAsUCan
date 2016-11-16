using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;
public class Tienda : IStoreAssets {

	public int GetVersion() {
		return 1;
	}

	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{GAME_COIN};
	}

	public VirtualGood[] GetGoods() {
		return new VirtualGood[] {BLUE_SKIN_GOOD, GREEN_SKIN_GOOD, RED_SKIN_GOOD, SPEC_SKIN_GOOD, NO_ADS_LTVG};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] {TENGC_PACK, FIFTYGC_PACK, FOURHUNDGC_PACK, THOUSANDGC_PACK};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{GENERAL_CATEGORY};
	}

	public const string GAME_COIN_ITEM_ID = "game_coin";

	public const string TENGC_PACK_PRODUCT_ID      = "10_game_coin";
	
	public const string FIFTYGC_PACK_PRODUCT_ID    = "50_game_coin";
	
	public const string FOURHUNDGC_PACK_PRODUCT_ID = "400_game_coin";
	
	public const string THOUSANDGC_PACK_PRODUCT_ID = "1000_game_coin";
	
	public const string BLUE_SKIN_ITEM_ID   = "blue_skin";

	public const string GREEN_SKIN_ITEM_ID = "green_skin";

	public const string RED_SKIN_ITEM_ID = "red_skin";

	public const string SPEC_SKIN_ITEM_ID = "spec_skin";
	
	public const string NO_ADS_LIFETIME_PRODUCT_ID = "no_ads";

	public static VirtualCurrency GAME_COIN = new VirtualCurrency(
		"Coins",										// name
		"",												// description
		GAME_COIN_ITEM_ID							// item id
	);

	public static VirtualCurrencyPack TENGC_PACK = new VirtualCurrencyPack(
		"10 game coins",                                   // name
		"Test refund of an item",                       // description
		"10_game_coin",                                   // item id
		10,												// number of currencies in the pack
		GAME_COIN_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(TENGC_PACK_PRODUCT_ID, 0.99)
		);
	
	public static VirtualCurrencyPack FIFTYGC_PACK = new VirtualCurrencyPack(
		"50 game coins",                                   // name
		"Test cancellation of an item",                 // description
		"50_game_coin",                                   // item id
		50,                                             // number of currencies in the pack
		GAME_COIN_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(FIFTYGC_PACK_PRODUCT_ID, 1.99)
		);
	
	public static VirtualCurrencyPack FOURHUNDGC_PACK = new VirtualCurrencyPack(
		"400 game coins",                                  // name
		"Test purchase of an item",                 	// description
		"400_game_coin",                                  // item id
		400,                                            // number of currencies in the pack
		GAME_COIN_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(FOURHUNDGC_PACK_PRODUCT_ID, 4.99)
		);
	
	public static VirtualCurrencyPack THOUSANDGC_PACK = new VirtualCurrencyPack(
		"1000 game coins",                                 // name
		"Test item unavailable",                 		// description
		"1000_game_coin",                                 // item id
		1000,                                           // number of currencies in the pack
		GAME_COIN_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(THOUSANDGC_PACK_PRODUCT_ID, 8.99)
		);
	
	/** Virtual Goods **/
	
	public static VirtualGood BLUE_SKIN_GOOD = new SingleUseVG(
		"Blue Skin",                                       		// name
		"Cambia el aspecto del jugador", // description
		"blue_skin",                                       		// item id
		new PurchaseWithVirtualItem(GAME_COIN_ITEM_ID, 225)); // the way this virtual good is purchased

	public static VirtualGood GREEN_SKIN_GOOD = new SingleUseVG (
		"Green Skin",											//name
		"Cambia el aspecto del jugador", 						//descripcion
		"green_skin",											//item id
		new PurchaseWithVirtualItem (GAME_COIN_ITEM_ID, 225));

	public static VirtualGood RED_SKIN_GOOD = new SingleUseVG (
		"Red Skin",											//name
		"Cambia el aspecto del jugador", 						//descripcion
		"red_skin",											//item id
		new PurchaseWithVirtualItem (GAME_COIN_ITEM_ID, 225));

	public static VirtualGood SPEC_SKIN_GOOD = new SingleUseVG (
		"Spec Skin",											//name
		"Cambia el aspecto del jugador", 						//descripcion
		"spec_skin",											//item id
		new PurchaseWithVirtualItem (GAME_COIN_ITEM_ID, 500));

	/** Virtual Categories **/
	// The muffin rush theme doesn't support categories, so we just put everything under a general category.
	public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
		"General", new List<string>(new string[] { BLUE_SKIN_ITEM_ID, GREEN_SKIN_ITEM_ID, RED_SKIN_ITEM_ID, SPEC_SKIN_ITEM_ID })
		);
	
	
	/** LifeTimeVGs **/
	// Note: create non-consumable items using LifeTimeVG with PuchaseType of PurchaseWithMarket
	public static VirtualGood NO_ADS_LTVG = new LifetimeVG(
		"No Ads", 														// name
		"No More Ads!",				 									// description
		"no_ads",														// item id
		new PurchaseWithMarket(NO_ADS_LIFETIME_PRODUCT_ID, 0.99));
}
