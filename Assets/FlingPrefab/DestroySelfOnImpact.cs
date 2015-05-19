using System.Collections;
using UnityEngine;

//  Asai - 5/18/2015. Converted to C# and commented by Matt McGrath.

// Script which, when attached to an object, will Destroy the object when it takes part in any type of collision. In our case, this destroys it when it hits our enemies.
public class DestroySelfOnImpact : MonoBehaviour 
{

	void OnCollisionEnter(Collision col)
	{
		// For now, we're only attaching this to PlayerProjectiles, so this chek makes sure we won't destroy the player's projectiles when he launches them against his other projectiles.
		if (col.gameObject.tag != "PlayerProjectile")
		{
			// Simply destroy the object the script is attached to.
			Destroy(gameObject);
		}
	}
}
