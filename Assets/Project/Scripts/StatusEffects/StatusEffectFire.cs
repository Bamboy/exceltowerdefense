using UnityEngine;
using System.Collections;
using Excelsion.Enemies;

// Matt McGrath - 4/18/2015

// Sloppy derived class of StatusEffect. Will make StatusEffect abstract and use virtual methods later.
public class StatusEffectFire : StatusEffect 
{
	public float fire = 0f; 								// This acts as our timer.
	public bool OnFire{ get{ return fire > 0.0f; } } 		// Return true if our timer is active.
	public int fireTickDamage = 2; 							// Damage to do every second
	private int numberOfTicks = 0;							// Lets keep track of how many times the fire has ticked the enemy; just for fun.
	
	// Trying this to replace use of co-routine.
	private float tickTime = 0f;							// We'll apply damage-over-time, once every second (is 1 second standard? if not this will need to be public).

	// Constructor which takes the Enemy that was hit and the duration he should be set on fire.
	public StatusEffectFire(Enemy targetEnemy, float duration)
	{
		enemyAffected = targetEnemy;
		effectDuration = duration;

		// Let's internally set the enum type, in case we want to iterate through effects and do things depending on its type.
		// For example: Let's say we apply freeze to an enemy and in the design it calls for removing the fire effect. We simply
		// check the Enemy's status effect's list, see if its StatusEffectType == Fire, then remove that effect.
		statusEffectType = StatusEffectType.Fire;

		IsEffectStackable = false;
	}

	public override void EvaluateStatusEffect ()
	{
		EvaluateFire();
	}

	// We can use this Constructor if we create an instance of StatusEffectFire and pass it our desired duration.
	public override void InflictStatusEffect()
	{
		//Debug.Log ("Setting Fire to Enemy " + enemyAffected.ToString ());
		SetFire(effectDuration);
	}

	// We don't need the duration paramater really, but lets keep this until we work more on this.
	public void SetFire(float duration)
	{
		// Let's do this check in the Item class instead -- FOR NOW.
		// If we're already burning, don't burn us more, please :(
//		if (OnFire && !IsEffectStackable)
//		{
//			Debug.Log("I'm already burning, sir!");
//			return;
//		}

		if (OnFire)
		{
			// Add time to existing fire.
			fire += duration;

			//Debug.Log("We're on Fire and adding the fire duration of: " + fire.ToString ());
		}
		else
		{ 	
			// Start new fire.
			fire = duration;
			//Debug.Log ("Starting new fire with duration of: " + fire.ToString ());

			enemyAffected.healthDisplay.ShowFire(true);
			//Debug.Log("Enemy Affected = " + enemyAffected.ToString());
		}
	}

	// Handles the Fire effect: Dealing damage over time.
	void EvaluateFire()
	{
		if (OnFire)
		{
			// Lower our fire duration. Reaching 0 indicates it's done).
			fire -= Time.deltaTime;

			// Lower our tick timer. When it's less than 0 it's time to apply damage.
			tickTime -= Time.deltaTime;
			//Debug.Log("I'm on fire for " + fire.ToString () + " more seconds!");

			// If during this update we're no longer on fire, stop the burning.
			if( OnFire == false )
			{
				//Debug.Log ("I'm not on fire anymore =D!");
				
				// We're no longer burning; stop the fire.
				fire = 0.0f;
				
				// Stop the Fire indicator as well.
				enemyAffected.healthDisplay.ShowFire(false);
			}

			if (OnFire && tickTime <= 0f)
			{
				numberOfTicks++;
				//Debug.Log("Number of times fire has ticked me: " + numberOfTicks.ToString ());
				
				// Deal the damage, but then wait until tickTime before dealing it again.
				enemyAffected.Damage( fireTickDamage );
				
				// Instead of yielding, we'll reset our tickTime so we do not enter this if statement until tickTime is reached again.
				tickTime = 1.0f;		// Again, if one second is not standard, we'll need another variable here.
			}
		}
	}
}