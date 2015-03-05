using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excelsion.Enemies;

namespace Excelsion.GameManagers
{
	public class DefenseController : MonoBehaviour 
	{
		#region Access Instance Anywhere
		public static DefenseController defControl;
		public static DefenseController Get()
		{
			if( defControl != null )
				return defControl;
			else
			{
				GameObject obj = new GameObject("_DefenseController");
				obj.tag = "GameController";
				defControl = obj.AddComponent< DefenseController >();
				return defControl;
			}
		}
		#endregion
		public List< Enemy > enemies;

		//Target of which the enemies are trying to destroy.
		public GameObject enemyObjective; 

		public float spawnRadius = 175.0f;
		public float spawnHeight = 15.0f;
		public GameObject enemyPrefab;

		void Awake()
		{
			if( defControl == null )
				defControl = this;
			else
				GameObject.Destroy( this.gameObject );
		}
		void Start()
		{
			enemies = new List< Enemy >();
			enemyObjective = GameObject.FindGameObjectWithTag("Player") as GameObject;
			if( enemyObjective == null )
			{
				Debug.LogError("You need to tag an object as Player so the enemies have something to attack!");
			}
			StartCoroutine( "TimedSpawner" );
		}

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

		IEnumerator TimedSpawner()
		{
			yield return new WaitForSeconds( 5.0f );
			while( enemies.Count >= 10 )
				yield return null;

			GameObject obj = GameObject.Instantiate( enemyPrefab, GetSpawnPosition(), Quaternion.identity ) as GameObject;
			Enemy newEnemy = obj.GetComponent< Enemy >();
			enemies.Add( newEnemy );

			StartCoroutine( "TimedSpawner" ); //Repeat forever...
		}
		 
		//Gets a random position that is located on the edge of a circle.
		Vector3 GetSpawnPosition()
		{
			//Gets a random 2D direction
			Vector2 direction = Random.insideUnitCircle.normalized; 
			//Removes the y axis of our objective's position.
			Vector2 origin = new Vector2(enemyObjective.transform.position.x, enemyObjective.transform.position.z);
			//"Pushes" our origin position in direction by a certian distance.
			Vector2 pos = VectorExtras.OffsetPosInDirection( origin, direction, spawnRadius );
			//Converts back into Vector3, with the y axis being at a set height, then returns it.
			return new Vector3( pos.x, spawnHeight, pos.y );
		}










	}
}