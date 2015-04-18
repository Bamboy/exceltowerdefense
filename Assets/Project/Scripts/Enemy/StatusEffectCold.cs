using UnityEngine;
using System.Collections;
using Excelsion.Enemies;

// Matt McGrath - 4/18/2015

// Sloppy derived class of StatusEffect. Will make StatusEffect abstract and use virtual methods later.
public class StatusEffectCold : StatusEffect 
{
	public float cold = 0.0f;								// This acts as our timer.
	public bool IsCold{ get{ return cold > 0.0f; } }		// Return true if our timer is active.
	public float coldMovementModifier = 0.50f; 				// Percentage at which effected enemies will move at. (50% normal speed, here)

	public StatusEffectCold(Enemy targetEnemy, float duration)
	{
		enemyAffected = targetEnemy;
		effectDuration = duration;

		statusEffectType = StatusEffectType.Cold;
	}

	public override void EvaluateStatusEffect ()
	{
		EvaluateCold();
	}

	// We can use this Constructor if we create an instance of StatusEffectFire and pass it our desired duration.
	public void SetCold()
	{
		SetCold(effectDuration);
	}

	public void SetCold(float duration )
	{
		if( IsCold )
			cold += duration;
		else
		{   // Start new cold
			enemyAffected.Speed = coldMovementModifier * 2f;//enemyAffected.defaultSpeed;
			cold = duration;
			enemyAffected.healthDisplay.ShowCold(true);
		}
	}

	void EvaluateCold()
	{
		if( IsCold )
		{
			cold -= Time.deltaTime;
			if( IsCold == false ) //End cold.
			{
				cold = 0.0f;
				enemyAffected.Speed = 2f;//enemyAffected.defaultSpeed; //Restore our normal speed.
				enemyAffected.healthDisplay.ShowCold(false);
			}
		}
	}
}
