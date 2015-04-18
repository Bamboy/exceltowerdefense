using UnityEngine;
using System.Collections;

//Stephan Ennen - 4/2/2015

namespace Excelsion.Towers
{
	public class TowerStats : System.Object
	{
		// Time in seconds between each shot.
		// MATT SUGGESTION TO-DO: Make Speed actually represent "Shots-per-second" rather than "time between each shot." 
		public float speed;
		// Distance in meters (units) the tower will target an enemy.
		public float range;
		//How much damage a projectile does.
		public int damage;

		// This can be used to calculate if the Tower is ready to fire agan. This way, Speed actually represents how often (speedy) a Tower shoots projectiles.
		// We'll keep this un-settable since it's just based on speed.
		public float coolDown
		{
			get 
			{
				if (speed != 0)
					return 1.0f / speed;
				else
					return float.MaxValue;
			}
		}

		public TowerStats()
		{
			speed = 1.0f / 1.75f; 	// Fire a shot every 1.75 seconds.
			range = 50.0f; 			// Target any enemies within 50 units.
			damage = 5;  			// 5 damage on hit.

			// Cooldown won't be set in default constructor. But it will be readily calculated when needed.
		}

		//Defines what to do when two towerstat objects are added together.
		public static TowerStats operator +(TowerStats a, TowerStats b)
		{
			TowerStats c = a;
			// Matt's suggestion: Now that Speed means "how fast per second," adding the two should provide an accurate outcome.
			c.speed = Mathf.Max(0.1f, a.speed + b.speed); //TODO - need a better way of combining speed. (Because two fast items equals a slow item)
			c.range = Mathf.Max(0.0f, a.range + b.range);
			c.damage = Mathf.Max(0, a.damage + b.damage);
			return c;
		}
	}
}