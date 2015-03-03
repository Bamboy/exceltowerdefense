using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excelsion.Inventory;
using Excelsion.Towers.Projectiles;

//Stephan Ennen - 3/3/2015

namespace Excelsion.Towers
{
	//All towers have this. Manages items, UI, and manages a TurretBase component depending on items.
	[RequireComponent(typeof(Collider))]
	public class TowerBase : MonoBehaviour 
	{
		public GameObject projectilePrefab;
		public Turret turret;
		public Bag inventory;
		public TowerStats stats;

		private bool canAttack{ get{ return stats.speed > 0.0f; } } //Disable attacking if speed is zero or less.
		public Vector3 targetPos; //Todo - add actual gameobject that is being targeted.
		private float cooldown = 0.0f; //Firing speed timer.
		void Start () 
		{
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

			cooldown -= Time.deltaTime;
			if( canAttack && cooldown <= 0.0f )
			{
				CreateProjectile();
			}
		}

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

			//Create projectile
			Vector3 head = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
			Vector3 direction = VectorExtras.Direction(head, targetPos);

			GameObject projObj = GameObject.Instantiate( projectilePrefab, 
			 						VectorExtras.OffsetPosInDirection( head, direction, 3.25f ), //Make sure the projectile doesnt hit the tower.
									Quaternion.LookRotation( direction, Vector3.up )) as GameObject;
			ProjectileBase projScript = projObj.GetComponent< ProjectileBase >();
			if( projScript == null )
			{ Debug.LogError("Prefab given does not have a ProjectileBase script attached!", this); Debug.Break(); }

			projScript.Initalize( this, targetPos );

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



















	}
}