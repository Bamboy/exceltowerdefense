using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Excelsion.Inventory;
using Excelsion.Towers.Projectiles;
using Excelsion.GameManagers;
using Excelsion.Enemies;

//Stephan Ennen - 3/4/2015

namespace Excelsion.Towers
{
	//All towers have this. Manages items, UI, and manages a TurretBase component depending on items.
	[RequireComponent(typeof(Collider))]
	public class TowerBase : MonoBehaviour 
	{
		private bool DO_DEBUG = true;
		public GameObject projectilePrefab;
		public Turret turret;
		public Bag inventory;
		public TowerStats stats;
		public List< Enemy > targets; //Switch this to internal later...
		public Enemy activeTarget;


		private bool CanAttack{ get{ return (stats.speed > 0.0f && activeTarget != null); } } //Disable attacking if speed is zero or less.
		public Vector3 targetPos; //Todo - add actual gameobject that is being targeted.
		private float cooldown = 0.0f; //Firing speed timer.
		private int tick;
		void Start () 
		{
			targets = new List< Enemy >();
			inventory = new Bag( 0 ); //Start with an empty inventory because at the start of a game we won't have a working tower.
			stats = new TowerStats();
			//stats.speed = 0.0f;
		}

		void Update () 
		{
			for( int i = 0; i < inventory.contents.Length; i++ )
			{
				if( inventory.contents[i] == null )
					continue;
				else
				{
					inventory.contents[i].OnTowerUpdate(); //TODO - sort by priority.
				}
			}

			TargettingUpdate();

			cooldown -= Time.deltaTime;
			if( CanAttack && cooldown <= 0.0f )
			{
				CreateProjectile();
			}
			tick++;
		}
		#region Targetting
		void TargettingUpdate()
		{
			if( activeTarget != null )
			{
				if( tick % 4 == 0 )
					FilterTargetsInRange();
				if( tick % 2 == 0 || targets.Count > 1 )
				{
					activeTarget = FilterTargetPriority();
					return;
				}

				//We have a target. Make sure it stays valid.
				if(Vector3.Distance( transform.position, activeTarget.transform.position ) >= stats.range )
				{
					Debug.Log("Target went out of range!");
					activeTarget = null;
					FilterTargetsInRange();
					activeTarget = FilterTargetPriority();
				}

			}
			else
			{
				FilterTargetsInRange();
				if( targets.Count > 0 )
					activeTarget = FilterTargetPriority();
			}


		}
		//Search for enemies in range and add them to a personal array.
		void FilterTargetsInRange() //TODO - Call this when active target dies. (is set to null)
		{
			targets.Clear();
			//if( CanAttack == false ) 
			//	return; //Don't bother if we can't attack.

			foreach( Enemy e in DefenseController.Get().enemies )
			{
				if( e == null )
				{
					DefenseController.Get().enemies.Remove( e ); //This might cause errors?
					continue;
				}
				if( Vector3.Distance( transform.position, e.transform.position ) <= stats.range )
					targets.Add( e );
			}
		}

		public virtual Enemy FilterTargetPriority()
		{
			//By default, target closest to objective.
			float closest = Mathf.Infinity;
			Enemy returnEnemy = null;
			foreach( Enemy e in targets )
			{
				if( e == null )
				{
					DefenseController.Get().enemies.Remove( e ); //This might cause errors?
					continue;
				}
				//float distance = Vector3.Distance( DefenseController.Get().enemyObjective.transform.position, e.transform.position );
				float distance = Vector3.Distance( transform.position, e.transform.position );
				if( distance < closest )
				{
					closest = distance;
					returnEnemy = e;
				}
			}
			return returnEnemy;
		}

		#endregion

		//Call this when an item is added or removed from our inventory.
		void OnBagModified()
		{
			//TODO Reset stats to default then tell items to re-add their value modifiers.
		}

		//Called when the mouse is over our collider and is clicked. TODO Open GUI and stuff here.
		void OnMouseDown()
		{
			this.GetComponent<Renderer>().material.color = Color.blue;
		}










		void CreateProjectile()
		{
			//Tell items we are about to create a projectile.
			for( int i = 0; i < inventory.contents.Length; i++ )
			{
				if( inventory.contents[i] == null )
					continue;
				else
				{
					inventory.contents[i].OnPreProjectileCreated(); //TODO - sort execution order by priority.
				}
			}
			targetPos = activeTarget.transform.position;
			//Create projectile
			Vector3 head = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
			Vector3 direction = VectorExtras.Direction(head, targetPos);

			GameObject projObj = GameObject.Instantiate( projectilePrefab, 
			 						VectorExtras.OffsetPosInDirection( head, direction, 3.25f ), //Make sure the projectile doesnt hit the tower.
									Quaternion.LookRotation( direction, Vector3.up )) as GameObject;
			ProjectileBase projScript = projObj.GetComponent< ProjectileBase >();
			if( projScript == null )
			{ Debug.LogError("Prefab given does not have a ProjectileBase script attached!", this); Debug.Break(); }

			projScript.Initalize( this, activeTarget, stats.damage );



			//Give items access to the projectile so they can make changes to it.
			for( int j = 0; j < inventory.contents.Length; j++ )
			{
				if( inventory.contents[j] == null )
					continue;
				else
				{
					inventory.contents[j].OnProjectileCreated( projScript ); //TODO - sort execution order by priority.
				}
			}

			cooldown = stats.speed; //trigger our cooldown
		}




		void OnDrawGizmos()
		{
			if( DO_DEBUG )
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere( transform.position, new TowerStats().range );

				if( targets == null ) return;

				foreach( Enemy e in targets ) //This causes errors as enemies are destroyed. Ignore it or turn off debug.
				{
					if( e == activeTarget )
					{
						Gizmos.color = Color.white;
						Gizmos.DrawLine( transform.position, e.transform.position );
						continue;
					}
					Gizmos.color = Color.grey;
					Gizmos.DrawLine( transform.position, e.transform.position );
				}

			}
		}














	}
}