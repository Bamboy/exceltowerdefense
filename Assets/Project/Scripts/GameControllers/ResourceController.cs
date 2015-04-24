using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Stephan Ennen - 4/2/2015

// Matt McGrath - 4/23/2014
// Structure to keep our Resources. GameResources prevents confusion with Unity's Resources definition.
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
				return resControl;
			}
		}
		void Awake() 
		{
			if( resControl == null )
				resControl = this;
			else
				GameObject.Destroy( this.gameObject );

			// Cache references to our UI Text fields.
//			if (GameObject.Find ("Resource Info") == null)
//				return;

			GameObject pText = GameObject.Find ("Population Text");
			populationText = pText.GetComponent<Text>();
			
			GameObject fText = GameObject.Find ("Food Text");
			foodText = fText.GetComponent<Text>();
			
			GameObject wText = GameObject.Find ("Wood Text");
			woodText = wText.GetComponent<Text>();
			
			GameObject sText = GameObject.Find ("Stone Text");
			stoneText = sText.GetComponent<Text>();
			
			GameObject mText = GameObject.Find ("Metal Text");
			metalText = mText.GetComponent<Text>();
		}
		#endregion

		/* wood - early resource material
		 * stone - mostly for early-mid game upgrades 
		 * metal - mostly late game upgrades - 2 stone -> 1 metal
		 * food - used to maintain population
		 * population - your lifeline
		*/

		// Matt 4/23 -- Use a structure instead of each  resource variable separate?
		// Keep this private from other scripts. Make it so other scripts use functions in order to get or change this structure's values.
		private GameResources gameResources = new GameResources(5, 30, 0, 0, 0);


		// MATT - 4/21/2015: Temporary testing. This could be part of this Controller or the Resource classes (if we choose to keep them).
		// Numbers are obviously temporary bs and temporarily public for testing -- we [probably] don't want them that way in the end!
		private int maxNumberOfWood = 1000;
		private int maxNumberOfStone = 750;
		private int maxNumberOfMetal = 500;
		private int maxNumberOfFood = 1000;
		private int maxNumberOfPopulation = 100;

		// UI References - Should probably have a UI Manager where all this bloat gets put and cached.
		Text populationText;
		Text foodText;
		Text woodText;
		Text stoneText;
		Text metalText;

		void Start()
		{
			gameResources = new GameResources(5, 30, 0, 0, 0);
		}

		#region Matt - Testing Public Methods -- Might redo this approach depending on if we keep Resource enum and / or Resource classes.
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

		// MonoBehaviour's Update function. 
		void Update()
		{
			// For now, let's keep the Resource UI constantly updated. Ideally we'll only want to do this with an OnResourcesChanged method for efficiency.
			UpdateResourceUI();
		}

		public void UpdateResourceUI()
		{
			// Too expensive to call this each update. For now make sure you have Resource Info prefab under Screen Canvas
//			if (GameObject.Find ("Resource Info") == null)
//				return;

			populationText.text = "Population: " + ResourceAmount(ResourceType.Population);
			foodText.text = "Food: " + ResourceAmount(ResourceType.Food);
			woodText.text = "Wood: " + ResourceAmount(ResourceType.Wood);
			stoneText.text = "Stone: " + ResourceAmount(ResourceType.Stone);
			metalText.text = "Metal: " + ResourceAmount(ResourceType.Metal);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// End of Matt's possibly awful / buggy / game-breaking code                                                                ///
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



		// So that player could get some Reward for finished Tasks
		public void GainReward (Reward reward)
		{
			gameResources.Wood += reward.wood;
			gameResources.Stone += reward.stone;
			gameResources.Metal += reward.metal;
			gameResources.Food += reward.food;

			if (reward.population > 0)
			{
				VillagerController.Get().CreateNewVillagers(reward.population - gameResources.Population);
				gameResources.Population += reward.population;
			}
		}
	}
}

//Sergey Bedov - 4/12/2015
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