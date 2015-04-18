using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;
using Excelsion.Enemies;

//Matt McGrath - 4/17/2015
// Using existing derived Item class from Stephan Ennen.

namespace Excelsion.Inventory
{
	// ItemSpeed is a type of Item that will simply speed up the projectiles of the Tower that holds it.
	public class ItemSpeed : Item
	{
		#region ItemSpeed-specific variables
		// How much to increase the speed of the Tower by.
		public float speedIncrease = 6f; 		// Let's make this high so it's obvious when a tower has it.
		#endregion

		private static Sprite spr;

		// Default constructor that will create the Sprite.
		public ItemSpeed()
		{
			if( spr == null )
				spr = Sprite.Create(Resources.Load( "GUI/Items/Testing/item_speed" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
		}

		// Return display icon location
		public override Sprite Icon{ get{ return spr; } }
		
		
		public override int Priority{ get{ return 0; } } //Lower values will be overwritten by higher values.
		public override int MutexBits{ get{ return 0; } } //Runs a bitwise AND operation to see if we can use this item alongside another item.
		public override TowerStats Stats
		{ 
			get
			{ 
				TowerStats val = new TowerStats();
			
				// We only want to modify the speed stat.
				val.speed = speedIncrease;
				val.range = 0f;
				val.damage = 0;

				return val;
			}
		}
		
		// Return display name
		public override string Name{ get{ return "Speed increase given to projectiles!"; } }
		
		//TODO - Add model / effect changes

		// We do not need to override any of the base Item's Tower or Projectile functions.
	}
}