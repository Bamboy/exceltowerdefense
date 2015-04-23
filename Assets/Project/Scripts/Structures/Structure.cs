using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Excelsion.Tasks;
using Excelsion.UI;
using Excelsion.GameManagers;

// Types of Structures we'll be dealing with.
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

// A Structure is non-Tower building...? I guess...Not much information on them provided.
// They do things and stuff, apparently.
// TODO: We'll make this abstract and a base class later. For now let's make it behave like specific Structure.
[RequireComponent(typeof(Collider))]						// We require a Collider for ISelectable 
public abstract class Structure : MonoBehaviour, ISelectable
{
	#region Fields

	public StructureType StructureType
	{
		get { return structureType; }
		protected set { structureType = value; }
	}
	protected StructureType structureType;

	public string Name;
	public float Age;
	public Sprite Icon;
	public int Level;

	public string[] names = new string[]{ "Bryan", "Sergey", "Tristan", "Stephan", "Bryan", "Dann", "David", "Imran", "Jake", "Jessin", "Matt", "Jimmy", "Joshua" };
	
	[SerializeField]
	protected StructureController structureController;

	// Construction-related fields.
	float constructionTime;				// How long (real-life time? in-game hours? no clue...) to construct the Structure.
	Resource[] resourcesNeeded;			// Which Resource types do we require to construct this?
	int[] resourcesAmountNeeded;		// And how many of each? (Ensure index of both arrays match).

	// Possible stuff needed?
	// int villagersRequired;			// Amount of villagers it takes to construct this.
	// Villager[] villagers;			// The villager(s) assigned to the construction.
	#endregion

	void Awake() 
	{
		structureController = StructureController.Get();	// Gives us a reference to StructureController (also creates it if it doesn't exist yet).
		structureController.SubscribeStructure(this); 		// To add Structure into StructureController, to be managed there.

		Name = "House of " + names[Random.Range(0, names.Length)];
		StructureType = StructureType.House;
	}

	public virtual void Update()
	{
		// Do this for now. We'll want to use Days (or hours) through the WorldClock functionality later.
		Age += Time.deltaTime;

	}
	
	// Places the Structure at the given location.
	// TODO: Instead of a Vector3, we'll probably need a "StructureZone" type object, since we can only build in pre-defined areas.
	// A StructureZone will tell us the positions where we can build what type of Structure.
	public virtual void PlaceAt(Vector3 pos)
	{
		transform.position = pos;
		// TODO: Set a "birth" age so we can calculate total age of building.
	}

	// Let the StructureController we are no longer managing this Structure.
	public virtual void OnDestroy()
	{
		structureController.UnSubscribeStructure(this);
	}


	#region Selection
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