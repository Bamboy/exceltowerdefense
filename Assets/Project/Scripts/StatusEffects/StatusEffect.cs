using UnityEngine;
using System.Collections;
using Excelsion.Enemies;

// Types of Status Effects. This lets us check for them using understandable names rather than more complicated means.
public enum StatusEffectType
{
	Fire,
	Cold,
	Frost,
	Poison,
	Oil
}

// Matt McGrath - 4/18/2015
// VERY rough, quick draft for collecting the hard-coded effects handling (found in Enemy.cs) into here.
// Eventually this will probably be an abstract class, with each status effect being its own class with its own unique methods.
// So far I gather each Effect will have a Duration, as well as an update function.
public abstract class StatusEffect 
{
	// Let's internally set the enum type, in case we want to iterate through effects and do things depending on its type.
	// For example: Let's say we apply freeze to an enemy and in the design it calls for removing the fire effect. We simply
	// check the Enemy's status effect's list, see if its StatusEffectType == Fire, then remove that effect.
	public StatusEffectType EffectType
	{
		get { return statusEffectType; }
		protected set { statusEffectType = value; }			// We DON'T want the user to change the StatusEffectType.
	}
	protected StatusEffectType statusEffectType;

	public float effectDuration;							// NOT USED YET: Duration the StatusEffect lasts.
	public Enemy enemyAffected;							// The Enemy suffering with this StatusEffect.
	
	public bool IsEffectStackable = false;					// If we're already on fire (or etc), should we have two stacks of fire ticking against us?

	// ETC for each StatusEffectType -- UNTIL WE SEPARATE THEM INTO DERIVED CLASSES.

	// NOT USED YET
	public StatusEffect()
	{
	}
	
	protected StatusEffect(StatusEffectType effectType, float duration)
	{
		statusEffectType = effectType;
		effectDuration = duration;
	}

	// Override this so each child StatusEffect can be "Activated" (put onto the Enemy) with this method.
	public virtual void InflictStatusEffect()
	{
	}

	// Override this so each child StatusEffect can have its logic done uniquely to its requirements.
	// NOTE: This is essentially an Update function that should be called each frame.
	public virtual void EvaluateStatusEffect()
	{
	}
}
