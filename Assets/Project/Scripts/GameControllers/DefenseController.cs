using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excelsion.Enemies;

//Stephan Ennen - 3/7/2015

namespace Excelsion.GameManagers
{
	public class DefenseController : MonoBehaviour 
	{
		#region Access Instance Anywhere
		private static DefenseController defControl;
		public static DefenseController Get()
		{
			if( defControl != null )
				return defControl;
			else
			{
				GameObject obj = new GameObject("_DefenseController");
				obj.tag = "GameController";
				defControl = obj.AddComponent< DefenseController >();

				// Let's child any Controller with a _Controllers object, creating it if it's not already present.
				if (GameObject.Find("_Controllers") == null) {new GameObject("_Controllers");}
				obj.transform.parent = GameObject.Find("_Controllers").transform;

				return defControl;
			}
		}
		#endregion

		#region Fields
		public List<Enemy> enemies;				// List of active enemies trying to attack us!
		public List<GameObject> houses;			// List of available houses.
		public int maxEnemies = 20;				// Maximum enemies alive at any given time.
		public GameObject enemyObjective; 		// Target to which the enemies are trying to destroy.
		public float spawnRadius = 175.0f;		// For knowing how far to spawn enemies.
		public float spawnHeight = 15.0f;		// For knowing how far to spawn enemies.
		public GameObject enemyPrefab;			// Reference to our Enemy prefab.
		public static int money;				// Money enemy drops upon death. Money doesn't seem to be used yet(?)	
		public float enemySpawnDelay = 3.0f;	// How often is a new Enemy spawned?
		#endregion

		#region Initialization
		void Awake()
		{
			if( defControl == null )
				defControl = this;
			else
				GameObject.Destroy( this.gameObject );
		}
		void Start()
		{
			enemies = new List<Enemy>();
			enemyObjective = GameObject.FindGameObjectWithTag("Player") as GameObject;
			if( enemyObjective == null )
			{
				Debug.LogError("You need to tag an object as Player so the enemies have something to attack!");
			}
			StartCoroutine( "TimedSpawner" );

			money = 500;
		}
		#endregion

		void OnDrawGizmos()
		{
			if( enemyObjective == null )
			{
				Debug.LogError("You need to set an objective for the enemies!", this);
				return;
			}
			Gizmos.color = Color.cyan;

			Gizmos.DrawWireSphere( enemyObjective.transform.position, spawnRadius );
		}

		//private Vector3 _lastTestPos;
		void Update()
		{
			//Vector3 thisTest = GetSpawnPosition();
			//Debug.DrawLine( _lastTestPos, thisTest, Color.green, 5.0f );
			//_lastTestPos = thisTest;
		}

		// This will repeat forever, spawning enemies every x amount of seconds. TODO: BUT only during the night!
		IEnumerator TimedSpawner()
		{
			yield return new WaitForSeconds(enemySpawnDelay);

			// Don't spawn more Enemies if we're not at night!
			while (WorldClock.isDaytime)
				yield return null;

			// Don't spawn more Enemies if we're at max count!
			while (enemies.Count >= maxEnemies)
				yield return null;

			// Otherwise, create a new enemy using the Enemy Prefab.
			GameObject obj = GameObject.Instantiate( enemyPrefab, GetSpawnPosition(), Quaternion.identity ) as GameObject;
			obj.transform.parent = this.transform;
			Enemy newEnemy = obj.GetComponent<Enemy>();
			if( newEnemy == null ) { Debug.LogError("Enemy prefab specified does not have an Enemy component!", this); Debug.Break(); }

			// Modify the max health of an enemy based on the day. For now, let's make them + 1 stronger each day -- no limits. //TODO: Health really should be a float, so we could do modifiers like Health += 1.20f.
			newEnemy.maxHealth = WorldClock.day + 1;
			newEnemy.health = newEnemy.maxHealth;

			// Add the enemy to our controller's list.
			enemies.Add( newEnemy );

			//Debug.Log("Enemy Spawned with Max Health " + newEnemy.maxHealth.ToString ());

			StartCoroutine( "TimedSpawner" ); 	//Repeat forever...
		}
		 
		// Gets a random position that is located on the edge of a circle.
		Vector3 GetSpawnPosition()
		{
			// Gets a random 2D direction.
			Vector2 direction = Random.insideUnitCircle.normalized; 
			// Removes the y axis of our objective's position.
			Vector2 origin = new Vector2(enemyObjective.transform.position.x, enemyObjective.transform.position.z);
			// "Pushes" our origin position in direction by a certian distance.
			Vector2 pos = VectorExtras.OffsetPosInDirection(origin, direction, spawnRadius);
			// Converts back into Vector3, with the y axis being at a set height, then returns it.

			return new Vector3(pos.x, spawnHeight, pos.y );
		}

		// Note from Matt McGrath - 4/28/2015: These should probably be done via the StructureController now. As a House is a type of Structure. These don't seem to be used anyways so I'll comment them out.
//		public void AddHouse(GameObject house) 
//		{
//			houses.Add(house);
//		}
//
//		public void RemoveHouse(GameObject house) 
//		{
//			houses.Remove(house);
//		}
	}
}