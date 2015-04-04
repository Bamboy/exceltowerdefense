using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;

//Stephan Ennen - 3/10/2015

namespace Excelsion.Inventory
{
	//NEVER DELETE THIS DEFINITION!!! 
	//It is used as a representation of a null item, so that actions can be performed upon it with less error checking required.
	public class ItemNull : Item
	{
		private static Sprite spr;
		public ItemNull()
		{
			if( spr == null )
				spr = Sprite.Create(Resources.Load( "GUI/Items/Testing/empty" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
		}

		//Lower values will be overwritten by higher values.
		public override int Priority{ get{ return 0; } } 
		//Runs a bitwise AND operation to see if we can use this item alongside another item.
		public override int MutexBits{ get{ return 0; } }
		public override TowerStats Stats{ 
			get{ 
				TowerStats val = new TowerStats();
				val.speed = 0f;
				val.range = 0f;
				val.damage = 0;
				return val;
			}
		}
		
		//Return display name
		public override string Name{ get{ return "Empty slot"; } }
		//Return display icon location
		public override Sprite Icon{ get{ return spr; } }
	}
}