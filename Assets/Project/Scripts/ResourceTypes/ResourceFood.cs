using UnityEngine;
using System.Collections;

// Matt McGrath - 4/21/2015

// A type of Resource used for villager consumption. 
// Villagers  consume this resource daily, and die without it.
public class ResourceFood : Resource 
{
	public static float baseDropChance = 50f;

	// Override the abstract constructor and set temporary values for our fields.
	public ResourceFood()
	{
		// Food is a vital resource; make enemies drop it quite often. *TEMP NUMBERS*
		baseDropChance = 50f;
		
		// Internally flag what type of resource this is.
		ResourceType = ResourceType.Food;
	}
}
