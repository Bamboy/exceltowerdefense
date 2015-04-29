using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Started by Stephan Ennen - 4/2/2015.

/*********************** ABOUT RESOURCES ***********************
* Wood  		- Early resource material
* Stone 		- Mostly for early-mid game upgrades 
* Metal 		- Mostly late game upgrades - 2 stone -> 1 metal
* Food  		- Used to maintain population
* Population 	- Your lifeline 
*****************************************************************/

// Matt McGrath - 4/21/2015
// An enum for the type of Resource we are looking at.
public enum ResourceType
{
	None,
	Population,
	Food,
	Wood,
	Stone,
	Metal
};

// Matt McGrath - 4/23/2014
// Structure to keep our Resources. "GameResources" prevents confusion with Unity's "Resources" definition.
public struct GameResources
{
	public int Population;
	public int Food;
	public int Wood;
	public int Stone;
	public int Metal;

	// Constructor simply sets up amount of each resource.
	public GameResources(int pop, int food, int wood, int stone, int metal)
	{
		Population = pop;
		Food = food;
		Wood = wood;
		Stone = stone;
		Metal = metal;
	}
};

namespace Excelsion.GameManagers
{
	// ResourceController is a manager-type class that will control and store our game's resources (wood, food, etc).
	public class ResourceController : MonoBehaviour 
	{
		#region Access Instance Anywhere
		private static ResourceController resControl;

		public static ResourceController Get()
		{
			if( resControl != null )
				return resControl;
			else
			{
				GameObject obj = new GameObject("_ResourceController");
				obj.tag = "GameController";
				resControl = obj.AddComponent< ResourceController >();

				// Let's child any Controller with a _Controllers object, creating it if it's not already present.
				if (GameObject.Find("_Controllers") == null) 
				{
					new GameObject("_Controllers");
				}
				obj.transform.parent = GameObject.Find("_Controllers").transform;

				return resControl;
			}
		}
		#endregion

		#region Fields
		// Matt 4/23 -- Use a structure instead of each a variable for tracking each resource count.
		// Keep this private from other scripts. Make it so other scripts use functions in order to get or change this structure's values.
		private GameResources gameResources = new GameResources(5, 30, 0, 0, 0);

		// Matt McGrath - 4/21/2015
		// Maximums for our resources. No need for special accessors if we're allowed to adjust them without restriction.
		public int maxNumberOfPopulation = 30;
		public int maxNumberOfFood = 1000;
		public int maxNumberOfWood = 1000;
		public int maxNumberOfStone = 750;
		public int maxNumberOfMetal = 500;

		// Matt McGrath - 4/24/2015
		// Chance that -- if an Enemy drops something -- it will be one of these. (0 = 0% chance, 1f = 100%)
		public float foodDropChance = 0.4f;
		public float woodDropChance = 0.3f;
		public float stoneDropChance = 0.2f;
		public float metalDropChance = 0.1f;

		// UI Reference -- Removed and made into a ResourceReader.
		#endregion

		#region Initialization
		// On Awake, set up our singleton instance if null. Otherwise, destroy the new instance attempt.
		void Awake() 
		{
			if( resControl == null )
				resControl = this;
			else
				GameObject.Destroy( this.gameObject );
		}

		// On Start, set up our GameResources structure to starting values.
		void Start()
		{
			// Create our GameResource reference and set up initial Population to 5 and Food to 30.
			gameResources = new GameResources(5, 20, 10, 5, 0);
		}
		#endregion

