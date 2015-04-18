using UnityEngine;
using System.Collections;
using Excelsion.Enemies;

// Matt McGrath - 4/18/2015

// Sloppy derived class of StatusEffect. Will make StatusEffect abstract and use virtual methods later.
public class StatusEffectFire : StatusEffect 
{
	public float fire = 0.0f; 								// This acts as our timer.
	public bool OnFire{ get{ return fire > 0.0f; } } 		// Return true if our timer is active.
	public int fireTickDamage = 2; 							// Damage to do every second


	// Trying this to replace use of co-routine.
	private float tickTime = 0f;							// We'll apply damage-over-time, once every second (is 1 second standard? if not this will need to be public).
	private bool isBurning = false;
	private int numberOfTicks = 0;
	
	public StatusEffectFire(Enemy targetEnemy, float duration)
	{
		enemyAffected = targetEnemy;
		effectDuration = duration;

		statusEffectType = StatusEffectType.Cold;
	}

	public override void EvaluateStatusEffect ()
	{
		EvaluateFire();
	}

	// We can use this Constructor if we create an instance of StatusEffectFire and pass it our desired duration.
	public void SetFire()
	{
		Debug.Log ("Setting Fire to Enemy " + enemyAffected.ToString ());
		SetFire(effectDuration);
	}

	// We don't need the duration paramater really, but lets keep this until we work more on this.
	public void SetFire(float duration)
	{
		if(OnFire)
		{
			Debug.Log("We're on Fire and adding the fire duration");
			// Add time to existing fire.
			fire += duration;
		}
		else
		{ 	
			// Start new fire.
			Debug.Log ("Starting new fire");
			fire = duration;

			//  Attempting to remove co-routine and do logic in an update loop.
			//StartCoroutine("FireTick", 1.0f);


			enemyAffected.healthDisplay.ShowFire(true);
			Debug.Log("Enemy Effect = " + enemyAffected.ToString());
		}
	}
	
	void EvaluateFire()
	{
		if(OnFire)
		{
			fire -= Time.deltaTime;
			if( OnFire == false )
			{
				// We're no longer burning; stop the fire.
				fire = 0.0f;

				//StopCoroutine("FireTick");
				isBurning = false;				// Let's stop the process with a flag rather than stopping coroutine.

				// Stop the Fire indicator as well.
				enemyAffected.healthDisplay.ShowFire(false);
			}
		}



		// FireTick stuff.
		//yield return null;
		//while( OnFire == true )
		if (isBurning)
		{
			if (OnFire == true)
			{
				tickTime -= Time.deltaTime;
			}
			if (OnFire == true && tickTime <= 0f)
			{
				numberOfTicks++;
				Debug.Log("Time to Tick Damage: " + numberOfTicks.ToString ());
				// Deal the damage, but then wait until tickTime before dealing it again.
				enemyAffected.Damage( fireTickDamage );
				
				
				//yield return new WaitForSeconds( tickTime );
				// Instead of yielding, we'll reset our tickTime so we do not enter this if statement until tickTime is reached again.
				tickTime = 1.0f;		// Again, if one second is not standard, we'll need another variable here.
			}
			Debug.Log("Escaped the flames!", this);
		}
	}
}