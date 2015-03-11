using UnityEngine;
using System.Collections;
using Excelsion.Towers;
using Excelsion.Towers.Projectiles;
using Excelsion.Enemies;

//Stephan Ennen - 3/10/2015

namespace Excelsion.Inventory
{
	public class ItemPaper : Item
	{
		private static Sprite spr;
		public ItemPaper()
		{
			if( spr == null )
				spr = Sprite.Create(Resources.Load( "GUI/Items/Testing/paper" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
		}
		
		//Lower values will be overwritten by higher values.
		public override int Priority{ get{ return 3; } } 
		//Runs a bitwise AND operation to see if we can use this item alongside another item.
		public override int MutexBits{ get{ return 0; } }
		public override TowerStats Stats{ 
			get{ 
				TowerStats val = new TowerStats();
				val.speed = -0.8f;
				val.range = -2.5f;
				val.damage = 12;
				val.luck = 0f;
				return val;
			}
		}
		
		//Return display name
		public override string Name{ get{ return "Test Approval Sheet"; } }
		//Return display icon location
		public override Sprite Icon{ get{ return spr; } }
		//TODO - Add model / effect changes
		
		#region Tower Functions
		//Apply tower delegates here.
		public override void OnTowerDelegates( TowerBase tower )
		{
			tower.onProjectileCreation += OnProjectileCreation;
		}
		/*
		//Called in the tower's update loop. You could do things like auras or something here. NOT a delegate.
		public override void OnTowerUpdate()
		{
			return;
		} */
		
		//Use this to create multiple projectiles.
		public override void OnProjectileCreation( out ProjectileBase[] projectiles, TowerBase tower ) 
		{
			projectiles = new ProjectileBase[0]; // THIS IS EXTREMELY MESSY AND I APOLOGIZE! - Steph

			//Spawn 8 projectiles all going in random directions.
			for( int i = 0; i < 8; i++ ) //TODO make spread uniform instead of random.
			{
				#region spawn in random dir
				Vector2 direction = Random.insideUnitCircle.normalized; 
				Vector2 origin = new Vector2(tower.transform.position.x, tower.transform.position.z);
				//"Pushes" our origin position in direction by a certian distance.
				Vector2 pos = VectorExtras.OffsetPosInDirection( origin, direction, 1.0f );
				//Converts back into Vector3, with the y axis being at a set height, then returns it.
				Vector3 spawnDir = new Vector3( pos.x, tower.transform.position.y - 0.5f, pos.y );
				#endregion
				Vector3 travelDir = new Vector3( direction.x, -0.05f, direction.y ).normalized;
				//Create projectiles
				GameObject projObj = GameObject.Instantiate( tower.projectilePrefab, 
				                                            VectorExtras.OffsetPosInDirection( tower.transform.position, travelDir, 3.25f ), //Make sure the projectile doesnt hit the tower.
				                                            Quaternion.identity) as GameObject;
				ProjectileBase projScript = projObj.GetComponent< ProjectileBase >();
				if( projScript == null )
				{ Debug.LogError("Prefab given does not have a ProjectileBase script attached!", tower); Debug.Break(); }
				projScript.travelDir = travelDir;

				projectiles = ArrayTools.Push<ProjectileBase>( projectiles, projScript );
			}
		}

		#endregion
		#region Projectile Functions

		//Called after our projectile is created. Use this to pass delegates or other info to the projectile.
		public override void OnProjectileDelegates( ProjectileBase projectile )
		{
			projectile.onUpdateEvent += OnProjectileUpdate;
		}
		
		//Called in the projectile's update loop. Pass as a delegate in OnProjectileDelegates.
		public override void OnProjectileUpdate( ProjectileBase projectile )
		{
			//projectile.transform.Translate( 
				//VectorExtras.Direction( projectile.owner.transform.position, projectile.transform.position ).normalized * projectile.speed * Time.deltaTime );
			projectile.transform.Translate( projectile.travelDir * projectile.speed * Time.deltaTime );
		}
		/*
		//Override how target(s) are selected. Only one of these functions is run.
		public override void EnemySelection( out Enemy[] enemies, ProjectileBase projectile )
		{
			enemies = new Enemy[0];
			return;
		}
		
		//Give the enemies status effects or just do some damage.
		public override void OnEnemiesHit( Enemy[] enemies )
		{
			return;
		} */
		#endregion
		
		
		
		
		
		
		
		
		
		
		
	}
}