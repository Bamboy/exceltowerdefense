using UnityEngine;
//using System.Collections;
//using Excelsion.Enemies;
//
//// Matt McGrath - 4/18/2015
//
//// Sloppy derived class of StatusEffect. Will make StatusEffect abstract and use virtual methods later.
//public class StatusEffectFrost : StatusEffect 
//{
//	public float frost = 0.0f;								// This acts as our timer.
//	public bool IsFrosted{ get{ return frost > 0.0f; } }	// Return true if our timer is active.
//	public float frostMovementModifier = 0.25f; 			// Percentage at which effected enemies will move at. (50% normal speed, here)
//
//	public StatusEffectFrost(Enemy targetEnemy, float duration)
//	{
//		enemyAffected = targetEnemy;
//		effectDuration = duration;
//		
//		statusEffectType = StatusEffectType.Frost;
//	}
//
//	public override void EvaluateStatusEffect ()
//	{
//		EvaluateFrost();
//	}
//
//	// We can use this Constructor if we create an instance of StatusEffectFire and pass it our desired duration.
//	public void SetFrost()
//	{
//		SetFrost(effectDuration);
//	}
//
//	public void SetFrost(float duration )
//	{
//		if( IsFrosted )
//			frost += duration;
//		else
//		{   // Start new cold
//			enemyAffected.Speed = frostMovementModifier * 2.0f;//enemyAffected.defaultSpeed;
//			frost = duration;
//			enemyAffected.healthDisplay.ShowCold(true);				// Will make a ShowFrost method later.
//		}
//	}
//	void EvaluateFrost()
//	{
//		if( IsFrosted )
//		{
//			frost -= Time.deltaTime;
//			if( IsFrosted == false ) //End cold.
//			{
//				frost = 0.0f;
//				enemyAffected.Speed = 2.0f;//enemyAffected.defaultSpeed; 	//Restore our normal speed.
//				enemyAffected.healthDisplay.ShowCold(false);		// Will make a ShowFrost method later.
//			}
//		}
//	}
//}