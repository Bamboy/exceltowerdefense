using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;
using Excelsion.Enemies;

using System.Collections.Generic;

namespace Excelsion.Inventory
{
	//Item that slows a tiny bit and puts a long DoT on the enemy
	public class ItemPoison : Item
	{
		private static Sprite spr;
		
		// Default Constructor; simply creates the Sprite for now.
		public ItemPoison()
		{
			if( spr == null )
				spr = Sprite.Create(Resources.Load( "GUI/Items/Testing/item_poison" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
		}
		// Return display icon location
		public override Sprite Icon{ get{ return spr; } }
		
		
		public override int Priority{ get{ return 0; } } 	// Lower values will be overwritten by higher values.
		public override int MutexBits{ get{ return 0; } } 	// Runs a bitwise AND operation to see if we can use this item alongside another item.
		public override TowerStats Stats
		{ 
			get
			{ 
				TowerStats val = new TowerStats();

				val.speed = 0f;
				val.range = 0f;
				

				// Possible TODO Maybe the Items should contain the StatusEffect(s) they cause.
				val.damage = -1;
				
				return val;
			}
		}
		
		//Return display name
		public override string Name{ get{ return "Poison Enemies!"; } }
		
		//TODO - Add model / effect changes
		
		#region Projectile Functions
		// Called after our projectile is created. Use this to pass delegates or other info to the projectile.
		public override void OnProjectileDelegates( ProjectileBase projectile )
		{
			projectile.onEnemiesHit += OnEnemiesHit;
		}
		
		// Give the enemies status effects or just do some damage.
		public override void OnEnemiesHit( Enemy[] enemies )
		{
			List<StatusEffectType> effectsBeingApplied = new List<StatusEffectType>();
			effectsBeingApplied.Add (StatusEffectType.Poison);
			List<StatusEffectType> effectsToIgnore = new List<StatusEffectType>();
			
			foreach( Enemy e in enemies )
			{
				// TESTING NOT STACKING EFFECTS:
				foreach (StatusEffect effect in e.statusEffects)
				{
					for (int i = 0; i < effectsBeingApplied.Count; i++)
					{
						if (effect.EffectType == effectsBeingApplied[i] && !effect.IsEffectStackable)
						{
							// Flag not to create, add, or set off this effect.
							effectsToIgnore.Add (effect.EffectType);
							
							//Debug.Log ("We're already suffering from " + effect.EffectType.ToString () + "!");
						}
					}
				}
				
				if (!effectsToIgnore.Contains(StatusEffectType.Poison))
				{
					StatusEffectPoison poisonEffect = new StatusEffectPoison(e, 7.0f);
					poisonEffect.poisonMovementModifier = 0.40f;
					poisonEffect.InflictStatusEffect();
					e.statusEffects.Add (poisonEffect);
				}
			}
		}
		#endregion
	}
}