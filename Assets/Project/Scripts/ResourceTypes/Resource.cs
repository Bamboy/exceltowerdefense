using UnityEngine;
using System.Collections;

// Matt McGrath - 4/21/2015
// *I don't like how the Resources seem built into the Controller so I'm going
// to make a Resource class and you guys can see what you think. A manager (controller) 
// should manage objects, not have the code for the internal workings of those objects.

// An enum for the type of Resource we are looking at.
public enum ResourceType
{
	Population,
	Food,
	Wood,
	Stone,
	Metal
}


// A Resource has purchasing power for various purposes.
// Resources will be managed by the ResourceController.
public abstract class Resource 
{
	// The enum for the type of Resource we're dealing with.
	public ResourceType ResourceType
	{
		get { return resourceType; }
		protected set { resourceType = value; }
	}
	private ResourceType resourceType;

	// Perchange (0f - 100f) this Resource may be dropped by an enemy.
	// We say this is a base chance because perhaps certain enemy types will modify this value (either directly or by adding their own modifier to it).
	// public static just for testing
	//public float baseDropChance = 0;


	public Resource()
	{
		// Internally set the type: Not necessary here because we can't create abstract Resource.

		//baseDropChance = 0f;
	}

	// TODO: LOTS! Need more info on Resources, how they'll tie into the task system (maybe we list or handle which tasks they're tied to here, I don't know).

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}