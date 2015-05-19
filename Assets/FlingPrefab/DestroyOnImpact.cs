using System.Collections;
using UnityEngine;

// Coding Asai - 5/18/2015. Converted to C# and commented by Matt McGrath.

// Script which, when attached to an object, will check for collisions against Enemies, upon which it will Destroy the Enemy object.
public class DestroyOnImpact : MonoBehaviour 
{
	void OnCollisionEnter(Collision col)
	{
		// If the collision was with an Enemy tagged object, destroy it.
		if (col.gameObject.tag == "Enemy")
		{
			Destroy(col.gameObject);
		}
	}
}
