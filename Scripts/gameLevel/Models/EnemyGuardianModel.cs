using System;
using System.Collections;
using UnityEngine;

public class EnemyGuardianModel : MonoBehaviour
{
	
	public float backingSpeed;
	public int backingAcc;
	public float routingSpeed;
	
	public int routingAcc;
	
	public float chasingSpeed;
	
	public int chasingAcc;
	public float maximumDistance;

	public delegate void Catched();
	
	public static event Catched catched;
	
	public float getRoutingSpeed()
	{
		return this.routingSpeed;
	}
	
	public int getRoutingAcc()
	{
		return this.routingAcc;
	}
	
	public float getChasingSpeed()
	{
		return this.chasingSpeed;
	}
	
	public int getChasingAcc()
	{
		return this.chasingAcc;
	}

	public float getBackingSpeed (){
		return this.backingSpeed;
	}
	
	public int getBackingAcc (){
		return this.backingAcc;
	}

	public float getMaximumDistance(){
		return maximumDistance;
	}
	
	private void OnTriggerEnter(Collider hit)
	{
		if (hit.gameObject.tag == "Player")
		{
			if (hit.gameObject.GetComponent<PlayerModel>().getStealth()){
				return;
			}
			if (catched != null)
			{
				GetComponent<EnemyGuardianIA>().setAnim("attack", true);
				catched();
			}
		}
	}
	
}