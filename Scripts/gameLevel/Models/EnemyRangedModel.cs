using UnityEngine;
using System.Collections;

public class EnemyRangedModel : MonoBehaviour {

	public float routingSpeed;
	
	public int routingAcc;

	public float backingSpeed;
	public int backingAcc;

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

	public float getBackingSpeed (){
		return this.backingSpeed;
	}
	
	public int getBackingAcc (){
		return this.backingAcc;
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
				GetComponent<EnemyRangedIA>().setAnim("attack", true);
				catched();
			}
		}
	}
}