		#region Matt - Public Resource Amount / Get Methods
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// Matt McGrath - 4/21/2015: Adding required tasked functionality + also experimenting with different approach to resources ///
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		// Matt McGrath - 4/21/2015
		// Return the value of a specified resource type.
		public int ResourceAmount(ResourceType resourceType)
		{
			int resourceAmount;
			
			// Could avoid switch-case if change other parts of this class, but I'm trying to tweak as little as possible for now.
			switch (resourceType)
			{
			case ResourceType.Population:
				resourceAmount = gameResources.Population;
				break;
			case ResourceType.Food:
				resourceAmount = gameResources.Food;
				break;
			case ResourceType.Wood:
				resourceAmount = gameResources.Wood;
				break;
			case ResourceType.Stone:
				resourceAmount = gameResources.Stone;
				break;
			case ResourceType.Metal:
				resourceAmount = gameResources.Metal;
				break;
			default:
				Debug.Log ("Undefined ResourceType provided!");
				resourceAmount = 0;
				break;
			}
			
			return resourceAmount;
		}

		// MATT - 4/21/2015: Temporary testing using a more flexible method for CanAfford. Avoids needing one for each resource type.
		// Doesn't technically rely on new Resource classes, either.
		public bool CanAffordResource(ResourceType resourceType, int amount)
		{
			int amountOfResource = ResourceAmount(resourceType);
			return (amountOfResource - amount >= 0) ? true : false;
		}


		// Matt McGrath - 4/21/2015: 
		// Adds an amount of a given resource, keeping it within min and max bounds.
		public void AddResource(ResourceType resourceType, int amount)
		{
			// Could avoid switch-case if we change other parts of this class, but I'm trying not to to tweak any code you guys have already done...yet.
			switch (resourceType)
			{
			case ResourceType.Population:
				gameResources.Population += amount;
				gameResources.Population = Mathf.Clamp(gameResources.Population, 0, maxNumberOfPopulation);
				break;
			case ResourceType.Food:
				gameResources.Food += amount;
				gameResources.Food = Mathf.Clamp(gameResources.Food, 0, maxNumberOfFood);
				break;
			case ResourceType.Wood:
				gameResources.Wood += amount;
				gameResources.Wood = Mathf.Clamp(gameResources.Wood, 0, maxNumberOfWood);
				break;
			case ResourceType.Stone:
				gameResources.Stone += amount;
				gameResources.Stone = Mathf.Clamp(gameResources.Stone, 0, maxNumberOfStone);
				break;
			case ResourceType.Metal:
				gameResources.Metal += amount;
				gameResources.Metal = Mathf.Clamp(gameResources.Metal, 0, maxNumberOfMetal);
				break;
			default:
				Debug.Log ("Undefined ResourceType provided!");
				break;
			}
		}

		// Matt McGrath - 4/21/2015: 
		// Removes an amount of a given resource, keeping it within min and max bounds.
		// *** Could simply just have AddResource and pass in negative amounts instead of creating this. But maybe we'll do something differently after removing, so keep this here.
		public void RemoveResource(ResourceType resourceType, int amount)
		{
			// Could avoid switch-case if we change other parts of this class, but I'm trying not to to tweak any code you guys have already done...yet.
			switch (resourceType)
			{
			case ResourceType.Population:
				gameResources.Population -= amount;
				gameResources.Population = Mathf.Clamp(gameResources.Population, 0, maxNumberOfPopulation);
				break;
			case ResourceType.Food:
				gameResources.Food -= amount;
				gameResources.Food = Mathf.Clamp(gameResources.Food, 0, maxNumberOfFood);
				break;
			case ResourceType.Wood:
				gameResources.Wood -= amount;
				gameResources.Wood = Mathf.Clamp(gameResources.Wood, 0, maxNumberOfWood);
				break;
			case ResourceType.Stone:
				gameResources.Stone -= amount;
				gameResources.Stone = Mathf.Clamp(gameResources.Stone, 0, maxNumberOfStone);
				break;
			case ResourceType.Metal:
				gameResources.Metal -= amount;
				gameResources.Metal = Mathf.Clamp(gameResources.Metal, 0, maxNumberOfMetal);
				break;
			default:
				Debug.Log ("Undefined ResourceType provided!");
				break;
			}
		}

