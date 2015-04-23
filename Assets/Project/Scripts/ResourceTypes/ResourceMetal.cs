using UnityEngine;
using System.Collections;

// Matt McGrath - 4/21/2015

// Another type of Resource used for construction. 
// Metal is a a top-tier construction material, and less abundant than even Stone. 
// Metal can only be obtained from a villager task that metls down two stone.
public class ResourceMetal : Resource 
{
	public static float baseDropChance = 10f;

	// Override the abstract constructor and set temporary values for our fields.
	public ResourceMetal()
	{
		// Metal is the most scarce resource; make enemies drop it rarely. *TEMP NUMBERS*
		baseDropChance = 10f;
		
		// Internally flag what type of resource this is.
		ResourceType = ResourceType.Metal;
	}
}
