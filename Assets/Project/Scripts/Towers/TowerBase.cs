using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Excelsion.Inventory;
using Excelsion.Towers.Projectiles;
using Excelsion.GameManagers;
using Excelsion.Enemies;

//Stephan Ennen - 3/7/2015

namespace Excelsion.Towers
{
	//All towers have this. Manages items, UI, and manages a TurretBase component depending on items.
	[RequireComponent(typeof(Collider))]
	public class TowerBase : MonoBehaviour 
	{
		public delegate void OnProjectileCreation( out ProjectileBase[] projectiles, TowerBase tower );

		public OnProjectileCreation onProjectileCreation;

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

		#region Statics
		public static TowerBase[] towers;
		static int nextTowerID = 0;
		static int selectedID = -1;
		private int myID;
		static void Register( TowerBase newTower )
		{
			if( towers == null )
			{
				towers = new TowerBase[1];
				towers[0] = newTower;
			}
			else
			{
				towers = ArrayTools.PushLast<TowerBase>( towers, newTower );
			}
			nextTowerID++;
		}
		public static TowerBase GetSelected()
		{
			if( selectedID != -1 )
			{
				Debug.Log( selectedID );
				return towers[selectedID];
			}
			else
			{
				return null;
			}
		}
		void OnLevelWasLoaded( int level )
		{
			towers = new TowerBase[0];
			nextTowerID = 0;
			selectedID = -1;
		}
		#endregion
		void Start () 
		{
			myID = nextTowerID;
			Register( this );

			targets = new List< Enemy >();
			inventory = new Bag( 6 ); //Start with an empty inventory because at the start of a game we won't have a working tower.

			//Note that multiple items of the same type is not intended here. (Except for ItemNull)
			//=======STARTING ITEMS======
			inventory.contents[0] = new ItemPaper();
			inventory.contents[1] = new ItemFireball();
			inventory.contents[2] = new ItemNull();
			//=======END ITEMS======
			Debug.LogError("Double click me to change initial tower items!", this);
			
			stats = new TowerStats();
			OnBagModified();
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
			if( inventory.contents.Length != 0 )
			{
				foreach( Item i in inventory.contents )
				{
					if( i == null )
					{
						Debug.LogWarning("Item somehow was null!", this);
						continue;
					}
					if( i as ItemNull != null )
						continue;

					newStats = newStats + i.Stats;
				}
			}
			stats = newStats;
		}


		//Call this when an item is added or removed from our inventory.
		void OnBagModified()
		{
			//TODO - Reset stats and delegates to default then tell items to re-add their value modifiers.
			CalculateStats(); //Note this also resets our stats.
			
			//Tell items to re-apply tower-based delegates.
			for( int i = 0; i < inventory.contents.Length; i++ )
			{
				if( inventory.contents[i] == null )
					continue;
				else
				{
					inventory.contents[i].OnTowerDelegates( this );
				}
			}
		}

		#endregion


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
			else //TODO make filtering and prioritizing delegates
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
				float distance = Vector3.Distance( transform.position, e.transform.position );//TODO make filtering and prioritizing delegate(s)
				if( distance < closest )
				{
					closest = distance;
					returnEnemy = e;
				}
			}
			return returnEnemy;
		}

		#endregion














		void CreateProjectile()
		{

			ProjectileBase[] projectiles = new ProjectileBase[0];
			if( onProjectileCreation != null )
				onProjectileCreation( out projectiles, this );
			else
				projectiles = DefaultProjectileCreation(); //No custom projectile creation specified. Do default.

			foreach( ProjectileBase proj_init in projectiles )
			{
				proj_init.Initalize( this, activeTarget, stats.damage );
			}

			//Give items access to the projectile so they can make changes to it.
			for( int j = 0; j < inventory.contents.Length; j++ )
			{
				if( inventory.contents[j] == null )
					continue;
				else
				{
					foreach( ProjectileBase proj_delegates in projectiles )
					{
						inventory.contents[j].OnProjectileDelegates( proj_delegates ); //TODO - sort execution order by priority.
					}
				}
			}

			cooldown = stats.speed; //trigger our cooldown
		}
		ProjectileBase[] DefaultProjectileCreation()
		{
			targetPos = (transform.position + CalculateInterceptCourse(activeTarget.transform.position,
			                                                           activeTarget.GetComponent<NavMeshAgent>().velocity,
			                                                           transform.position,
			                                                           projectilePrefab.GetComponent<ProjectileBase>().speed));
			
			Vector3 head = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
			Vector3 direction = VectorExtras.Direction(head, targetPos);
			
			//Create projectile
			GameObject projObj = GameObject.Instantiate( projectilePrefab, 
			                                            VectorExtras.OffsetPosInDirection( head, direction, 3.25f ), //Make sure the projectile doesnt hit the tower.
			                                            Quaternion.LookRotation( direction, Vector3.up )) as GameObject;
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

		#region Interaction
		//Called when the mouse is over our collider and is clicked.
		void OnMouseDown()
		{
			this.GetComponent<Renderer>().material.color = Color.blue;
			selectedID = myID;
		}



		#endregion







	}
}