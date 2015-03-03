using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;

//Stephan Ennen - 3/3/2015

namespace Excelsion.Inventory
{
	//RIGHT NOW THIS CLASS IS ONLY A PLACEHOLDER FOR OTHER SCRIPTS.
	public abstract class Item : System.Object
	{
		private int priority = 0; //Lower values will be overwritten by higher values.
		private int mutexbits = 0; //Runs a bitwise AND operation to see if we can use this item alongside another item.
		public int Priority{ get; set; }
		public int MutexBits{ get; set; }

		//Called when the item is given to the tower. Use this to modify stat changes.
		public virtual void OnApplied( TowerBase tower )
		{
			return;
		}
		//Called when the item is removed from the tower. Use this to modify stat changes.
		public virtual void OnRemoved( TowerBase tower )
		{
			return;
		}

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














	}
}