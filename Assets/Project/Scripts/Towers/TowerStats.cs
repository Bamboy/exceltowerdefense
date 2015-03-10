using UnityEngine;
using System.Collections;

//Stephan Ennen - 3/10/2015

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
			speed = 1.0f; //Fire a shot every 1 seconds.
			range = 50.0f; //Target any enemies within 50 units.
			damage = 5;  //5 damage on hit.
			luck = 0.01f; //10% chance to drop something useful.
		}

		//Defines what to do when two towerstat objects are added together.
		public static TowerStats operator +(TowerStats a, TowerStats b)
		{
			TowerStats c = a;
			c.speed = Mathf.Max(0.1f, a.speed + b.speed); //TODO - need a better way of combining speed. (Because two fast items equals a slow item)
			c.range = Mathf.Max(0.0f, a.range + b.range);
			c.damage = Mathf.Max(0, a.damage + b.damage);
			c.luck = Mathf.Clamp01(a.luck + b.luck);
			return c;
		}
	}
}