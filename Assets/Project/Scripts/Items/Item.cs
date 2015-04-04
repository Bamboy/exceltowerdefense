using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;
using Excelsion.Enemies;
using UnityEngine.EventSystems;

//Stephan Ennen - 3/7/2015

namespace Excelsion.Inventory
{
	//Base class for all items and their functionality.
	public abstract class Item : System.Object
	{
		public abstract int Priority{ get; } //Lower values will be overwritten by higher values. NOT IMPLEMENTED - IGNORE.
		public abstract int MutexBits{ get; } //Runs a bitwise AND operation to see if we can use this item alongside another item. NOT IMPLEMENTED - IGNORE.
		public abstract TowerStats Stats{ get; } //Values here are added with the tower's base stats and other item's stats. There are min/max limits in place so go nuts.

		//Return display name NOT IMPLEMENTED.
		public abstract string Name{ get; }
		//Return display icon location for GUI. A sprite needs to actually be created here, so look at or copy some of the existing item definitions.
		public abstract Sprite Icon{ get; }

		//TODO - Add model / effect changes

		#region Tower Functions
		//Apply tower delegates here. Called every time the tower's inventory is changed. Use this to pass delegates or other info to the tower.
		public virtual void OnTowerDelegates( TowerBase tower )
		{
			return;
		}
		//Called in the tower's update loop. You could do things like auras or something here. NOT to be used as a delegate. [Called automagically by source tower]
		public virtual void OnTowerUpdate( TowerBase tower )
		{
			return;
		}

		//Use this to create your own projectile(s). You MUST add all projectiles created to the projectiles array, otherwise they wont work! [Add to tower's 'onProjectileCreation' delegate]
		public virtual void OnProjectileCreation( out ProjectileBase[] projectiles, TowerBase tower )
		{
			projectiles = new ProjectileBase[0];
			return;
		}
		#endregion
		#region Projectile Functions
		//Called after our projectile is created. Use this to pass delegates or other info to the new projectile.
		public virtual void OnProjectileDelegates( ProjectileBase projectile )
		{
			return;
		}

		//Called in the projectile's update loop. Note that a projectile with this won't move on its own! [Add to projectile's 'onUpdateEvent' delegate]
		public virtual void OnProjectileUpdate( ProjectileBase projectile )
		{
			return;
		}

		//Override how target(s) are selected before collision damage is dealt. An example of how to use this would be like getting all enemies within X range for an explosion. [Add to projectile's 'onEnemySelection' delegate]
		public virtual void EnemySelection( out Enemy[] enemies, ProjectileBase projectile ) 
		{
			enemies = new Enemy[0];
			return;
		}

		//Give the enemies status effects or just do some extra damage. Note that damage is always seperately dealt by ProjectileBase. [Add to projectile's 'onEnemiesHit' delegate]
		public virtual void OnEnemiesHit( Enemy[] enemies )
		{
			return;
		}
		#endregion


	}
}