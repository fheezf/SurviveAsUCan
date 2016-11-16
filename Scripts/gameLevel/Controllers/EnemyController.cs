using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private GameObject[] enemyGuardians;
	private GameObject[] enemies;
	private GameObject[] enemyMages;
	private GameObject[] enemyRanged;
	private int aux;

	public int generateEnemy (){
		aux = UnityEngine.Random.Range (0, 4);
		switch (aux){
		case 0:
			return 0;
		case 1:
			return 1;
		case 2:
			return 2;
		default:
			return 3;
		}
	}

	public Vector3 generateEnemyPos(){
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			return new Vector3(-25.3f, 1.02f, UnityEngine.Random.Range(-49.9f, 47.4f));
		case 1:
			return new Vector3(18.6f, 1.02f, UnityEngine.Random.Range(-49.9f, 47.4f));
		case 2:
			return new Vector3(UnityEngine.Random.Range(-25.3f, 18.6f), 1.02f, -49.9f);
		default:
			return new Vector3(UnityEngine.Random.Range(-25.3f, 18.6f), 1.02f, 47.4f);
		}
	}

	public void enemyStart(int aux, GameObject gameObject){
		switch(aux){
		case 0:
			gameObject.GetComponent<EnemyIA>().rePosition();
			gameObject.GetComponent<EnemyIA>().setRouting();
			break;
		case 1:
			gameObject.GetComponent<EnemyGuardianIA>().rePosition();
			gameObject.GetComponent<EnemyGuardianIA>().setRouting();
			break;
		case 2:
			gameObject.GetComponent<EnemyMageIA>().rePosition();
			gameObject.GetComponent<EnemyMageIA>().setRouting();
			break;
		default:
			gameObject.GetComponent<EnemyRangedIA>().rePosition();
			gameObject.GetComponent<EnemyRangedIA>().setRouting();
			break;
			
		}
	}


	public void back(){
		enemyGuardians = GameObject.FindGameObjectsWithTag ("EnemyGuardian");
		enemies = GameObject.FindGameObjectsWithTag("enemy");
		enemyMages = GameObject.FindGameObjectsWithTag("enemyMage");
		enemyRanged = GameObject.FindGameObjectsWithTag("enemyRanged");
		if (enemies.Length >= 1) {
			foreach (GameObject ene in enemies) {
				ene.GetComponent<EnemyIA> ().setBacking ();
			}
		}
		if (enemyGuardians.Length >= 1) {
			foreach (GameObject ene in enemyGuardians) {
				ene.GetComponent<EnemyGuardianIA> ().setBacking ();
			}
		}
		if (enemyMages.Length >= 1) {
			foreach (GameObject ene in enemyMages) {
				ene.GetComponent<EnemyMageIA> ().setBacking ();
			}
		}
		if (enemyRanged.Length >= 1) {
			foreach (GameObject ene in enemyRanged) {
				ene.GetComponent<EnemyRangedIA> ().setBacking ();
			}
		}
	}

	public void gameOver(){
		enemyGuardians = GameObject.FindGameObjectsWithTag ("EnemyGuardian");
		enemies = GameObject.FindGameObjectsWithTag("enemy");
		enemyMages = GameObject.FindGameObjectsWithTag("enemyMage");
		enemyRanged = GameObject.FindGameObjectsWithTag("enemyRanged");
		if (enemies.Length >= 1) {
			foreach (GameObject ene in enemies) {
				ene.GetComponent<EnemyIA> ().Ended ();
			}
		}
		if (enemyGuardians.Length >= 1) {
			foreach (GameObject ene in enemyGuardians) {
				ene.GetComponent<EnemyGuardianIA> ().Ended ();
			}
		}
		if (enemyMages.Length >= 1) {
			foreach (GameObject ene in enemyMages) {
				ene.GetComponent<EnemyMageIA> ().Ended ();
			}
		}
		if (enemyRanged.Length >= 1) {
			foreach (GameObject ene in enemyRanged) {
				ene.GetComponent<EnemyRangedIA> ().Ended ();
			}
		}
	}

	public void moveGuardians(){
		enemyGuardians = GameObject.FindGameObjectsWithTag("EnemyGuardian");
		if (enemyGuardians.Length > 0) {
			foreach (GameObject aux in enemyGuardians){
				aux.GetComponent<EnemyGuardianIA>().callGenerate();
			}
		}
	}
}
