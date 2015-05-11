using System.Collections;
using UnityEngine;
using Excelsion.GameManagers;

// Matt McGrath 4/25/2015

// Game Manager handles setting up all our major game elements -- especially Controllers which SHOULD be initialized from the get-go.
// This will fix problems such as the Resource display showing us as having 0 resources until an Enemy first dies and ResourceController.Get() is called, for example.
// This is usually a class that ties all the elements of the game together and handles the game's logic, states, menus, loading, etc.
// I'm attaching this script to an empty object to my scene for testing purposes. For now, you don't have to.
public class GameManager : MonoBehaviour 
{
	#region Singleton
	private static GameManager gameManager;
	public static GameManager Get()
	{
		if (gameManager != null)
			return gameManager;
		else
		{
			GameObject obj = new GameObject("_GameManager");
			obj.tag = "GameController";
			gameManager = obj.AddComponent<GameManager>();
			
			return gameManager;
		}
	}
	#endregion

	#region Editor Fields
	// Choose what you want to Initialize when the game first begins, or just for whatever you need in your test scenes. Will only work if set BEFORE running scene.
	public bool InitializeDefenseController;
	public bool InitializeInventoryController;
	public bool InitializeResourceController;
	public bool InitializeStructureController;
	public bool InitializeTaskController;
	public bool IntializeVillagerController;
	public bool InitializeWorldClock;
	public bool InitializeNotifications;
	#endregion

	// TODO: Probably references to our Main Menu, Option Menu, etc here, for transitioning game states.

	void Awake()
	{
		gameManager = this;

		// Initialize our Controllers. Usually I see Initialize methods in Singletons called at game start-up, but Get will do the same'ish thing.
		if (InitializeDefenseController)
			DefenseController.Get();	
		if (InitializeInventoryController)
			InventoryController.Get();
		if (InitializeResourceController)
			ResourceController.Get();
		if (InitializeStructureController)
			StructureController.Get();
		if (InitializeTaskController)
			TaskController.Get();
		if (IntializeVillagerController)
			VillagerController.Get();
		if (InitializeWorldClock)
			WorldClock.Get();
		if (InitializeNotifications)
			NotificationLog.Get();
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Allows us to pause the world clock. This is here for testing at the moment. In the real game we'd want a REAL pause (with menu) to happen here.
		if (Input.GetKeyDown (KeyCode.P))
		{
			WorldClock.Pause = !WorldClock.Pause;
		}
	}
}
