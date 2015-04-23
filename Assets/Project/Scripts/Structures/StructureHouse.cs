using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Excelsion.Tasks;
using Excelsion.UI;
using Excelsion.GameManagers;

// Matt McGrath - 4/22/2015, using Sergey Bedov's Villager code as a reference to maintain some consistency.

// A House is a type of Structure that does...something...						
public class StructureHouse : Structure
{
	#region Fields
	public override GameResources[] ResourceRequirements 
	{ 
		get { return houseRequirements; }
	}

	private GameResources[] houseRequirements;

	#endregion
	
	protected override void Awake ()
	{
		base.Awake();
		
		Name = "House of " + names[Random.Range(0, names.Length)];
		StructureType = StructureType.House;
		Icon = Sprite.Create(Resources.Load( "GUI/Structure Icons/Testing/structure_house" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
	}
	
	public override void Update()
	{
		// Update our Age.
		base.Update ();

		// House logic.
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
	
	
	#region Selection
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