using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;
using Excelsion.Enemies;
using UnityEngine.EventSystems;

//Stephan Ennen - 3/7/2015

namespace Excelsion.Inventory
{
	//RIGHT NOW THIS CLASS IS ONLY A PLACEHOLDER FOR OTHER SCRIPTS.
	public abstract class Item : System.Object
	{
		public abstract int Priority{ get; } //Lower values will be overwritten by higher values.
		public abstract int MutexBits{ get; } //Runs a bitwise AND operation to see if we can use this item alongside another item.
		public abstract TowerStats Stats{ get; }

		//Return display name
		public abstract string Name{ get; }
		//Return display icon location
		public abstract Sprite Icon{ get; }
		//TODO - Add model / effect changes


		#region Tower Functions
		//Apply tower delegates here.
		public virtual void OnTowerDelegates( TowerBase tower )
		{
			return;
		}
		//Called in the tower's update loop. You could do things like auras or something here. NOT a delegate.
		public virtual void OnTowerUpdate()
		{
			return;
		}

		//Use this to create multiple projectiles.
		public virtual void OnProjectileCreation( out ProjectileBase[] projectiles, TowerBase tower )
		{
			projectiles = new ProjectileBase[0];
			return;
		}
		#endregion
		#region Projectile Functions
		//Called after our projectile is created. Use this to pass delegates or other info to the projectile.
		public virtual void OnProjectileDelegates( ProjectileBase projectile ) //DONE
		{
			return;
		}

		//Called in the projectile's update loop. Pass as a delegate in OnProjectileDelegates.
		public virtual void OnProjectileUpdate( ProjectileBase projectile )
		{
			return;
		}

		//Override how target(s) are selected. Only one of these functions is run.
		public virtual void EnemySelection( out Enemy[] enemies, ProjectileBase projectile )
		{
			enemies = new Enemy[0];
			return;
		}

		//Give the enemies status effects or just do some damage.
		public virtual void OnEnemiesHit( Enemy[] enemies )
		{
			return;
		}
		#endregion


	}
}