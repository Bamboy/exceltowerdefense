using UnityEngine;
using System.Collections;
using Excelsion.GameManagers;
using System.Collections.Generic;

// Matt McGrath - 4/22/2015, using Sergey Bedov's VillagerController code as a reference to maintain some consistency.


// A Structure Controller manages all the structures (buildings) in the game world.
public class StructureController : MonoBehaviour
{
	#region Fields
	// A List of the Structure objects we are managing.
	public List<Structure> StructureList;

	// A list of all our possible build locations, as the GDD currently claims they are fixed.
	public List<StructureBuildZone> BuildLocations;
	#endregion

	#region Access Instance Anywhere
	private static StructureController structureController;
	public static StructureController Get()
	{
		if( structureController != null )
			return structureController;
		else
		{
			Debug.Log ("Creating Structure Controller Singleton");

			// We don't have a StructureController yet: Create one!
			GameObject obj = new GameObject("_StructureController");

			// Then, if we don't have a Controller in the hierarchy, create one and parent it to this controller.
			if (GameObject.Find("_Controllers") == null) {new GameObject("_Controllers");}
			obj.transform.parent = GameObject.Find("_Controllers").transform;
			//	obj.tag = "StructureController";
			// Our newly-created object will need this script as a component; attach it!
			structureController = obj.AddComponent< StructureController >();

			return structureController;
		}
	}
	#endregion

	#region Initialization
	void Awake ()
	{
		Debug.Log ("StructureController is Awake");
		if( structureController == null )
			structureController = this;
		else
			GameObject.Destroy( this.gameObject );

		// Initialize our Structure List.
		StructureList = new List<Structure>();

		// If we reach here, we must not have had this controller in our scene, and build locations weren't put in via inspector.
//		if (BuildLocations == null || BuildLocations.Count != 3)
//		{
//			BuildLocations = new List<StructureBuildZone>();
//
//			// We initialize our BuildLocations list through the editor.
//			// * NEVERMIND: Until we get an overall GameManager to ensure our Controllers are referenced from a global source, we must do this.
//			GameObject zonePrefab = Resources.Load ("Prefabs/Structures/Build Zone") as GameObject;
//			if (zonePrefab == null)
//				Debug.Log ("Build Zone Prefab is Null");
//
//			for (int i = 0; i < 3; i++)
//			{
//				Vector3 position = new Vector3((i - 1) * 50f, 0.1f, -60f);
//				StructureBuildZone buildZone = Instantiate(zonePrefab, position, transform.rotation) as StructureBuildZone;
//				if (buildZone == null)
//				{
//					Debug.Log ("Are we null here? " + i.ToString ());
//				}
//				BuildLocations.Add (buildZone);
//			}
//		}

	}
	#endregion

	#region Structure Adding and Removing
	// Add the specified Structure to our controller's list.
	public void AddStructure(Structure structureToAdd)
	{
		StructureList.Add (structureToAdd);
	}

	// Remove the specified Structure from our controller's list.
	public void RemoveStructure(Structure structureToRemove)
	{
		StructureList.Remove (structureToRemove);
	}
	#endregion
	
	#region Place Structure

	// Returns the first StructureBuildZone that is not currently occupied.
	// EDIT: Let's see if we're hovering our mouse over a potential build zone instead!
	private StructureBuildZone GetAvailableBuildZone()
	{
//		Debug.Log ("Get Available List Count: " + BuildLocations.Count.ToString ());
//
//		// Find a StructureBuildZone that is not Occupied.
//		foreach (StructureBuildZone zone in BuildLocations)
//		{
//			if (zone == null)
//				Debug.Log ("Zone is null :(");
//
//			if (!zone.isOccupied)
//			{
//				return zone;
//			}
//		}
//
//		// All them are occupied. What do we do?! Null for now.
//		return null;

		foreach (StructureBuildZone zone in BuildLocations)
		{
			if (zone == null)
				Debug.Log ("Zone is null :(");

			if (zone.isSelected)
			{
				Debug.Log ("This zone is selected.");

				if (!zone.isOccupied)
				{
					return zone;
				}
				else
				{
					Debug.Log ("...but it's occupied!");
				}
			}
		}

		return null;
	}

	public void PlaceStructure(Structure structureToPlace)
	{
		StructureBuildZone buildZone = GetAvailableBuildZone();

		if (buildZone != null)
		{
			structureToPlace.Build (buildZone, Quaternion.identity);

			string debugString = "Building " + structureToPlace.Name + " on day " + WorldClock.day.ToString();
			NotificationLog.Get ().PushNotification(new Notification(debugString, Color.green, 5.0f));
			Debug.Log (debugString);
			AddStructure (structureToPlace);
		}

		else
		{
//			string debugString = "Could not build structure here.";
//			NotificationLog.Get ().PushNotification(new Notification(debugString, Color.green, 5.0f));
//			Debug.Log (debugString);
		}
	}
	
	#endregion
}
