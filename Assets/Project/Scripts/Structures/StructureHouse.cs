using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Excelsion.GameManagers;
using Excelsion.Tasks;
using Excelsion.UI;

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

	#region MonoBehavior (and thus Structure) overrides
	protected override void Awake()
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
	#endregion

	#region Structure Building and Upgrading Logic
	// Builds the Structure at the given location. A StructureZone will tell us the positions where we can build what type of Structure.
	public override void Build(StructureBuildZone buildZone, Quaternion rotation)
	{
		base.Build (buildZone, rotation);
	}
	#endregion

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