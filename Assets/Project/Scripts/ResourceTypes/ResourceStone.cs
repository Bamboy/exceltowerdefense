using UnityEngine;
using System.Collections;

// Matt McGrath - 4/21/2015

// Another type of Resource used for construction. 
// Stone is a mid-tier construction material, and less abundant than wood. 
// It can be gained from mining, mainly.
public class ResourceStone : Resource 
{
	public static float baseDropChance = 25f;

	// Override the abstract constructor and set temporary values for our fields.
	public ResourceStone()
	{
		// Stone is less abundant than  Wood; make enemies drop it less often. *TEMP NUMBERS*
		baseDropChance = 25f;
		
		// Internally flag what type of resource this is.
		ResourceType = ResourceType.Stone;
	}
}
