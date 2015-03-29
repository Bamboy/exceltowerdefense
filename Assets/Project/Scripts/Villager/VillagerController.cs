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
		Refresh();
	}

	// to refresh the list of villagers in scene
	public void Refresh()
	{
		VillagerList = new Villager[0];
		foreach(Villager villager in FindObjectsOfType(typeof(Villager)))
		{
			VillagerList = ArrayTools.PushLast(VillagerList,villager);
		}
	}
}
