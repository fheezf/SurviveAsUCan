using UnityEngine;
using System.Collections;

public class EnemyMageIA : EnemyController {

	public enum enemyState
	{
		Routine,
		Backing,
		dontMove
	}
	public float reCastTime;
	private float maxTime;

	private Animator anim;
	private NavMeshAgent nav;
	
	private GameObject waypoints;
	
	private int indexWaypoints;
	
	private GameObject[] Routing = new GameObject[4];
	private GameObject BackWaypoint;
	private int indexRoute;

	private int indexInit;
	
	private enemyState currentState;

	public delegate void Fire();
	
	public static event Fire fire;
	
	
	private void Awake()
	{
		maxTime = Time.time;
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
			if (Time.time > maxTime){
				setAnim("attack", true);
				yield return new WaitForSeconds(0.5f);
				setAnim("attack", false);
				fire();
				maxTime = Time.time + reCastTime;
			}
			nav.SetDestination(Routing[indexRoute].transform.position);
			if ((Mathf.Abs(this.transform.position.x - Routing[indexRoute].transform.position.x) < 0.5f) &&
			    (Mathf.Abs(this.transform.position.z - Routing[indexRoute].transform.position.z) < 0.5f)){
				nextStepRoutine();
			}
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
		if (nextState == enemyState.Routine) {
			nav.speed = GetComponent<EnemyMageModel> ().getRoutingSpeed ();
			nav.acceleration = (float)GetComponent<EnemyMageModel> ().getRoutingAcc ();
		}
		if (nextState == enemyState.dontMove) {
			StartCoroutine (delayAnimation(0.2f,"end",true));
		}
		if (nextState == enemyState.Backing) {
			StartCoroutine (delayAnimation(0.5f,"attack",false));
			transform.LookAt(new Vector3(BackWaypoint.transform.position.x, this.transform.position.y, BackWaypoint.transform.position.z));
			this.nav.speed = base.GetComponent<EnemyMageModel>().getBackingSpeed();
			this.nav.acceleration = (float)GetComponent<EnemyMageModel>().getBackingAcc();
		}
		this.currentState = nextState;
	}
	
	public void setRouting()
	{
		this.changeState(EnemyMageIA.enemyState.Routine);
	}

	public void setBacking(){
		this.changeState (EnemyMageIA.enemyState.Backing);
	}
	
	public void Ended()
	{
		this.changeState(EnemyMageIA.enemyState.dontMove);
	}
	
	public string getState()
	{
		return this.currentState.ToString();
	}
	
	public void setAnim(string cadena, bool nuevo){
		anim.SetBool (cadena, nuevo);
	}
	
	IEnumerator delayAnimation (float waitTime, string cadena, bool booleano){
		yield return new WaitForSeconds (waitTime);
		setAnim (cadena, booleano);
	}

}
