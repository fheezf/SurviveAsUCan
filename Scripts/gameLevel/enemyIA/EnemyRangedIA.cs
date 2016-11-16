using UnityEngine;
using System.Collections;

public class EnemyRangedIA : EnemyController {

	public enum enemyState
	{
		Routine,
		Looking,
		Backing,
		dontMove
	}
	public float lookingTime;
	public float fireRate;

	private GameObject pj;
	private float nextFire;
	private Animator anim;
	private NavMeshAgent nav;
	
	private GameObject waypoints;
	
	private int indexWaypoints;
	
	private GameObject[] Routing = new GameObject[4];
	private GameObject BackWaypoint;
	private int indexRoute;
	
	private int indexInit;
	
	private enemyState currentState;
	
	public delegate void Fire(Transform transform);
	
	public static event Fire fire;
	
	
	private void Awake()
	{
		pj = GameObject.FindWithTag("Player");
		nextFire = Time.time;
		nav = base.GetComponent<NavMeshAgent>();
		waypoints = GameObject.Find("Waypoints");
		anim = GetComponent<Animator> ();
	}

	public void rePosition(){
		generateRoute();
		transform.LookAt(new Vector3(Routing[indexRoute].transform.position.x, this.transform.position.y, Routing[indexRoute].transform.position.z));
		StartCoroutine(FSM());
	}

	protected IEnumerator FSM()
	{
		while(true){
			yield return StartCoroutine (currentState.ToString());
		}
	}
	
	
	private IEnumerator Routine()
	{
		while (currentState == enemyState.Routine){
			nav.SetDestination(Routing[indexRoute].transform.position);
			if ((Mathf.Abs(this.transform.position.x - Routing[indexRoute].transform.position.x) < 0.5f) &&
			    (Mathf.Abs(this.transform.position.z - Routing[indexRoute].transform.position.z) < 0.5f)){
				setLooking();
			}
			yield return 0;
		}
	}

	private IEnumerator Looking(){
		while (currentState == enemyState.Looking){
			nav.SetDestination(this.gameObject.transform.position);
			yield return 0;
		}
	}

	private IEnumerator Backing() {
		while (currentState == enemyState.Backing){
			nav.SetDestination(BackWaypoint.transform.position);
			if ((Mathf.Abs(this.transform.position.x - BackWaypoint.transform.position.x) < 0.5f) &&
			    (Mathf.Abs(this.transform.position.z - BackWaypoint.transform.position.z) < 0.5f)){
				gameObject.SetActive(false);
			}
			yield return 0;
		}
	}
	
	private IEnumerator dontMove()
	{
		while (currentState == enemyState.dontMove){
			nav.SetDestination(this.gameObject.transform.position);
			yield return 0;
		}
	}
	
	private void nextStepRoutine()
	{
		if (this.indexRoute < 3)
		{
			this.indexRoute++;
			transform.LookAt(new Vector3(Routing[indexRoute].transform.position.x, this.transform.position.y, Routing[indexRoute].transform.position.z));
		}
		else
		{
			this.indexRoute = 0;
			transform.LookAt(new Vector3(Routing[indexRoute].transform.position.x, this.transform.position.y, Routing[indexRoute].transform.position.z));
		}
	}
	
