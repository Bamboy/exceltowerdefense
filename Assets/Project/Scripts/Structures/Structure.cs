using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Excelsion.Tasks;
using Excelsion.UI;
using Excelsion.GameManagers;

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

// Matt McGrath - 4/22/2015, using Sergey Bedov's Villager code as a reference to maintain some consistency.

// A Structure is the abstract bass class for what are essentially buildings that villagers can be tasked to.
[RequireComponent(typeof(Collider))]							// We require a Collider for ISelectable 
public abstract class Structure : MonoBehaviour, ISelectable
{
	#region Fields

	protected BoxCollider boxCollider;

	public StructureType StructureType
	{
		get { return structureType; }
		protected set { structureType = value; }
	}
	protected StructureType structureType;

	public string Name;								// Our building name.
	public int Age;									// Our building's age (in full days).
	public int Level;								// Our building's level. Higher means more tasks and features.
	public Sprite Icon;								// An icon for UI purposes. (Need?)

	// Construction-related fields.
	protected int constructionTime;					// How many days to construct the Structure.
	protected bool isBeingBuilt = false;			// Is this currently being built?
	protected bool isBuilt = false;					// Are we finished being built and thus active?
	public abstract GameResources[] ResourceRequirements { get; }	// List of the resources and amounts of each we require to construct this. The index is for Level.
	protected int day;								// Keeps track of what day this structure is on. TODO use newly added WorldClock delegates instead.
	
	// For funsies.
	public string[] names = new string[]{ "Bryan", "Sergey", "Tristan", "Stephan", "Bryan", "Dann", "David", "Imran", "Jake", "Jessin", "Matt", "Jimmy", "Joshua" };

	[SerializeField]
	protected StructureController structureController;
	
	#endregion

	protected virtual void Awake() 
	{
	}

	protected virtual void Start()
	{
	}
	
	// Override & don't call base.Update() if you don't need this basic logic in one of the structure classes.
	public virtual void Update()
	{
		// On each new Day, we will increment our age.
//		if (IsNewDay())
//		{
//			Age += 1;
//		}
	}
	
	// Builds the Structure at the given location.
	// TODO: Instead of a Vector3, we'll probably need a "StructureZone" type object, since we can only build in pre-defined areas.
	// A StructureZone will tell us the positions where we can build what type of Structure.
	public virtual void Build(StructureBuildZone buildZone, Quaternion rotation)
	{
		transform.position = buildZone.transform.position;
		transform.rotation = rotation;

		// TODO: Set a "birth" age so we can calculate total age of building.

		buildZone.isOccupied = true;

		// TODO: Make a new windmill structure, not just grab one already existing in the scene.
		GameObject obj = GameObject.Find("Windmill Structure");
		obj.SetActive(true);

		//TODO: It isn't placed in the center of the pad, rather top left.
		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;
	}

	// Let the StructureController know we are no longer managing this Structure.
	public virtual void OnDestroy()
	{
		structureController.RemoveStructure(this);
	}

	#region ISelection
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