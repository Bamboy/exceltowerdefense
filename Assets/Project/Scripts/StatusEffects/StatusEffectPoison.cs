using UnityEngine;
using System.Collections;
using Excelsion.Enemies;

// Matt McGrath - 4/18/2015

// Sloppy derived class of StatusEffect. Will make StatusEffect abstract and use virtual methods later.
public class StatusEffectPoison : StatusEffect 
{
	public float poison = 0.0f;								// This acts as our timer.
	public bool IsPoisoned{ get{ return poison > 0.0f; } }	// Return true if our timer is active.
	public float poisonMovementModifier = 0.10f; 			// Percentage at which effected enemies will move at. (25% normal speed, here)
	public int poisonDamageTick = 1;

	private float tickTime = 0f;

	public StatusEffectPoison(Enemy targetEnemy, float duration)
	{
		enemyAffected = targetEnemy;
		effectDuration = duration;
		
		statusEffectType = StatusEffectType.Poison;
	}
	
	public override void EvaluateStatusEffect ()
	{
		EvaluatePoison();
	}
	
	// We can use this Constructor if we create an instance of StatusEffectFire and pass it our desired duration.
	public override void InflictStatusEffect()
	{
		SetPoison(effectDuration);
	}
	
	public void SetPoison(float duration )
	{
		Debug.Log("poisoned something!");
		if( IsPoisoned )
			poison += duration;
		else
		{   
			enemyAffected.Speed = poisonMovementModifier * enemyAffected.defaultSpeed;
			poison = duration;
			enemyAffected.healthDisplay.ShowPoison(true);//make show poison method
		}
	}
	void EvaluatePoison()
	{
		if( IsPoisoned )
		{
			poison -= Time.deltaTime;
			if( IsPoisoned == false ) 
			{

				poison = 0.0f;
				enemyAffected.Speed = enemyAffected.defaultSpeed; 	//Restore our normal speed.
				enemyAffected.healthDisplay.ShowPoison(false);	//make ShowPoison
			}
		}


		if (IsPoisoned)
		{
			// Lower our poison duration. Reaching 0 indicates it's done).
			poison -= Time.deltaTime;
			
			// Lower our tick timer. When it's less than 0 it's time to apply damage.
			tickTime -= Time.deltaTime;
			
			// If during this update we're no longer poisoned, stop.
			if( IsPoisoned == false )
			{
				//Debug.Log ("I'm not on fire anymore =D!");
				
				// We're no longer burning; stop the fire.
				poison = 0.0f;
				
				// Stop the Fire indicator as well.
				enemyAffected.healthDisplay.ShowPoison(false);
			}
			
			if (IsPoisoned && tickTime <= 0f)
			{

				// Deal the damage, but then wait until tickTime before dealing it again.
				enemyAffected.Damage( poisonDamageTick );
				
				// Instead of yielding, we'll reset our tickTime so we do not enter this if statement until tickTime is reached again.
				tickTime = 1.0f;		// Again, if one second is not standard, we'll need another variable here.
			}
		}
	}
}