	public void generateRoute()
	{
		indexInit = 0;
		int num = 0;
		while (this.indexInit < 4)
		{
			num = UnityEngine.Random.Range(0, 24);
			if (!this.isRepeated(num))
			{
				this.Routing[this.indexInit] = this.waypoints.transform.GetChild(num).gameObject;
				this.indexInit++;
			}
		}
		num = UnityEngine.Random.Range (0, 15);
		switch (num){
		case 0:
			BackWaypoint = this.waypoints.transform.GetChild(num).gameObject;
			break;
		case 1:
			BackWaypoint = this.waypoints.transform.GetChild(num).gameObject;
			break;
		case 2:
			BackWaypoint = this.waypoints.transform.GetChild(num).gameObject;
			break;
		case 3:
			BackWaypoint = this.waypoints.transform.GetChild(num).gameObject;
			break;
		case 4:
			BackWaypoint = this.waypoints.transform.GetChild(num).gameObject;
			break;
		case 5:
			BackWaypoint = this.waypoints.transform.GetChild(num).gameObject;
			break;
		case 6:
			BackWaypoint = this.waypoints.transform.GetChild(9).gameObject;
			break;
		case 7:
			BackWaypoint = this.waypoints.transform.GetChild(10).gameObject;
			break;
		case 8:
			BackWaypoint = this.waypoints.transform.GetChild(14).gameObject;
			break;
		case 9:
			BackWaypoint = this.waypoints.transform.GetChild(15).gameObject;
			break;
		case 10:
			BackWaypoint = this.waypoints.transform.GetChild(19).gameObject;
			break;
		case 11:
			BackWaypoint = this.waypoints.transform.GetChild(20).gameObject;
			break;
		case 12:
			BackWaypoint = this.waypoints.transform.GetChild(21).gameObject;
			break;
		case 13:
			BackWaypoint = this.waypoints.transform.GetChild(22).gameObject;
			break;
		case 14:
			BackWaypoint = this.waypoints.transform.GetChild(23).gameObject;
			break;
		default:
			BackWaypoint = this.waypoints.transform.GetChild(24).gameObject;
			break;
			
		}
	}
	
	private bool isRepeated(int aux)
	{
		for (int i = 0; i < this.indexInit; i++)
		{
			if (this.Routing[i].GetInstanceID() == this.waypoints.transform.GetChild(aux).GetInstanceID())
			{
				return true;
			}
		}
		return false;
	}
	
	public void changeState(enemyState nextState)
	{
		if (nextState == enemyState.Looking) {
			StartCoroutine (delayAnimation(0.2f,"look",true));
			StartCoroutine(look(lookingTime));
		}

		if (nextState == enemyState.Routine) {
			StartCoroutine (delayAnimation(0.2f,"look",false));
			nav.speed = GetComponent<EnemyRangedModel> ().getRoutingSpeed ();
			nav.acceleration = (float)GetComponent<EnemyRangedModel> ().getRoutingAcc ();
		}
		if (nextState == enemyState.dontMove) {
			StartCoroutine (delayAnimation(0.2f,"end",true));
		}
		if (nextState == enemyState.Backing) {
			StartCoroutine (delayAnimation(0.5f,"attack",false));
			transform.LookAt(new Vector3(BackWaypoint.transform.position.x, this.transform.position.y, BackWaypoint.transform.position.z));
			this.nav.speed = base.GetComponent<EnemyRangedModel>().getBackingSpeed();
			this.nav.acceleration = (float)GetComponent<EnemyRangedModel>().getBackingAcc();
		}
		this.currentState = nextState;
	}
	
	public void setRouting()
	{
		this.changeState(enemyState.Routine);
	}

	public void setLooking(){
		this.changeState(enemyState.Looking);
	}

	public void setBacking(){
		this.changeState (enemyState.Backing);
	}
	
	public void Ended()
	{
		this.changeState(enemyState.dontMove);
	}
	
	public string getState()
	{
		return this.currentState.ToString();
	}

	IEnumerator look (float waitTime){
		yield return new WaitForSeconds (waitTime);
		if (getState () == "dontMove") {
			yield break;
		}
		nextStepRoutine ();
		setRouting ();
	}

	public void Shoot (){
		if (Time.time > nextFire) {
			transform.LookAt(new Vector3(pj.transform.position.x, this.transform.position.y, pj.transform.position.z));
			nextFire = Time.time + fireRate;
			setAnim("attack", true);
			fire(this.transform);
			StartCoroutine (delayAnimation(0.5f,"attack",false));
		}
	}

	
	public void setAnim(string cadena, bool nuevo){
		anim.SetBool (cadena, nuevo);
	}
	
	IEnumerator delayAnimation (float waitTime, string cadena, bool booleano){
		yield return new WaitForSeconds (waitTime);
		setAnim (cadena, booleano);
	}

}
