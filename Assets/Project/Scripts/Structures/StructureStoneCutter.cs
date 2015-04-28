using UnityEngine.UI;
using Excelsion.GameManagers;
using Excelsion.Tasks;
using Excelsion.UI;
using UnityEngine;

// Matt McGrath - 4/28/2015

// A StoneCutter is a type of Structure which allows an assigned Villager(s) to mine for Stone resources.
public class StructureStoneCutter : Structure
{
	#region Fields
	public override GameResources[] ResourceRequirements 
	{ 
		// Returns (and creates each time -- bit odd design decision...) the amount of each Resource required to build this Structure at varying Levels.
		get 
		{
			GameResources levelTwoResources = new GameResources(0, 2, 0, 0, 0);				// 2 Wood to upgrade from Level 1 to Level 2.
			GameResources levelThreeResources = new GameResources(0, 4, 0, 0, 0);			// 4 Wood to upgrade from Level 1 to Level 2.
			
			return new GameResources[2] { levelTwoResources, levelThreeResources };
		}
	}
	
	// Amount of Wood produced each day. This is a base value.
	private int stoneMinedEachDay = 2;
	// Call this to get the actual Wood produced eachstoneMinedEachDaysome Level modifier to increase the amount per Level.
	public int StoneMinedPerDay
	{
		get { return stoneMinedEachDay * Level; }
	}
	
	#endregion
	
	#region MonoBehavior Overrides
	protected override void Awake () 
	{
		base.Awake ();
		
		Name = "Stone Cutter of " + names[Random.Range(0, names.Length)];
		StructureType = StructureType.StoneCutter;
		Icon = Sprite.Create(Resources.Load( "GUI/Structure Icons/Testing/structure_stonecutter" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
	}
	protected override void Start()
	{
		Level = 0;
		isBeingBuilt = false;
		constructionTime = 1;		// Two days to create a Windmill.
	}
	
	public override void Update()
	{
		if (isBeingBuilt || isBuilt)
			return;
		
		if (ConstructAutomatically)//if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			ConstructAutomatically = false;
			isBeingBuilt = true;
			WorldClock.onDusk += DuskTesting;
			
			// Add the WoodCutter to our Structure Controller.
			//StructureController.Get().AddStructure(this);
			
			// And place the Windmill.
			StructureController.Get().PlaceStructure (this);
		}
		
		// If we aren't built we need no further logic.
		if (isBuilt)
		{
			
		}
	}
	
	// Let the StructureController we are no longer managing this Structure.
	public override void OnDestroy()
	{
		// Remove our dusk delegate.
		WorldClock.onDusk -= DuskTesting;
		
		base.OnDestroy();
	}
	#endregion
	
	#region Structure Building and Upgrading Logic
	// Builds the Structure at the given location. A StructureZone will tell us the positions where we can build what type of Structure.
	public override void Build(StructureBuildZone buildZone, Quaternion rotation)
	{
		
	}
	
	// Upgrade the structure, removing the required resources. * IMPORTANT: Ensure this is placed inside an if-statement for if CheckIfWeCanUpgrade is true.
	public override void Upgrade(int currentLevel, out string upgradeRequirementsString)
	{
		base.Upgrade (currentLevel, out upgradeRequirementsString);
		
		upgradeRequirementsString = Name + " upgraded to Level " + (Level).ToString() + " using " + ResourceRequirements[currentLevel - 1].Wood.ToString() + " Wood!";
	}
	#endregion
	
	#region WorldClock Events
	private void DuskTesting()
	{
		isBeingBuilt = false;
		
		// If we aren't built, count down the days until we will be.
		if (!isBuilt)
		{
			this.constructionTime--;
			
			if (constructionTime == 0)
			{
				// Notify the player a structure has finished being built.
				NotificationLog.Get ().PushNotification(new Notification(Name + " has been constructed!", Color.green, 5.0f));
				isBuilt = true;
				Level = 1;
			}
			else
			{
				NotificationLog.Get().PushNotification(new Notification(Name + " requires " + constructionTime + " more days to be built.", Color.green, 5.0f));
			}
		}
		// Otherwise, we're already built, so we can start producting Food!
		else
		{
			// Give us the food we just harvested. We can call this even if we're not built yet because our Level (0) will give no food anyways.
			ResourceController.Get().AddResource(ResourceType.Stone, StoneMinedPerDay);
			
			// Notify the player (temporary probably).
			if (Level > 0)
				NotificationLog.Get().PushNotification(new Notification(Name + " produced " + StoneMinedPerDay.ToString () + " Stone!", Color.green, 5.0f));
			
			Age += 1;		// Might not even care about age, but let's keep it for now.
			OnNewDay();
		}
	}
	#endregion
	
	private void OnNewDay()
	{
		// Automatically attempt to upgrade each night, until we get our Pause phase and menus going.
		if (CheckIfWeCanUpgrade(Level))
		{
			//Upgrade(Level);
		}
		else
		{
			//NotificationLog.Get ().PushNotification(new Notification(Name + " doesn't meet resource requirement to level up :(", Color.green, 5.0f));
		}
	}
	
	#region UI Information
	// We will call this (From a UI Manager or StructureReader for now) to display structure-specific stats.
	protected override void DisplayStructureInformation()
	{
	}
	#endregion
	
	#region ISelection
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