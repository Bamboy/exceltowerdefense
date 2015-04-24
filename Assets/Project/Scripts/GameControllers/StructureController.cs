using UnityEngine;
using System.Collections;

// Matt McGrath - 4/22/2015, using Sergey Bedov's VillagerController code as a reference to maintain some consistency.
public class StructureController : MonoBehaviour
{
	#region Fields
	// An array of the Structure objects we are managing.
	public Structure[] StructureList;
	#endregion

	#region Access Instance Anywhere
	private static StructureController structureController;
	public static StructureController Get()
	{
		if( structureController != null )
			return structureController;
		else
		{
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
		// Initialize our structure array with 0 elements.
		StructureList = new Structure[0];
	}
	#endregion
	
	#region Create Structure(s)
	public void CreateNewStructure(string name, int age, Sprite icon, Vector3 pos)
	{
		GameObject instance = Instantiate (Resources.Load ("Prefabs/Structures/StructureGO")) as GameObject;
		Structure structureInstance = instance.GetComponent<Structure>();
		structureInstance.Name = name;
		structureInstance.Age = age;
		structureInstance.Icon = icon;
		structureInstance.transform.position = pos;
		//GameObject newVillager = Instantiate(Resources.Load("Prefabs/VillagerSample", GameObject));
	}

	public void CreateNewStructure()
	{
		CreateNewStructure("Name Surname", 0, null, new Vector3(1000,2,1000));
	}
	
	public void CreateNewStructures(int quantity)
	{
		for (int i = 0; i < quantity; i++)
		{
			CreateNewStructure();
		}
	}
	
	#endregion
	
	#region StructureList
	// To refresh the list of Structures in the scene.
	public void RefreshStructures()
	{
		StructureList = new Structure[0];
		foreach(Structure structure in FindObjectsOfType(typeof(Structure)))
		{
			StructureList = ArrayTools.PushLast(StructureList, structure);
		}
	}

	// To track and manage the Structure that is in the scene.
	public void SubscribeStructure(Structure structure)
	{
		StructureList = ArrayTools.PushLast(StructureList, structure);
	}

	// To stop tracking and managing the Structure that is in the scene.
	public void UnSubscribeStructure(Structure structure)
	{
		StructureList = ArrayTools.Remove(StructureList, structure);
	}
	#endregion
}
