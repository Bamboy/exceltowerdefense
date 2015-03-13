using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;
using Excelsion.Enemies;
using Excelsion.GameManagers;
using UnityEngine.EventSystems;
//Stephan Ennen - 3/10/2015

namespace Excelsion.Inventory
{
	public class ItemFireball : Item
	{
		private static Sprite spr;
		public ItemFireball()
		{
			if( spr == null )
				spr = Sprite.Create(Resources.Load( "GUI/Items/Testing/flame" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
		}
		
		//Lower values will be overwritten by higher values.
		public override int Priority{ get{ return 30; } } 
		//Runs a bitwise AND operation to see if we can use this item alongside another item.
		public override int MutexBits{ get{ return 0; } } //TODO fireball will need this.
		public override TowerStats Stats{ 
			get{ 
				TowerStats val = new TowerStats();
				val.speed = 1.5f;
				val.range = -6f;
				val.damage = 6;
				val.luck = 0.05f;
				return val;
			}
		}
		
		//Return display name
		public override string Name{ get{ return "Fireball"; } }
		//Return display icon location
		public override Sprite Icon{ get{ return spr; } }
		//TODO - Add model / effect changes
		
		/*
		//Called in the tower's update loop. You could do things like auras or independent projectiles here.
		public virtual void OnTowerUpdate()
		{
			return;
		}
		//Called before a projectile is created. Use this to create multiple projectiles..?
		public virtual void OnProjectileCreation( out ProjectileBase[] projectiles, TowerBase tower )
		{
			return;
		}
		*/


		//Called after our projectile is created. Use this to pass delegates or other info to the projectile.
		public override void OnProjectileDelegates( ProjectileBase projectile )
		{

			projectile.onEnemySelection += EnemySelection;
		}

		/*
		//Called in the projectile's update loop.
		public override void OnProjectileUpdate( ProjectileBase projectile )
		{
			return;
		} */

		//Override how target(s) are selected. Only one of these functions is run.
		public override void EnemySelection( out Enemy[] enemies, ProjectileBase projectile ) 
		{
			//SELECT ALL ENEMIES IN AN AOE.
			enemies = new Enemy[0];
			Color debugPaint = new Color( Random.value, Random.value, Random.value );
			foreach( Enemy e in DefenseController.Get().enemies )
			{
				if( e == null )
				{
					DefenseController.Get().enemies.Remove( e ); //This might cause errors?
					continue;
				}
				if( Vector3.Distance( projectile.transform.position, e.transform.position ) <= 10.0f )
				{
					enemies = ArrayTools.Push<Enemy>( enemies, e );
					e.GetComponent<Renderer>().material.color = debugPaint;
				}
			}

		}

		//Give the enemies status effects or just do some damage.
		//public override void OnEnemiesHit( Enemy[] enemies )
		//{
		//	return;
		//}
			
		
		
		
		
	}
}