		// Matt McGrath - 4/21/2015
		// Returns the Maximum amount of this resource the player is allowed to have.
		public int MaximumAmountOfResource(ResourceType resourceType)
		{
			int maxAmount;

			// Could avoid switch-case if we change other parts of this class, but I'm trying not to to tweak any code you guys have already done...yet.
			switch (resourceType)
			{
			case ResourceType.Population:
				maxAmount =  maxNumberOfWood;
				break;
			case ResourceType.Food:
				maxAmount = maxNumberOfFood;
				break;
			case ResourceType.Wood:
				maxAmount =  maxNumberOfWood;
				break;
			case ResourceType.Stone:
				maxAmount =  maxNumberOfStone;
				break;
			case ResourceType.Metal:
				maxAmount =  maxNumberOfMetal;
				break;
			default:
				Debug.Log ("Undefined ResourceType provided!");
				maxAmount = 0;
				break;
			}

			return maxAmount;
		}

		public void SetMaximumAmountOfResource(ResourceType resourceType, int amount)
		{
			// Could avoid switch-case if we change other parts of this class, but I'm trying not to to tweak any code you guys have already done...yet.
			switch (resourceType)
			{
			case ResourceType.Population:
				maxNumberOfWood = amount;
				break;
			case ResourceType.Food:
				maxNumberOfFood = amount;
				break;
			case ResourceType.Wood:
				maxNumberOfWood = amount;
				break;
			case ResourceType.Stone:
				maxNumberOfStone = amount;
				break;
			case ResourceType.Metal:
				maxNumberOfMetal = amount;
				break;
			default:
				Debug.Log ("Undefined ResourceType provided!");
				break;
			}
		}

		// Matt - 4/22
		public GameResources GetResources()
		{	
			return gameResources;
		}

		// Matt McGrath - 4/24/2015: 
		// Removes an amount of all resources, keeping it within min and max bounds.
		public void RemoveResources(GameResources resourcesToRemove)
		{
			gameResources.Population -= resourcesToRemove.Population;
			Mathf.Clamp(gameResources.Population, 0, maxNumberOfPopulation);

			gameResources.Food -= resourcesToRemove.Food;
			Mathf.Clamp(gameResources.Food, 0, maxNumberOfFood);

			gameResources.Wood -= resourcesToRemove.Wood;
			Mathf.Clamp(gameResources.Wood, 0, maxNumberOfWood);

			gameResources.Stone -= resourcesToRemove.Stone;
			Mathf.Clamp(gameResources.Stone, 0, maxNumberOfStone);

			gameResources.Metal -= resourcesToRemove.Metal;
			Mathf.Clamp(gameResources.Metal, 0, maxNumberOfMetal);
		}

		#endregion

		#region Updates
		// MonoBehaviour's Update function. 
		void Update()
		{
			// No longer need to update UI in this class.
		}
		#endregion

		#region Sergey's Rewards
		// Sergey - So that player could get some Reward for finished Tasks
		public void GainReward (Reward reward)
		{
			AddResource(ResourceType.Wood, reward.wood);
			AddResource(ResourceType.Stone, reward.stone);
			AddResource(ResourceType.Metal, reward.metal);
			AddResource(ResourceType.Food, reward.food);

			if (reward.population > 0)
			{
				VillagerController.Get().CreateNewVillagers(reward.population - gameResources.Population);
				gameResources.Population += reward.population;
			}
		}
		#endregion
	}
}

#region Sergey's Serializable Reward class
// Sergey Bedov - 4/12/2015
[System.Serializable]
public class Reward
{
	public int wood;
	public int stone;
	public int metal;
	public int food;
	public int population;
	
	public Reward (int wood, int stone, int metal, int food, int pop)
	{
		this.wood = wood;
		this.stone = stone;
		this.metal = metal;
		this.food = food;
		this.population = pop;
	}
	
	public Reward (): this(0,0,0,0,0) {}
}
#endregion