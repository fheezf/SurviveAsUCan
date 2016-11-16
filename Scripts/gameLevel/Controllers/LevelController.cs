using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	private PlayerController playerController;
	private EnemyController enemyController;
	private ViewController viewController;
	private ControllerObjetive objetiveController;
	private ObstacleController obstacleController;
	private PowerUpController powerupController;
	private SpearGeneralController spearController;
	private Pool poolController;
    private SoundController soundController;
	
	public float CharacterBlinkTime;
	public float PowerUpCDTime;
	public GameObject pj;

	private Vector3 objetivePos;

	private int genEnemy;
	private GameObject enemyGameObject;
	private Vector3 enemyPos;

	private Vector3 powerUpPos;

	private GameObject characterPrefab;
	private GameObject obstacleGameObject;
	private GameObject spearGameObject;
	void Start () {
	 

		playerController = pj.GetComponent<PlayerController> ();
		enemyController = GameObject.Find ("Controllers").GetComponent<EnemyController> ();
		viewController = GameObject.Find ("Controllers").GetComponent<ViewController> ();
		objetiveController = GameObject.Find ("Controllers").GetComponent<ControllerObjetive> ();
		obstacleController = GameObject.Find ("Controllers").GetComponent<ObstacleController> ();
		powerupController = GameObject.Find ("Controllers").GetComponent<PowerUpController> ();
		spearController = GameObject.Find ("Controllers").GetComponent<SpearGeneralController> ();
		poolController = GameObject.Find ("Pool").GetComponent<Pool> ();
        soundController = GameObject.Find("InstanceSounds").GetComponent<SoundController>();
		characterPrefab = (GameObject)Instantiate(Resources.Load("MainCharacter/"+ PlayerPrefs.GetString("skin")));
		characterPrefab.transform.SetParent (pj.transform, false);
        soundController.PlayMusic((AudioClip)Resources.Load("Music/game"), true);
        playerController.getStarted ();
		poolController.instantiate ();
		objetivePos = objetiveController.Generate ();
		poolController.activate (4, objetivePos, Quaternion.identity);
		StartCoroutine (callGeneratePowerUp(PowerUpCDTime));
		PunctuationModel.startGame ();
		viewController.startGame ();

	}

	void OnEnable(){
		ObjetiveModel.adquired += sumaPuntos;
		EnemyModel.catched += hitted;
		EnemyGuardianModel.catched += hitted;
		EnemyMageModel.catched += hitted;
		EnemyRangedModel.catched += hitted;
		ObstacleModel.catched += hitted;
		PlayerModel.actualizeHeart += drawHearts;
		EnemyMageIA.fire += generateObstacle;
		EnemyRangedIA.fire += generateSpear;
		SpearModel.catched += hitted;
		PowerUpController.generatePowerUp += generatePowerUp;
	}

	void OnDisable(){
		ObjetiveModel.adquired -= sumaPuntos;
		EnemyModel.catched -= hitted;
		EnemyGuardianModel.catched -= hitted;
		EnemyMageModel.catched -= hitted;
		EnemyRangedModel.catched -= hitted;
		ObstacleModel.catched -= hitted;
		PlayerModel.actualizeHeart -= drawHearts;
		EnemyMageIA.fire -= generateObstacle;
		EnemyRangedIA.fire -= generateSpear;
		SpearModel.catched -= hitted;
		PowerUpController.generatePowerUp -= generatePowerUp;
	}

	private void sumaPuntos() {
		PunctuationModel.sumaPuntos ();
		viewController.actualizePunctuation ();
		objetivePos = objetiveController.Generate ();
		poolController.activate (4, objetivePos, Quaternion.identity);
		enemyController.moveGuardians ();
		if (PunctuationModel.getPuntos() % 3 == 0) {
			genEnemy = enemyController.generateEnemy();
			enemyPos = enemyController.generateEnemyPos();
			enemyGameObject = poolController.activate (genEnemy, enemyPos, Quaternion.identity);
			enemyController.enemyStart(genEnemy, enemyGameObject);
		}
	}

	private void generateObstacle() {
		obstacleGameObject = poolController.activate (6);
		obstacleController.generate(obstacleGameObject);
		obstacleGameObject.GetComponent<ObstacleModel> ().setAutoDestroy ();
	}

	private void generateSpear(Transform tnsf){
		spearController.generate (tnsf);
	}

	private void hitted(){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/enemyAttack"), pj.gameObject);
        pj.GetComponent<PlayerModel> ().remainLife ();
		if (pj.GetComponent<PlayerModel> ().getLife () > 0) {
			powerupController.setStealth();
			playerController.setBlink(CharacterBlinkTime);
			enemyController.back();
		}else{
            soundController.PlayMusic((AudioClip)Resources.Load("Music/endGame"), false);
            playerController.characterDied();
			enemyController.gameOver();
			PunctuationModel.setCoinsShop();
			StartCoroutine(EndView(3.0f));
		}
	}

	private void generatePowerUp (){
		StartCoroutine (callGeneratePowerUp(PowerUpCDTime));
	}

	IEnumerator EndView(float waitTime){
		yield return new WaitForSeconds (waitTime);
		viewController.EndGame ();
	}

	IEnumerator callGeneratePowerUp(float waitTime){
		yield return new WaitForSeconds (waitTime);
		powerUpPos = powerupController.generate ();
		poolController.activate (5, powerUpPos, Quaternion.identity);
	}

	private void drawHearts(){
		viewController.drawHearts ();
	}

	//GENERAL USES

	public void Restart(){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("EndView").gameObject);
        StartCoroutine(changeScene(0.5f, 1));
    }

	public void MainMenu(){
        soundController.PlaySFX((AudioClip)Resources.Load("Sounds/buttonNormal"), GameObject.Find("EndView").gameObject);
        StartCoroutine(changeScene(0.5f, 0));
    }

    IEnumerator changeScene(float waitTime, int index) {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(index);
    }
}
