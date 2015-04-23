using UnityEngine;
using System.Collections;

// Matt McGrath - 4/21/2015

// A type of Resource used to represent the population (life-line of the Player).
public class ResourcePopulation : Resource 
{
	public static float baseDropChance = 0f;

	// Override the abstract constructor and set temporary values for our fields.
	public ResourcePopulation()
	{
		// I doubt an enemy will be dropping the player any population.
		baseDropChance = 0f;
		
		// Internally flag what type of resource this is.
		ResourceType = ResourceType.Population;
	}
}
