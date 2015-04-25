using UnityEngine;
using System.Collections;
using Excelsion.GameManagers;
using Excelsion.UI;

// Matt McGrath - 4/25/2015

// A Build Zone is a place where we are able to set Structures.
public class StructureBuildZone : MonoBehaviour
{
	#region Fields
	public bool isOccupied = false;
	public bool isSelected = false;

	// Show the Building Zone? Only during construction phase & if not occupied already.
	public bool DisplayBuildingZone
	{
		get { return WorldClock.isDaytime && !isOccupied; }
	}

	// Reference to MeshRenderer so we can cache it.
	private MeshRenderer meshRenderer;

	// Reference to box collider (mostly for MouseOver or raycasting).
	private BoxCollider boxCollider;

	#endregion

	#region Initialization
	void Awake()
	{
		meshRenderer = this.GetComponent<MeshRenderer>();
		boxCollider = this.GetComponent<BoxCollider>();
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	#endregion

	#region Update
	// Update is called once per frame
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
}
