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
	public float foodProductionRate = 1.0f / 10f;							// Example: One food produced every 10 seconds.

	// Won't have a Set because this will most likely be calculated internally based on level / # of  villagers working here.
	public float TimeUntilFoodProduction
	{
		get 
		{ 
			if (Level > 0)
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
	void Awake () 
	{
		structureController = StructureController.Get();	// Gives us a reference to StructureController (also creates it if it doesn't exist yet).
		structureController.SubscribeStructure(this); 		// To add Structure into StructureController, to be managed there.
		
		Name = "Windmill of " + names[Random.Range(0, names.Length)];
		StructureType = StructureType.Windmill;
		Icon = Sprite.Create(Resources.Load( "GUI/Structure Icons/Testing/structure_windmill" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
		Level = 0;

		timerHelper = TimeUntilFoodProduction;

		// WorldClock.day starts off at 0 for a few seconds.
		day = WorldClock.day;
		//Debug.Log ("InAwake(): Windmill Day = " + day.ToString () + ", Clock's Day = " + WorldClock.day.ToString ());
	}
	#endregion

	#region Structure / MonoBehavior Overrides
	public override void Update()
	{
		if (IsNewDay())
		{
			OnNewDay();
		}

		// Do this for now. We'll want to use Days (or hours) through the WorldClock functionality later.
		Age += Time.deltaTime;

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
		// Increase this Windmill's Level.
		Level++;
		NotificationLog.Get ().PushNotification(new Notification(Name + " reached Level " + Level.ToString() + "!", Color.green, 5.0f));
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