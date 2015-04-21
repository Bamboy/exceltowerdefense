using UnityEngine;
using System.Collections;

//Stephan Ennen - 4/2/2015

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
		}
		#endregion

		/* wood - early resource material
		 * stone - mostly for early-mid game upgrades 
		 * metal - mostly late game upgrades - 2 stone -> 1 metal
		 * food - used to maintain population
		 * population - your lifeline
		*/


		//Keep these private from other scripts. Make it so other scripts use functions in order to get or change these values.
		private int numberOfWood  = 0;
		private int numberOfStone = 0;
		private int numberOfMetal = 0;
		private int numberOfFood  = 30;
		private int numberOfPopulation   = 5;

		// MATT - 4/21/2015: Temporary testing. This could be part of this Controller or the Resource classes (if we choose to keep them).
		// Numbers are obviously temporary bs and temporarily public for testing -- we [probably] don't want them that way in the end!
		public int maxNumberOfWood = 1000;
		public int maxNumberOfStone = 750;
		public int maxNumberOfMetal = 500;
		public int maxNumberOfFood = 1000;
		public int maxNumberOfPopulation = 100;


		void Start()
		{
			numberOfWood = 5;
			numberOfStone = 5;
			numberOfMetal = 3;
			numberOfFood = 30;
			numberOfPopulation = 5;
		}


		//Return true if resource isn't negative if we subtract 'amount' from it. Used if the player tries to buy something, for example.
		public bool CanAffordWood( int amount )
		{
			return (numberOfWood - amount >= 0) ? true : false;
		}





		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// Matt McGrath - 4/21/2015: Adding required tasked functionality + also experimenting with different approach to resources ///
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// 
		// Matt McGrath - 4/21/2015
		// Return the value of a resource type.
		public int ResourceAmount(ResourceType resourceType)
		{
			int resourceAmount;
			
			// Could avoid switch-case if change other parts of this class, but I'm trying to tweak as little as possible for now.
			switch (resourceType)
			{
			case ResourceType.Population:
				resourceAmount = numberOfPopulation;
				break;
			case ResourceType.Food:
				resourceAmount = numberOfFood;
				break;
			case ResourceType.Wood:
				resourceAmount = numberOfWood;
				break;
			case ResourceType.Stone:
				resourceAmount = numberOfStone;
				break;
			case ResourceType.Metal:
				resourceAmount = numberOfMetal;
				break;
				//TODO: Should have a better default case if we somehow manage to provide a value not in here.
			default:
				resourceAmount = numberOfWood;
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
				numberOfPopulation += amount;
				numberOfPopulation = Mathf.Clamp(numberOfPopulation, 0, maxNumberOfPopulation);
				break;
			case ResourceType.Food:
				numberOfFood += amount;
				numberOfFood = Mathf.Clamp(numberOfFood, 0, maxNumberOfFood);
				break;
			case ResourceType.Wood:
				numberOfWood += amount;
				numberOfWood = Mathf.Clamp(numberOfWood, 0, maxNumberOfWood);
				break;
			case ResourceType.Stone:
				numberOfStone += amount;
				numberOfStone = Mathf.Clamp(numberOfStone, 0, maxNumberOfStone);
				break;
			case ResourceType.Metal:
				numberOfMetal += amount;
				numberOfMetal = Mathf.Clamp(numberOfFood, 0, maxNumberOfMetal);
				break;
			//TODO: Should have a better default case if we somehow manage to provide a value not in here.
			default:
				numberOfFood += amount;
				numberOfFood = Mathf.Clamp(numberOfFood, 0, maxNumberOfFood);
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
				numberOfPopulation -= amount;
				numberOfPopulation = Mathf.Clamp(numberOfPopulation, 0, maxNumberOfPopulation);
				break;
			case ResourceType.Food:
				numberOfFood -= amount;
				numberOfFood = Mathf.Clamp(numberOfFood, 0, maxNumberOfFood);
				break;
			case ResourceType.Wood:
				numberOfWood -= amount;
				numberOfWood = Mathf.Clamp(numberOfWood, 0, maxNumberOfWood);
				break;
			case ResourceType.Stone:
				numberOfStone -= amount;
				numberOfStone = Mathf.Clamp(numberOfStone, 0, maxNumberOfStone);
				break;
			case ResourceType.Metal:
				numberOfMetal -= amount;
				numberOfMetal = Mathf.Clamp(numberOfFood, 0, maxNumberOfMetal);
				break;
			//TODO: Should have a better default case if we somehow manage to provide a value not in here.
			default:
				numberOfFood -= amount;
				numberOfFood = Mathf.Clamp(numberOfFood, 0, maxNumberOfFood);
				break;
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// End of Matt's possibly awful / buggy / game-breaking code                                                                ///
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////











		// So that player could get some Reward for finished Tasks
		public void GainReward (Reward reward)
		{
			this.numberOfWood += reward.wood;
			this.numberOfStone += reward.stone;
			this.numberOfMetal += reward.metal;
			this.numberOfFood += reward.food;
			if (reward.pop > 0)
			{
				VillagerController.Get().CreateNewVillagers(reward.pop - this.numberOfPopulation);
				this.numberOfPopulation += reward.pop;
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
	public int pop;
	
	public Reward (int wood, int stone, int metal, int food, int pop)
	{
		this.wood = wood;
		this.stone = stone;
		this.metal = metal;
		this.food = food;
		this.pop = pop;
	}
	
	public Reward (): this(0,0,0,0,0) {}
}