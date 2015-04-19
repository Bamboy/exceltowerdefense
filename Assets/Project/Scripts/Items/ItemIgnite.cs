using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;
using Excelsion.Enemies;

//Stephan Ennen - 3/17/2015
using System.Collections.Generic;

namespace Excelsion.Inventory
{
	//RIGHT NOW THIS CLASS IS ONLY A PLACEHOLDER FOR OTHER SCRIPTS.
	public class ItemIgnite : Item
	{
		private static Sprite spr;
		public ItemIgnite()
		{
			if( spr == null )
				spr = Sprite.Create(Resources.Load( "GUI/Items/Testing/apple" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
		}
		//Return display icon location
		public override Sprite Icon{ get{ return spr; } }


		public override int Priority{ get{ return 0; } } //Lower values will be overwritten by higher values.
		public override int MutexBits{ get{ return 0; } } //Runs a bitwise AND operation to see if we can use this item alongside another item.
		public override TowerStats Stats{ 
			get{ 
				TowerStats val = new TowerStats();
				val.speed = 1.0f / 3.0f;
				val.range = -2.0f;
				val.damage = 4;
				return val;
			}
		}
		
		//Return display name
		public override string Name{ get{ return "Set fire to things! :D"; } }

		//TODO - Add model / effect changes
		
		
		#region Tower Functions
		//Apply tower delegates here.
		/*public override void OnTowerDelegates( TowerBase tower )
		{
			return;
		}
		//Called in the tower's update loop. You could do things like auras or something here. NOT a delegate.
		public override void OnTowerUpdate()
		{
			return;
		}
		
		//Use this to create multiple projectiles.
		public override void OnProjectileCreation( out ProjectileBase[] projectiles, TowerBase tower )
		{
			projectiles = new ProjectileBase[0];
			return;
		}*/
		#endregion
		#region Projectile Functions
		//Called after our projectile is created. Use this to pass delegates or other info to the projectile.
		public override void OnProjectileDelegates( ProjectileBase projectile )
		{
			projectile.onEnemiesHit += OnEnemiesHit;
		}
		/*
		//Called in the projectile's update loop. Pass as a delegate in OnProjectileDelegates.
		public override void OnProjectileUpdate( ProjectileBase projectile )
		{
			return;
		}
		
		//Override how target(s) are selected. Only one of these functions is run.
		public override void EnemySelection( out Enemy[] enemies, ProjectileBase projectile )
		{
			enemies = new Enemy[0];
			return;
		} */
		
		//Give the enemies status effects or just do some damage.
		public override void OnEnemiesHit( Enemy[] enemies )
		{
			List<StatusEffectType> effectsBeingApplied = new List<StatusEffectType>();
			effectsBeingApplied.Add (StatusEffectType.Fire);
			effectsBeingApplied.Add (StatusEffectType.Cold);
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

				if (!effectsToIgnore.Contains(StatusEffectType.Fire))
				{
					StatusEffectFire fireEffect = new StatusEffectFire(e, 5.0f);
					fireEffect.InflictStatusEffect();
					e.statusEffects.Add (fireEffect);
				}
				if (!effectsToIgnore.Contains(StatusEffectType.Cold))
				{
					StatusEffectCold coldEffect = new StatusEffectCold(e, 5.0f);
					coldEffect.InflictStatusEffect();
					e.statusEffects.Add (coldEffect);
				}
			}
		}
		#endregion
		
		
	}
}