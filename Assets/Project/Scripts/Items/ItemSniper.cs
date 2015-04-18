using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;
using Excelsion.Enemies;

//Matt McGrath - 4/18/2015

namespace Excelsion.Inventory
{
	// ItemSniperis a type of Item that will simply increase the range of the Tower that holds it. Possibly a real item eventually, but for now made for fun.
	public class ItemSniper : Item
	{
		#region ItemSpeed-specific variables
		// How much to increase the Range of the Tower by.
		public float rangeIncreaseFactor = 3f; 		// Let's make this high so it's obvious when a tower has it.
		#endregion
		
		private static Sprite spr;
		
		// Default constructor that will create the Sprite.
		public ItemSniper()
		{
			if( spr == null )
				spr = Sprite.Create(Resources.Load( "GUI/Items/Testing/item_sniper" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
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

				val.speed *= 0.50f;					// Let's make it half speed since we'll be doubling its range.
				val.range *= rangeIncreaseFactor;	// Let's double the range.
				val.damage = 0;						// Damage will not be affected.
				
				return val;
			}
		}
		
		// Return display name
		public override string Name{ get{ return "Range Increase For Tower!"; } }
		
		//TODO - Add model / effect changes
		
		// We do not need to override any of the base Item's Tower or Projectile functions.
	}
}