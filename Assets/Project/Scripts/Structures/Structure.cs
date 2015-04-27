using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Excelsion.GameManagers;
using Excelsion.Tasks;
using Excelsion.UI;

// Matt McGrath - 4/22/2015
// Types of Structures we'll be dealing with. * Don't "NEED" but keeping for now.
public enum StructureType
{
	House,					
	TownHall,
	Watermill,
	Windmill,
	Blacksmith,
	WoodCutter,
	StoneCutter,
	Farm,
	Church,
	Wall
}

// Matt McGrath - 4/22/2015

// A Structure is the abstract bass class for what are essentially buildings that villagers can be tasked to.
[RequireComponent(typeof(Collider))]			// We require a Collider for ISelectable 
public abstract class Structure : MonoBehaviour, ISelectable
{
	#region Fields
	// We need a Collider to this this ISelectable.
	protected BoxCollider boxCollider;

	// The type of Structure we're dealing with. Cannot set it outside of Structure classes!
	public StructureType StructureType
	{
		get { return structureType; }
		protected set { structureType = value; }
	}
	protected StructureType structureType;

	// Statistics-related fields.
	public string Name;												// Our building name.
	public int Age;													// Our building's age (in full days).
	public int Level;												// Our building's level. Higher means more tasks and features.
	public Sprite Icon;												// An icon for UI purposes. (Need?)

	// Construction-related fields.
	public abstract GameResources[] ResourceRequirements { get; }	// List of the resources and amounts of each we require to construct this. The index is for Level.
	protected int constructionTime;									// How many days to construct the Structure.
	protected bool isBeingBuilt = false;							// Is this currently being built?
	protected bool isBuilt = false;									// Are we finished being built and thus active?
	protected int dayBuilt;											// Keeps track of what Day this structure was built on.
	
	// For funsies random name of buildings.
	public string[] names = new string[]{ "Bryan", "Sergey", "Tristan", "Stephan", "Bryan", "Dann", "David", "Imran", "Jake", "Jessin", "Matt", "Jimmy", "Joshua" };

	[SerializeField]
	protected StructureController structureController;
	
	#endregion

	#region MonoBehaviors
	protected virtual void Awake() 
	{
	}

	protected virtual void Start()
	{
	}
	
	// Override & don't call base.Update() if you don't need this basic logic in one of the structure classes.
	public virtual void Update()
	{
	}

	// Let the StructureController know we are no longer managing this Structure.
	public virtual void OnDestroy()
	{
		if (structureController != null)
		{
			structureController.RemoveStructure(this);
		}
	}
	#endregion
	
	#region Structure Building and Upgrading Logic
	// Builds the Structure at the given location. A StructureZone will tell us the positions where we can build what type of Structure.
	public virtual void Build(StructureBuildZone buildZone, Quaternion rotation)
	{
//		Debug.Log("WTF");
//		transform.position = buildZone.transform.position;
//		transform.rotation = rotation;
//
//		// TODO: Set a "birth" age so we can calculate total age of building.
//
//		buildZone.isOccupied = true;
//
//		// TODO: Make a new windmill structure, not just grab one already existing in the scene.
//
//		// TESTING
////		GameObject obj = Instantiate(Resources.Load ("Prefabs/Structures/Windmill Structure") as GameObject);
//		//structurePrefab = Instantiate(Resources.Load ("Prefabs/Structures/Windmill Structure") as GameObject);
//		if (structurePrefab != null)
//			Debug.Log ("We're !NULL ! :(");
//		structurePrefab.gameObject.SetActive(true);
//		// END
//
////		GameObject obj = GameObject.Find("Windmill Structure");
////		obj.SetActive(true);
//
//		//TODO: It isn't placed in the center of the pad, rather top left.
//		structurePrefab.transform.position = transform.position;
//		structurePrefab.transform.rotation = transform.rotation;
	}

	// Check if player can upgrade the tower (if he has enough resources), BUT do not actually upgrade.
	public virtual bool CheckIfWeCanUpgrade(int currentLevel)
	{
		// Grab reference to our requirements.
		GameResources[] requirements = ResourceRequirements;
		
		// We're already at max level (or our requirements array was set up with too few levels).
		if (currentLevel > requirements.Length || currentLevel < 1)
			return false;
		
		// Grab a reference to all our resources.
		GameResources res = ResourceController.Get().GetResources();
		
		if (res.Population < requirements[currentLevel - 1].Population)
			return false;
		if (res.Food < requirements[currentLevel - 1].Food)
			return false;
		if (res.Wood < requirements[currentLevel - 1].Wood)
			return false;
		if (res.Stone < requirements[currentLevel - 1].Stone)
			return false;
		if (res.Metal < requirements[currentLevel - 1].Metal)
			return false;

		return true;
	}

	// Upgrade the structure, removing the required resources. * IMPORTANT: Ensure this is placed inside an if-statement for if CheckIfWeCanUpgrade is true.
	public virtual void Upgrade(int currentLevel, out string upgradeRequirementsString)
	{
		// Should this stuff be in ResourceController?
		ResourceController.Get().RemoveResources (ResourceRequirements[currentLevel - 1]);
		
		Level++;
//		NotificationLog.Get().PushNotification(new Notification(Name + " Upgraded to Level " + Level.ToString() + "!", Color.green, 5.0f));

		// We have no requirements for abstract Structure.
		upgradeRequirementsString = string.Empty;
	}
	#endregion

	#region UI Information
	// We will call this (From a UI Manager or StructureReader for now) to display structure-specific stats.
	protected virtual void DisplayStructureInformation()
	{
	}
	#endregion

	#region ISelection Stuff -- Should just be a Component you can add to an object IMO.
	[SerializeField] protected Transform selectTrans;
	public virtual Transform SelectionTransform
	{
		get{ return selectTrans; }
	}
	// This isn't implemented in SelectionController yet :(
	public virtual void OnDrawSelectedUI()
	{
		return; //See TowerReader.cs for Tower UI. TODO - Move that code here...?
	}
	#endregion
}