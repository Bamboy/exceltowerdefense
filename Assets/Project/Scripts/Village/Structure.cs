using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Excelsion.Tasks;
using Excelsion.UI;

// Matt McGrath - 4/22/2015, using Sergey Bedov's Villager code as a reference to maintain some consistency.

// A Structure is non-Tower building...? I guess...Not much information on them provided.
// They do things and stuff, apparently.
// TODO: We'll make this abstract and a base class later. For now let's make it behave like specific Structure.
[RequireComponent(typeof(Collider))]
public class Structure : MonoBehaviour, ISelectable
{
	#region Fields
	// Using Sergey's Villager fields. I'm assuming a Structure wants these too.
	public string Name;
	public float Age;
	public Sprite Icon;
	
	[SerializeField]
	private StructureController structureController;

	// Construction-related fields.
	float constructionTime;				// How long (real-life time? in-game hours? no clue...) to construct the Structure.
	Resource[] resourcesNeeded;			// Which Resource types do we require to construct this?
	int[] resourcesAmountNeeded;		// And how many of each? (Ensure index of both arrays match).

	// Possible stuff needed?
	// int villagersRequired;			// Amount of villagers it takes to construct this.
	// Villager[] villagers;			// The villager(s) assigned to the construction.
	#endregion

	void Awake () 
	{
		structureController = StructureController.Get();	// Gives us a reference to StructureController (also creates it if it doesn't exist yet).
		structureController.SubscribeStructure(this); 		// To add Structure into StructureController, to be managed there.
	}
	
	// Places the Structure at the given location.
	// TODO: Instead of a Vector3, we'll probably need a "StructureZone" type object, since we can only build in pre-defined areas.
	// A StructureZone will tell us the positions where we can build what type of Structure.
	public void PlaceAt(Vector3 pos)
	{
		transform.position = pos;
	}

	// Let the StructureController we are no longer managing this Structure.
	void OnDestroy()
	{
		structureController.UnSubscribeStructure(this);
	}


	#region Selection
	[SerializeField] private Transform selectTrans;
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