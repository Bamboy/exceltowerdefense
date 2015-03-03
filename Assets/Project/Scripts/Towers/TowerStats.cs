using UnityEngine;
using System.Collections;

//Stephan Ennen - 3/3/2015

namespace Excelsion.Towers
{
	public class TowerStats : System.Object
	{
		//Time in seconds between each shot.
		public float speed;
		//Distance in meters (units) the tower will target an enemy.
		public float range;
		//How much damage a projectile does.
		public int damage;
		//Percent chance that a shot that kills an enemy will cause the enemy to drop resources.
		public float luck;

		public TowerStats()
		{
			speed = 2.0f; //Fire a shot every 2 seconds.
			range = 30.0f; //Target any enemies within 30 units.
			damage = 3;  //3 damage on hit.
			luck = 0.10f; //10% chance.
		}
	}
}