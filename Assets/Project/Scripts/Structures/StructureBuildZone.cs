using System.Collections;
using UnityEngine;
using Excelsion.GameManagers;
using Excelsion.UI;

// Matt McGrath - 4/25/2015

// A Build Zone is a place where we are able to set Structures.
public class StructureBuildZone : MonoBehaviour
{
	#region Fields

	public bool isOccupied = false;		// Is this build zone occupied already?
	public bool isSelected = false;		// Is this build zone "selected"? (Right now this means IsMouseEnter finds our mouse is over the zone)

	// Show the Building Zone? Only during construction phase & if not occupied already.
	public bool DisplayBuildingZone
	{
		get { return WorldClock.isDaytime && !isOccupied; }
	}

	// Reference to MeshRenderer so we can cache it. We'll need to change its color (possibly more) for selected & unselected states.
	private MeshRenderer meshRenderer;

	// Reference to box collider (mostly for MouseOver functionality or raycasting).
	private BoxCollider boxCollider;

	#endregion

	#region Initialization
	void Awake()
	{
		// Set and cache our mesh renderer and collider, as GetComponent is too expensive to keep recalling.
		meshRenderer = this.GetComponent<MeshRenderer>();
		boxCollider = this.GetComponent<BoxCollider>();
	}

	// Use this for initialization.
	void Start () 
	{
	}
	#endregion

	#region Update
	// Update is called once per frame.
	void Update () 
	{
		// TODO: Make this only change during certain times / events for efficiency.
		if (DisplayBuildingZone)
		{
			meshRenderer.enabled = true;
		}
		else
		{
			meshRenderer.enabled = false;
		}
	}
	#endregion

	#region Mouse (Selection) Events
	// When the mouse is over a building zone...
	void OnMouseEnter()
	{
		// Let's flag it as selected and color it blue.
		isSelected = true;
		meshRenderer.materials[0].color = Color.blue;
	}

	// When our mouse leaves that building zone...
	void OnMouseExit()
	{
		// Let's flag it as not selected and color it yellow.
		isSelected = false;
		meshRenderer.materials[0].color = Color.yellow;
	}
	#endregion
}
