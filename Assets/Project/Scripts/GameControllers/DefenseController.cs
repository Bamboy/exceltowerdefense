using UnityEngine;
using System.Collections;

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

		//Target of which the enemies are trying to destroy.
		public GameObject enemyObjective; 

		public float spawnRadius = 175.0f;

		void Start()
		{
			enemyObjective = GameObject.FindGameObjectWithTag("Player") as GameObject;
			if( enemyObjective == null )
			{
				Debug.LogError("You need to tag an object as Player so the enemies have something to attack!");
			}
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
	}
}