using System;
using System.Collections;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
	public float speed = 15f;
	public int lives = 3;
	public float rotationSpeed = 90f;

	public delegate void ActualizeHeart ();
	public static event ActualizeHeart actualizeHeart;

	private bool stealth;

	void Start (){
		stealth = false;
	}

	void OnEnable(){
		PowerUpController.pjstealth += setStealth;
	}

	void OnDisable(){
		PowerUpController.pjstealth -= setStealth;
	}

	public void remainLife(){
		lives--;
		if (actualizeHeart != null) {
			actualizeHeart();
		}
	}

	public void addLife(){
		if (lives < 13) {
			lives++;
			if (actualizeHeart != null) {
				actualizeHeart();
			}
		}
	}

	public int getLife(){
		return lives;
	}

	private void setStealth (bool stea){
		stealth = stea;
		if (stea) {
			Color color = transform.GetChild (0).GetChild (0).GetComponent<SkinnedMeshRenderer> ().material.color;
			color.a -= 0.6f;
			transform.GetChild (0).GetChild (0).GetComponent<SkinnedMeshRenderer> ().material.color = color;
		} else {
			Color color = transform.GetChild (0).GetChild (0).GetComponent<SkinnedMeshRenderer> ().material.color;
			color.a += 0.6f;
			transform.GetChild (0).GetChild (0).GetComponent<SkinnedMeshRenderer> ().material.color = color;
		}
	}

	public bool getStealth (){
		return stealth;
	}

	public void setMovementSpeed(float speed)
	{
		this.speed = speed;
	}
	
	public float getMovementSpeed()
	{
		return this.speed;
	}
	
	public void setRotationSpeed(float speed)
	{
		this.rotationSpeed = speed;
	}
	
	public float getRotationSpeed()
	{
		return this.rotationSpeed;
	}
}
