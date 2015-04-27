using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Excelsion.GameManagers;
using Excelsion.Tasks;
using Excelsion.UI;

// Matt McGrath - 4/23/2015, using Sergey Bedov's Villager code as a reference to maintain some consistency.

// A TownHill is a type of Structure that the player begins with.					
public class StructureTownhall : Structure
{
	#region Fields
	public override GameResources[] ResourceRequirements 
	{ 
		get { return townhallRequirements; }
	}
	
	private GameResources[] townhallRequirements;
	#endregion

	#region MonoBehavior Overrides
	protected override void Awake () 
	{
		structureController = StructureController.Get();	// Gives us a reference to StructureController (also creates it if it doesn't exist yet).
		//structureController.AddStructure(this); 		// To add Structure into StructureController, to be managed there.
		
		Name = "Village Townhall";
		StructureType = StructureType.TownHall;
		Icon = Sprite.Create(Resources.Load( "GUI/Structure Icons/Testing/structure_house" ) as Texture2D, new Rect(0,0,64,64), Vector2.zero, 100.0f);
	}

	protected override void Start()
	{
		base.Start ();
	}
	
	public override void Update()
	{
		base.Update();

		// Town Hall logic.
	}

	// Let the StructureController we are no longer managing this Structure.
	public override void OnDestroy()
	{
		base.OnDestroy();
	}
	#endregion
	
	#region Structure Building and Upgrading Logic
	// Builds the Structure at the given location. A StructureZone will tell us the positions where we can build what type of Structure.
	public override void Build(StructureBuildZone buildZone, Quaternion rotation)
	{
		base.Build (buildZone, rotation);

		// Townhill-specific stuff here.
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