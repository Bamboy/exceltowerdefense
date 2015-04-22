using UnityEngine;
using System.Collections;

// Matt McGrath - 4/21/2015

// A type of Resource used for construction. It is the most basic of the construction resources.
public class ResourceWood : Resource 
{
	public static float baseDropChance = 100f;

	// Override the abstract constructor and set temporary values for our fields.
	public ResourceWood()
	{
		// Wood is the most abundant resource; make enemies drop it a lot. *TEMP NUMBERS*
		baseDropChance = 100f;				// Let's 100% it right now to test something.

		// Internally flag what type of resource this is.
		ResourceType = ResourceType.Wood;
	}
}
