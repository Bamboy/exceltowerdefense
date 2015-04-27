using UnityEngine;
using System.Collections;
using Excelsion.GameManagers;

// Matt McGrath 4/25/2015

// Game Manager handles setting up all our major game elements -- especially Controllers which SHOULD be initialized from the get-go.
// This will fix problems such as the Resource display showing us as having 0 resources until an Enemy first dies and ResourceController.Get() is called, for example.
// This is usually a class that ties all the elements of the game together and handles the game's logic, states, menus, loading, etc.
// I'm attaching this script to an empty object to my scene for testing purposes. For now, you don't have to.
public class GameManager : MonoBehaviour 
{
	void Awake()
	{
		// Initialize our Controllers. Usually I see Initialize methods in Singletons called game start-up, but Get will do the same'ish thing.
		DefenseController.Get();				
		InventoryController.Get();
		ResourceController.Get();
		StructureController.Get();
		TaskController.Get();
		VillagerController.Get();
		WorldClock.Get();
		NotificationLog.Get();
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
