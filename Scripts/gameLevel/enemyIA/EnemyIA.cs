using System.Collections;
using UnityEngine;

public class EnemyIA : EnemyController
{
	public enum enemyState
	{
		Routine,
		Chasing,
		Backing,
		dontMove
	}
	public float chasingTime;

	private GameObject pj;
	private Animator anim;
	private NavMeshAgent nav;
	private bool dontFollow;
	private GameObject waypoints;

	private int indexWaypoints;

	private GameObject[] Routing = new GameObject[4];
	private GameObject BackWaypoint;

	private int indexRoute;
	private float maxTime;
	private int indexInit;

	private enemyState currentState;
	

	private void Awake()
	{
		dontFollow = false;
		pj = GameObject.FindWithTag("Player");
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
				nextStepRoutine();
			}
			yield return 0;
		}
	}


	private IEnumerator Chasing()
	{
		while (currentState == enemyState.Chasing){
			if (pj.GetComponent<PlayerModel>().getStealth() || Time.time > maxTime){
				generateRoute();
				setRouting();
				dontFollow = true;
				StartCoroutine(canFollow(2f));
			}
			transform.LookAt(new Vector3(pj.transform.position.x, this.transform.position.y, pj.transform.position.z));
			nav.SetDestination(pj.transform.position);
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

	public void changeState(EnemyIA.enemyState nextState)
	{
		if (nextState == enemyState.Routine)
		{
			nav.speed = GetComponent<EnemyModel>().getRoutingSpeed();
			nav.acceleration = (float)GetComponent<EnemyModel>().getRoutingAcc();
		}
		if (nextState == enemyState.Chasing)
		{
			if (dontFollow){
				return;
			}else{
				maxTime = Time.time + chasingTime;
				this.nav.speed = base.GetComponent<EnemyModel>().getChasingSpeed();
				this.nav.acceleration = (float)GetComponent<EnemyModel>().getChasingAcc();
			}
		}
		if (nextState == enemyState.dontMove) {
			StartCoroutine (delayAnimation(0.2f,"end",true));
		}
		if (nextState == enemyState.Backing) {
			StartCoroutine (delayAnimation(0.5f,"attack",false));
			transform.LookAt(new Vector3(BackWaypoint.transform.position.x, this.transform.position.y, BackWaypoint.transform.position.z));
			this.nav.speed = base.GetComponent<EnemyModel>().getBackingSpeed();
			this.nav.acceleration = (float)GetComponent<EnemyModel>().getBackingAcc();
		}
		this.currentState = nextState;
	}

	public void setRouting()
	{
		this.changeState(EnemyIA.enemyState.Routine);
	}

	public void setChasing()
	{
		this.changeState(EnemyIA.enemyState.Chasing);
	}

	public void setBacking(){
		this.changeState (EnemyIA.enemyState.Backing);
	}

	public void Ended()
	{
		this.changeState(EnemyIA.enemyState.dontMove);
	}

	public string getState()
	{
		return this.currentState.ToString();
	}

	IEnumerator canFollow(float waitTime){
		yield return new WaitForSeconds (waitTime);
		dontFollow = false;
	}


	public void setAnim(string cadena, bool nuevo){
		anim.SetBool (cadena, nuevo);
	}

	IEnumerator delayAnimation (float waitTime, string cadena, bool booleano){
		yield return new WaitForSeconds (waitTime);
		setAnim (cadena, booleano);
	}
}