using UnityEngine;
using System.Collections;
using Excelsion.Enemies;


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
public class StatusEffect : MonoBehaviour 
{
	public StatusEffectType statusEffectType;
	public float effectDuration;									// NOT USED YET: Duration the StatusEffect lasts.
	public Enemy enemyAffected;								// The Enemy suffering with this StatusEffect.

	// ETC for each StatusEffectType -- UNTIL WE SEPARATE THEM INTO DERIVED CLASSES.

	// NOT USED YET
	public StatusEffect()
	{
	}

	public StatusEffect(StatusEffectType effectType, float duration)
	{
		statusEffectType = effectType;
		effectDuration = duration;
	}

	public virtual void EvaluateStatusEffect()
	{
	}
}
