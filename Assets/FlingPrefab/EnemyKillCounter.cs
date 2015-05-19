using System.Collections;
using UnityEngine;

// Matt McGrath - 5/18/2015.

// Script which we will temporarily attach to the SlingProjectile to keep track of "Kills."
public class EnemyKillCounter : MonoBehaviour 
{
	private static uint numberOfKills;

	// We want access to this value, but we don't want to allow other classes to change its value.
	public static uint NumberOfKills
	{
		get { return numberOfKills; }
		private set { numberOfKills = value; }
	}


	void OnCollisionEnter(Collision col)
	{
		// If the collision was with an Enemy tagged object...
		if (col.gameObject.tag == "Enemy")
		{
			// In this prototype, we assume (for now) that a collision with the projectile results in death. Later we will obviously need to see if the enemy's health is < 0.
			numberOfKills++;
		}
	}
}
