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
	public void SetFire(float duration )
	{
		if(OnFire)
		{
			// Add time to existing fire.
			fire += duration;
		}
		else
		{ 	
			// Start new fire.
			Debug.Log ("Starting new fire");
			fire = duration;
			//StartCoroutine("FireTick", 1.0f);
			Debug.Log ("After StartCoRoutine is called");		// This never gets called: Problem with StartCoroutine?
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
				fire = 0.0f; //Stop fire.
				StopCoroutine("FireTick");
				enemyAffected.healthDisplay.ShowFire(false);
			}
		}
	}
	IEnumerator FireTick(float tickTime)
	{
		yield return null;
		while( OnFire == true )
		{
			enemyAffected.Damage( fireTickDamage ); //Deal damage then wait 'tickTime'.
			yield return new WaitForSeconds( tickTime );
		}
		Debug.Log("Escaped the flames!", this);
	}
}