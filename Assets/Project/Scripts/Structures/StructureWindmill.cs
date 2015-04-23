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
		get { return windmillRequirements; }
	}
	
	private GameResources[] windmillRequirements;

	public float foodProductionRate = 1.0f / 10f;							// Example: One food produced every 10 seconds.

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

		// WorldClock.day starts off at 0 for a few seconds.
		day = WorldClock.day;

		GameResources levelOneResources = new GameResources(0, 5, 0, 0, 0);
		GameResources levelTwoResources = new GameResources(0, 10, 0, 0, 0);
		GameResources levelThreeResources = new GameResources(0, 20, 0, 0, 0);
		GameResources levelFourResources = new GameResources(0, 80, 1, 0, 0);
		windmillRequirements = new GameResources[4] { levelOneResources, levelTwoResources, levelThreeResources, levelFourResources };
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
				//Debug.Log ("In IsNewDay(): Windmill Day = " + day.ToString () + ", Clock's Day = " + WorldClock.day.ToString ());
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
		// We're already at max level.
		if (currentLevel >= windmillRequirements.Length)
			return false;

		// Grab a reference to all our resources.
		GameResources res = ResourceController.Get ().GetResources();

		if (res.Population < windmillRequirements[currentLevel].Population)
			return false;
		if (res.Food < windmillRequirements[currentLevel].Food)
			return false;
		if (res.Wood < windmillRequirements[currentLevel].Wood)
			return false;
		if (res.Stone < windmillRequirements[currentLevel].Stone)
			return false;
		if (res.Metal < windmillRequirements[currentLevel].Metal)
			return false;


		ResourceController.Get ().RemoveResource(ResourceType.Population,  windmillRequirements[currentLevel].Population);
		ResourceController.Get ().RemoveResource(ResourceType.Food,  windmillRequirements[currentLevel].Food);
		ResourceController.Get ().RemoveResource(ResourceType.Wood,  windmillRequirements[currentLevel].Wood);
		ResourceController.Get ().RemoveResource(ResourceType.Stone,  windmillRequirements[currentLevel].Stone);
		ResourceController.Get ().RemoveResource(ResourceType.Population,  windmillRequirements[currentLevel].Metal);

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