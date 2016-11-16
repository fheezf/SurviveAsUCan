using UnityEngine;
using System.Collections;

public class PunctuationModel : MonoBehaviour {
	private static int contadorPartida;

	public delegate void PointUp();
	public static event PointUp actualizeRecord;
	
	
	void Start(){
		contadorPartida = 0;
	}

	public static void startGame(){
		contadorPartida = 0;
	}

	public static void sumaPuntos(){
		contadorPartida++;
		setRecord ();
	}
	
	public static int getPuntos(){
		return contadorPartida;
	}

	public static void setCoinsShop(){
		PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + contadorPartida);
	}

	private static void setRecord(){
		if (contadorPartida > PlayerPrefs.GetInt ("record")) {
			PlayerPrefs.SetInt ("record", contadorPartida);
			if (actualizeRecord != null) {
				actualizeRecord();
			}
		}
	}
	
	public static int getRecord(){
		return PlayerPrefs.GetInt("record");
	}
}
