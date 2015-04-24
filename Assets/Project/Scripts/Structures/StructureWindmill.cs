using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Excelsion.Tasks;
using Excelsion.UI;
using Excelsion.GameManagers;

// Matt McGrath - 4/22/2015, using Sergey Bedov's Villager code as a reference to maintain some consistency.

// A Windowmill, or "Farm", is a type of Structure which  ?????	I think produces Food?
public class StructureWindmill : Structure
{
	#region Fields
	public override GameResources[] ResourceRequirements 
	{ 
		// *** We will do it this way so others who potentially work on Structure children have an easier time doing this. More expensive on CPU if we call this a lot -- worth tradeoff of tiny bit of exra memory usage?
		get 
		{
			GameResources levelOneResources = new GameResources(0, 5, 0, 0, 0);
			GameResources levelTwoResources = new GameResources(0, 10, 0, 0, 0);
			GameResources levelThreeResources = new GameResources(0, 20, 0, 0, 0);
			GameResources levelFourResources = new GameResources(0, 80, 1, 0, 0);

			return new GameResources[4] { levelOneResources, levelTwoResources, levelThreeResources, levelFourResources };
		}
	}

	public float foodProductionRate = 1.0f / 10f;		// Example: One food produced every 10 seconds.

	// Won't have a Set because this will most likely be calculated internally based on level / # of  villagers working here.
	public float TimeUntilFoodProduction
	{
		get 
		{ 
			if (Level >= 0)
			{
				return timeUntilFoodProduction;// / (Level * 0.5f);
			}
			else return timeUntilFoodProduction; 
		}
		//set { value =  Mathf.Clamp(timerHelper, 0, timeUntilFoodProduction / (Level * 0.25f)); }			
	}
	private float timeUntilFoodProduction = 10f;

	public int FoodProducedEachHarvest
	{
		get { 
				return foodProducedEachHarvest * Level;
			}			
		//set { foodProducedEachHarvest = value; }
	}
	private int foodProducedEachHarvest = 1;

	private float timerHelper = 0f;
	#endregion

	#region Initialization
	protected override void Awake () 
	{
		base.Awake ();
	
		Name = "Windmill of " + names[Random.Range(0, names.Length)];
		StructureType = StructureType.Windmill;
		Icon = Sprite.Create(Resources.Load( "GUI/Structure Icons/Testing/structure_windmill" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
		Level = 0;

		timerHelper = TimeUntilFoodProduction;

		// WorldClock.day starts off at 0 for a few seconds, so this causes some issues.
		day = WorldClock.day;
	}
	#endregion

	#region Structure / MonoBehavior Overrides
	public override void Update()
	{
		if (IsNewDay())
		{
			Age += 1;
			OnNewDay();
		}
	
		timerHelper -= Time.deltaTime;
		if (timerHelper <= 0f)
		{
			// Reset the timer.
			timerHelper = TimeUntilFoodProduction;

			// Give us the food we just harvested.
			ResourceController.Get ().AddResource (ResourceType.Food, FoodProducedEachHarvest);

			// Notify the player (temporary probably).
			NotificationLog.Get ().PushNotification(new Notification(Name + " produced " + FoodProducedEachHarvest.ToString() + " Food!", Color.green, 5.0f));
		}
	}
	
	// Places the Structure at the given location.
	// TODO: Instead of a Vector3, we'll probably need a "StructureZone" type object, since we can only build in pre-defined areas.
	// A StructureZone will tell us the positions where we can build what type of Structure.
	public override void PlaceAt(Vector3 pos)
	{
		transform.position = pos;
		// TODO: Set a "birth" age so we can calculate total age of building.
	}
	
	// Let the StructureController we are no longer managing this Structure.
	public override void OnDestroy()
	{
		structureController.UnSubscribeStructure(this);
	}
	#endregion

	private int day;

	private bool IsNewDay()
	{
		if (WorldClock.day > 0)
		{
			// If this happens, our Day must have changed.
			if (WorldClock.day != day)
			{
				day = WorldClock.day;
				return true;
			}
		}

		return false;
	}

	private void OnNewDay()
	{
		if (Age > 0)
		NotificationLog.Get ().PushNotification(new Notification(Name + " reached Age " + Age.ToString() + "!", Color.green, 5.0f));

		if (CheckIfWeCanUpgrade(Level))
		{
//			Level++;
//			NotificationLog.Get ().PushNotification(new Notification(Name + " reached Level " + Level.ToString() + "!", Color.green, 5.0f));
		}
		else
		{
			//NotificationLog.Get ().PushNotification(new Notification(Name + " doesn't meet resource requirement to level up :(", Color.green, 5.0f));
		}
	}

	private bool CheckIfWeCanUpgrade(int currentLevel)
	{
		// Grab reference to our requirements.
		GameResources[] requirements = ResourceRequirements;

		// We're already at max level (or our requirements array was set up with too few levels).
		if (currentLevel >= requirements.Length)
			return false;

		// Grab a reference to all our resources.
		GameResources res = ResourceController.Get ().GetResources();

		if (res.Population < requirements[currentLevel].Population)
			return false;
		if (res.Food < requirements[currentLevel].Food)
			return false;
		if (res.Wood < requirements[currentLevel].Wood)
			return false;
		if (res.Stone < requirements[currentLevel].Stone)
			return false;
		if (res.Metal < requirements[currentLevel].Metal)
			return false;

		ResourceController.Get ().RemoveResources (requirements[currentLevel]);
//		ResourceController.Get ().RemoveResource(ResourceType.Population,  requirements[currentLevel].Population);
//		ResourceController.Get ().RemoveResource(ResourceType.Food,  requirements[currentLevel].Food);
//		ResourceController.Get ().RemoveResource(ResourceType.Wood,  requirements[currentLevel].Wood);
//		ResourceController.Get ().RemoveResource(ResourceType.Stone,  requirements[currentLevel].Stone);
//		ResourceController.Get ().RemoveResource(ResourceType.Population,  requirements[currentLevel].Metal);

		Level++;
		NotificationLog.Get ().PushNotification(new Notification(Name + " reached Level " + Level.ToString() + "!", Color.green, 5.0f));

		return true;

	}

	// TODO: Add specialized UI stuff for this type of structure. Example: Production rate, production amount.

	#region ISelection Stuff -- Should just be a Component you can add to an object IMO.
	//[SerializeField] private Transform selectTrans;
	public Transform SelectionTransform
	{
		get{ return selectTrans; }
	}
	// This isn't implemented in SelectionController yet :(
	public void OnDrawSelectedUI()
	{
		return; //See TowerReader.cs for Tower UI. TODO - Move that code here...?
	}
	#endregion
}