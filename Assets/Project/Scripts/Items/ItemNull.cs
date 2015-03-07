using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;

//Stephan Ennen - 3/7/2015

namespace Excelsion.Inventory
{
	//NEVER DELETE THIS DEFINITION!!!
	public class ItemNull : Item
	{
		public ItemNull()
		{

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
				val.damage = 1;
				val.luck = 0f;
				return val;
			}
		}
		
		//Return display name
		public override string Name{ get{ return "Empty slot"; } }
		//Return display icon location
		public override string Icon{ get{ return "GUI/Items/Testing/null"; } }
		//TODO - Add model / effect changes
		
		/*
		//Called in the tower's update loop. You could do things like auras or independent projectiles here.
		public virtual void OnTowerUpdate()
		{
			return;
		}
		//Called before a projectile is created. Use this to create multiple projectiles..?
		public virtual void OnPreProjectileCreated()
		{
			return;
		}
		//Called after our projectile is created. Use this to pass delegates or other info to the projectile.
		public virtual void OnProjectileCreated( ProjectileBase projectile )
		{
			return;
		}
		//Called in the projectile's update loop. Pass as a delegate in OnProjectileCreated.
		public virtual void OnProjectileUpdate()
		{
			return;
		}
		//This is given to the projectile as a delegate.
		public virtual void OnProjectileImpact() //TODO - pass array of enemies hit here.
		{
			return;
		}
		
		*/
		
		
		
		
		
		
		
		
		
		
		
		
	}
}