using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excelsion.Enemies;
using Excelsion.GameManagers;
using Excelsion.Inventory;
using Excelsion.Towers.Projectiles;
using Excelsion.UI;

// Matt McGrath - 5/09/2015

namespace Excelsion.Towers
{
	// The Player Tower is a tower the player actually controls, using an Angry Bird's, slingshot type shooting ability. It provides stronger damage than the other, manual towers.
	[RequireComponent(typeof(Collider))]
	public class PlayerTower : MonoBehaviour, ISelectable
	{
		public delegate void OnProjectileCreation(out ProjectileBase[] projectiles, PlayerTower tower);
		public OnProjectileCreation onProjectileCreation;
		
		private bool DO_DEBUG = true;
		public GameObject projectilePrefab;
		public Vector3 projectileSpawnOffset = Vector3.up;
		public float projectileSpawnDistance = 3f;
		public TowerStats stats;
		public List<Enemy> targets; //Switch this to internal later...
		public Enemy activeTarget;
		
		private bool CanAttack
		{ 
			get{ return (stats.speed > 0.0f && activeTarget != null); } 		//Disable attacking if speed is zero or less.
		} 
		public Vector3 targetPos; //Todo - add actual gameobject that is being targeted.
		private float cooldown = 0.0f; //Firing speed timer.
		private int tick;

		public static Transform projectileParentTransform;
		
		#region Statics
		public static PlayerTower playerTower;

		static void Register(PlayerTower newTower)
		{
			if( playerTower == null )
			{
				playerTower = new PlayerTower();
			}
		}
		
		void OnLevelWasLoaded(int level)
		{
//			towers = new TowerBase[0];
//			nextTowerID = 0;
		}
		#endregion
		
		void Awake()
		{
		}
		void Start () 
		{
			GameObject parentObj = GameObject.Find("Projectiles");
			if (parentObj == null)
			{
				Debug.Log("No Projectiles object found: Creating new one.");
				
				GameObject obj = new GameObject("Projectiles");
				projectileParentTransform = obj.transform;
			}
			else
			{
				projectileParentTransform = parentObj.transform;
			}

			Register(this);
			
			targets = new List<Enemy>();
		}

		void Update () 
		{


			CalculateStats(); //TODO - Don't update this every frame - only when our bag is changed.
			
			cooldown -= Time.deltaTime;
			if( CanAttack && cooldown <= 0.0f )
			{
				CreateProjectile();
			}
			tick++;
		}
		#region Inventory
		//Add up stats from our base stats and stats given by items. Call each time a modification is made to our bag.
		void CalculateStats() 
		{
			TowerStats newStats = new TowerStats(); //The default values here are equal to that of an empty (no items) tower.

			stats = newStats;
		}
		#endregion
		

		void CreateProjectile()
		{
			ProjectileBase[] projectiles = new ProjectileBase[0];
			if (onProjectileCreation != null)
			{
				// Execute each method in the delegate independently. 
				// We need to do this because otherwise the 'out' array gets overwritten by the other delegates contained. (We want to add)
				// Not doing this means that not all of our projectiles will be initialized correctly! (See 10 lines down)
				foreach (OnProjectileCreation invoke in onProjectileCreation.GetInvocationList())
				{
					ProjectileBase[] projectilesCreated = new ProjectileBase[0];
					invoke( out projectilesCreated, this );
					projectiles = ArrayTools.Concat<ProjectileBase>( projectiles, projectilesCreated );
				}
			}
			else
				projectiles = DefaultProjectileCreation(); //No custom projectile creation specified. Do default.
			
			foreach (ProjectileBase proj_init in projectiles)
			{
				//proj_init.Initalize(this, activeTarget, stats.damage);
			}

			cooldown = stats.coolDown;		// Trigger our cooldown.
		}
		ProjectileBase[] DefaultProjectileCreation()
		{
			targetPos = (transform.position + CalculateInterceptCourse(activeTarget.transform.position,
			                                                           activeTarget.GetComponent<NavMeshAgent>().velocity,
			                                                           transform.position,
			                                                           projectilePrefab.GetComponent<ProjectileBase>().speed));
			
			Vector3 head = transform.position + projectileSpawnOffset;
			Vector3 direction = VectorExtras.Direction(head, targetPos);
			
			//Create projectile
			GameObject projObj = GameObject.Instantiate( projectilePrefab, 
			                                            VectorExtras.OffsetPosInDirection( head, direction, projectileSpawnDistance ), //Make sure the projectile doesnt hit the tower.
			                                            Quaternion.LookRotation( direction, Vector3.up )) as GameObject;
			// MATT TESTING
			projObj.transform.parent = projectileParentTransform;
			// END MATT TESTING
			ProjectileBase projScript = projObj.GetComponent< ProjectileBase >();
			if( projScript == null )
			{ Debug.LogError("Prefab given does not have a ProjectileBase script attached!", this); Debug.Break(); }
			
			ProjectileBase[] val = new ProjectileBase[1];
			val[0] = projScript;
			return val;
		}
		
		#region Prediction
		public static Vector3 CalculateInterceptCourse(Vector3 aTargetPos, Vector3 aTargetSpeed, Vector3 aInterceptorPos, float aInterceptorSpeed)
		{
			Vector3 targetDir = aTargetPos - aInterceptorPos;
			
			float iSpeed2 = aInterceptorSpeed * aInterceptorSpeed;
			float tSpeed2 = aTargetSpeed.sqrMagnitude;
			float fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
			float targetDist2 = targetDir.sqrMagnitude;
			float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);
			if (d < 0.1f)  // negative == no possible course because the interceptor isn't fast enough
				return Vector3.zero;
			float sqrt = Mathf.Sqrt(d);
			float S1 = (-fDot1 - sqrt) / targetDist2;
			float S2 = (-fDot1 + sqrt) / targetDist2;
			
			if (S1 < 0.0001f)
			{
				if (S2 < 0.0001f)
					return Vector3.zero;
				else
					return (S2) * targetDir + aTargetSpeed;
			}
			else if (S2 < 0.0001f)
				return (S1) * targetDir + aTargetSpeed;
			else if (S1 < S2)
				return (S2) * targetDir + aTargetSpeed;
			else
				return (S1) * targetDir + aTargetSpeed;
		}
		#endregion
		void OnDrawGizmos()
		{
			if( DO_DEBUG )
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere( transform.position, new TowerStats().range );
				
				if( targets == null ) return;
				
				foreach (Enemy e in targets) //This causes errors as enemies are destroyed. Ignore it or turn off debug.
				{
					if (e == null)
						continue;
					else if (e == activeTarget)
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
		
		#region Selection
		
		[SerializeField] private Transform selectTrans;
		public Transform SelectionTransform
		{
			get{ return selectTrans; }
		}
		public void OnDrawSelectedUI()
		{
			return; //See TowerReader.cs for Tower UI. TODO - Move that code here...?
		}
		
		#endregion
	}
}
