using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {
	public float basicPowerUpTime;
	public float StealthPowerUpTime;

	public GameObject smokeScreen;
	public GameObject magnetPlayer;
	public GameObject moreHealth;

	private GameObject instanceMagnet;

	public Camera mainCamera;
	public GameObject prefab;
	private static float depth;
	private static Vector3 upperLeftScreen;
	private static Vector3 lowerRightScreen;
	private bool again;
	private bool doitagain;

	public delegate void PjStealth(bool booleano);
	public static event PjStealth pjstealth;

	public delegate void GeneratePowerUp();
	public static event GeneratePowerUp generatePowerUp;

	public enum powerupState {
		SpeedUp,
		ImantedObjetive,
		EnemySlow,
		Stealth,
		Life,
		None
	}
	private powerupState currentState;
	private powerupState prevState;

	void Start(){
		depth = 0.82f - mainCamera.transform.position.y;
		upperLeftScreen = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width*0.1f, Screen.height*0.1f, depth ));
		lowerRightScreen = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width*0.9f, Screen.height*0.9f, depth));
		again = true;
		doitagain = false;
	}
	
	void OnEnable()
	{
		PowerUpModel.poweruptaken += PowerUpTaken;
	}
	
	
	void OnDisable()
	{
		PowerUpModel.poweruptaken -= PowerUpTaken;
	}

	void Awake(){
		setNone ();
		StartCoroutine(FSM());
	}

	public powerupState getCurrentState(){
		return currentState;
	}

	protected IEnumerator FSM(){
		while(true){
			yield return StartCoroutine (currentState.ToString());
		}
	}

	IEnumerator None(){
		while (currentState == powerupState.None){
			yield return 0;
		}
	}

	IEnumerator Stealth(){
		while (currentState == powerupState.Stealth){
			yield return 0;
		}
	}

	IEnumerator Life(){
		while (currentState == powerupState.Life){
			yield return 0;
		}
	}

	IEnumerator SpeedUp(){
		while (currentState == powerupState.SpeedUp){
			GameObject.FindWithTag("Player").GetComponent<PlayerModel>().setMovementSpeed(20f);
			yield return 0;
		}
	}

	IEnumerator ImantedObjetive(){
		while (currentState == powerupState.ImantedObjetive){
			GameObject.FindWithTag("Objetive").GetComponent<SphereCollider>().radius = 2.5f;
			yield return 0;
		}
	}

	IEnumerator EnemySlow(){
		while (currentState == powerupState.EnemySlow){
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("enemy")){
				if (enemy.GetComponent<EnemyIA>().getState() == "Routine"){
					enemy.GetComponent<NavMeshAgent>().speed = enemy.GetComponent<EnemyModel>().getRoutingSpeed() - 5.0f;
				}else{
					enemy.GetComponent<NavMeshAgent>().speed = enemy.GetComponent<EnemyModel>().getChasingSpeed() - 5.0f;
				}
			}
			foreach(GameObject enemyGuardian in GameObject.FindGameObjectsWithTag("EnemyGuardian")){
				if (enemyGuardian.GetComponent<EnemyGuardianIA>().getState() == "Routine"){
					enemyGuardian.GetComponent<NavMeshAgent>().speed = enemyGuardian.GetComponent<EnemyGuardianModel>().getRoutingSpeed() - 5.0f;
				}else{
					enemyGuardian.GetComponent<NavMeshAgent>().speed = enemyGuardian.GetComponent<EnemyGuardianModel>().getChasingSpeed() - 5.0f;
				}
			}
			foreach (GameObject enemyMage in GameObject.FindGameObjectsWithTag("enemyMage")) {
				if (enemyMage.GetComponent<EnemyMageIA> ().getState () == "Routine") {
					enemyMage.GetComponent<NavMeshAgent> ().speed = enemyMage.GetComponent<EnemyMageModel> ().getRoutingSpeed () - 5.0f;
				}
			}
			foreach (GameObject enemyRanged in GameObject.FindGameObjectsWithTag("enemyRanged")) {
				if (enemyRanged.GetComponent<EnemyRangedIA> ().getState () == "Routine") {
					enemyRanged.GetComponent<NavMeshAgent> ().speed = enemyRanged.GetComponent<EnemyRangedModel> ().getRoutingSpeed () - 5.0f;
				}
			}
			yield return 0;
		}
	}

	public void changeState(powerupState nextState){
		prevState = currentState;
		if (prevState == powerupState.Stealth) {
			if (pjstealth != null){
				pjstealth (false);
			}
		}
		if (prevState == powerupState.SpeedUp) {
			setNormalSpeed();
		}
		if (prevState == powerupState.ImantedObjetive) {
			setNormalRadius();
			Destroy(instanceMagnet);
		}
		if (prevState == powerupState.EnemySlow) {
			setNormalEnemySpeed();
		}
		if (nextState == powerupState.Life) {
			GameObject.FindWithTag("Player").GetComponent<PlayerModel>().addLife();
			Instantiate(moreHealth);
		}
		if (nextState == powerupState.ImantedObjetive) {
			instanceMagnet = Instantiate(magnetPlayer);
			Vector3 newpos = new Vector3(0,0,0);
			instanceMagnet.transform.SetParent(GameObject.FindWithTag("Player").transform);
			instanceMagnet.transform.localPosition = newpos;
		}
		if (nextState != powerupState.None && nextState != powerupState.Stealth) {
			StartCoroutine (PowerUpEnabled (basicPowerUpTime));
		} else {
			if (nextState == powerupState.Stealth){
				if (pjstealth != null){
					smokeScreen.transform.position = GameObject.FindWithTag("Player").transform.position;
					Instantiate(smokeScreen);
					pjstealth(true);
				}
				StartCoroutine(PowerUpEnabled(StealthPowerUpTime));
			}
		}
		currentState = nextState;
	}
	
	public void setSpeedUp(){
		changeState(powerupState.SpeedUp);
	}
	
	public void setEnemySlow(){
		changeState(powerupState.EnemySlow);
	}
	
	public void setImantedObjetive(){
		changeState(powerupState.ImantedObjetive);
	}

	public void setStealth(){
		changeState (powerupState.Stealth);
	}

	public void setNone(){
		changeState (powerupState.None);
	}

	public void setLife (){
		changeState (powerupState.Life);
	}

	private void PowerUpTaken (){
		switch (UnityEngine.Random.Range(0, 5)) {
		case 0:
			setSpeedUp ();
			break;
		case 1:
			setImantedObjetive ();
			break;
		case 2:
			setEnemySlow ();
			break;
		case 3:
			setStealth();
			break;
		case 4:
			setLife();
			break;
		}

		if (generatePowerUp != null){
			generatePowerUp();
		}
	}

	private void setNormalSpeed(){
		GameObject.FindWithTag("Player").GetComponent<PlayerModel>().setMovementSpeed(15f);
	}

	private void setNormalRadius(){
		GameObject.FindWithTag("Objetive").GetComponent<SphereCollider>().radius = 0.5f;
	}

	private void setNormalEnemySpeed(){
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("enemy")){
			if (enemy.GetComponent<EnemyIA>().getState() == "Routine"){
				enemy.GetComponent<NavMeshAgent>().speed = enemy.GetComponent<EnemyModel>().getRoutingSpeed();
			}else{
				enemy.GetComponent<NavMeshAgent>().speed = enemy.GetComponent<EnemyModel>().getChasingSpeed();
			}
		}
		foreach(GameObject enemyGuardian in GameObject.FindGameObjectsWithTag("EnemyGuardian")){
			if (enemyGuardian.GetComponent<EnemyGuardianIA>().getState() == "Routine"){
				enemyGuardian.GetComponent<NavMeshAgent>().speed = enemyGuardian.GetComponent<EnemyGuardianModel>().getRoutingSpeed();
			}else{
				enemyGuardian.GetComponent<NavMeshAgent>().speed = enemyGuardian.GetComponent<EnemyGuardianModel>().getChasingSpeed();
			}
		}
		foreach (GameObject enemyMage in GameObject.FindGameObjectsWithTag("enemyMage")) {
			if (enemyMage.GetComponent<EnemyMageIA> ().getState () == "Routine") {
				enemyMage.GetComponent<NavMeshAgent> ().speed = enemyMage.GetComponent<EnemyMageModel> ().getRoutingSpeed ();
			}
		}
		foreach (GameObject enemyRanged in GameObject.FindGameObjectsWithTag("enemyRanged")) {
			if (enemyRanged.GetComponent<EnemyRangedIA> ().getState () == "Routine") {
				enemyRanged.GetComponent<NavMeshAgent> ().speed = enemyRanged.GetComponent<EnemyRangedModel> ().getRoutingSpeed ();
			}
		}
	}

	IEnumerator PowerUpEnabled(float waitTime){
		yield return new WaitForSeconds (waitTime);
		setNone ();
	}


	public Vector3 generate(){
		again = true;
		Vector3 pwu = new Vector3 (0, 0, 0);
		GameObject pj = GameObject.FindWithTag("Player");
		GameObject[] obs = GameObject.FindGameObjectsWithTag ("obstacle");
		while (again) { //Generamos la posicion del power up de manera iterativa para que no se genere cerca del jugador.
			pwu = new Vector3(Random.Range(upperLeftScreen.x,lowerRightScreen.x),0.50f,Random.Range(upperLeftScreen.z,lowerRightScreen.z));
			if ((Mathf.Abs(pwu.x - pj.transform.position.x) < 2.5f) ||
			    (Mathf.Abs(pwu.z - pj.transform.position.z) < 2.5f)){
				again = true;
			}else{
				if (obs.Length > 0){
					foreach (GameObject go in obs) {
						doitagain = false;
						if ((Mathf.Abs(pwu.x - go.transform.position.x) < 2.5f) ||
					  	  (Mathf.Abs(pwu.z - go.transform.position.z) < 2.5f)){
							again = true;
							doitagain = true;
							break;
						}
					}
				}
				if (!doitagain){
					again = false;
				}

			}
		}
		return pwu;
		
	}
	
}
