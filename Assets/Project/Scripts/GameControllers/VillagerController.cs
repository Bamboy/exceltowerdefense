/// <summary>
/// illagerController.cs
/// Sergey Bedov
/// 03/27/2015
/// ----------------------
/// General purpose: to get/... Villagers
/// ----------------------
/// </summary>
using UnityEngine;
using System.Collections;
using Excelsion.Villagers;

public class VillagerController : MonoBehaviour
{

	public Villager[] VillagerList;
	
	#region Access Instance Anywhere
	private static VillagerController villagerController;
	public static VillagerController Get()
	{
		if( villagerController != null )
			return villagerController;
		else
		{
			GameObject obj = new GameObject("_VillagerController");
			if (GameObject.Find("_Controllers") == null) {new GameObject("_Controllers");}
			obj.transform.parent = GameObject.Find("_Controllers").transform;
		//	obj.tag = "VillagerController";
			villagerController = obj.AddComponent< VillagerController >();
			return villagerController;
		}
	}
	#endregion
	
	void Awake ()
	{
		VillagerList = new Villager[0];
	}

	#region Create Villager(s)
	public void CreateNewVillager (string name, float age, Sprite icon, Vector3 pos)
	{
		GameObject instance = Instantiate (Resources.Load ("Prefabs/Villagers/VillagerGO")) as GameObject;
		Villager vil_inst = instance.GetComponent<Villager>();
		vil_inst.Name = name;
		vil_inst.Age = age;
		vil_inst.Icon = icon;
		vil_inst.transform.position = pos;
		//GameObject newVillager = Instantiate(Resources.Load("Prefabs/VillagerSample", GameObject));
		 
	}
	public void CreateNewVillager ()
	{
		CreateNewVillager ("Name Surname", 0, null, new Vector3(1000,2,1000));
	}

	public void CreateNewVillagers (int quantity)
	{
		for (int i = 0; i < quantity; i++)
		{
			CreateNewVillager();
		}
	}

	#endregion

	#region VillagerList
	// to refresh the list of villagers in scene
	public void RefreshVillagers()
	{
		VillagerList = new Villager[0];
		foreach(Villager villager in FindObjectsOfType(typeof(Villager)))
		{
			VillagerList = ArrayTools.PushLast(VillagerList,villager);
		}
	}

	//----- to track the Villager that is in a scene -----
	public void SubscribeVillager (Villager villager)
	{
		VillagerList = ArrayTools.PushLast(VillagerList,villager);
	}
	//----- to stop track the Villager that is Destroyed -----
	public void UnSubscribeVillager (Villager villager)
	{
		VillagerList = ArrayTools.Remove(VillagerList,villager);
	}
	#endregion
}
