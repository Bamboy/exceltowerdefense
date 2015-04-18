using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;
using Excelsion.Enemies;

// Matt McGrath - 4/17/2015, using example Item class created by Stephan Ennen.

namespace Excelsion.Inventory
{
	// A Frost Item is one which slows enemies by quite a bit and makes them take slightly more damage.
	public class ItemFrost : Item
	{
		private static Sprite spr;

		// Default Constructor; simply creates the Sprite for now.
		public ItemFrost()
		{
			if( spr == null )
				spr = Sprite.Create(Resources.Load( "GUI/Items/Testing/item_frost" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
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

				// Let's assume we don't buff or weaken range or speed, for now.
				val.speed = 0f;
				val.range = 0f;
				val.damage = 1;

				return val;
			}
		}
		
		//Return display name
		public override string Name{ get{ return "Frost Enemies!"; } }
		
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
			foreach( Enemy e in enemies )
			{
				//e.SetCold(5.0f); 					// Slow the enemy for 5 seconds.
				//e.coldMovementModifier = 0.25f;	// Slow them down to 25% for now.
			
//				StatusEffectFrost frostEffect = new StatusEffectFrost(e, 5.0f);
//				frostEffect.frostMovementModifier = 0.25f;
//				e.statusEffects.Add(frostEffect);
//
//				frostEffect.SetFrost();
			}
		}
		#endregion
	}
}