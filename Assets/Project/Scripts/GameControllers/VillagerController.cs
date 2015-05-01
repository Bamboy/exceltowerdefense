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
using Excelsion.GameManagers;

public class VillagerController : MonoBehaviour
{
	public TaskMenu TheTaskMenu;

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

			// Let's child any Controller with a _Controllers object, creating it if it's not already present.
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
		if( villagerController == null )
			villagerController = this;
		else
			GameObject.Destroy( this.gameObject );

		int villagersQty = ResourceController.Get().ResourceAmount(ResourceType.Population);
		VillagerList = new Villager[0];
		CreateNewVillagers(villagersQty);

		TheTaskMenu = TaskMenu.Get();
		TheTaskMenu.HideShow(day);

	}

	void Start()
	{
		WorldClock.onDawn += TheTaskMenu.HideShow;

	}

	void Update()
	{
	//	if WorldClock.
	//	WorldClock.onDusk += TheTaskMenu.HideShow;
	}

	private int day;
	private void onDawn()
	{
	//	DayNightCycle
	}

//	private IEnumerator test()
//	{
//		yield return new WaitForSeconds (1);
//	}

	#region Create Villager(s)
	public void CreateNewVillager (string name, float age, Sprite icon, Vector3 pos)
	{
		Object [] vil_res = Resources.LoadAll ("Prefabs/Villagers");
		GameObject instance = Instantiate (vil_res[Random.Range(0, vil_res.Length-1)], pos, Quaternion.identity) as GameObject;
		Villager vil_inst = instance.GetComponent<Villager>();
	//	vil_inst.Name = name;
	//	vil_inst.Age = age;
	//	vil_inst.Icon = icon;
	}
	public void CreateNewVillager ()
	{
		//TODO define villagers spawn position (If they are going to be born? Where?)
		Vector3 villagersSpawner = new Vector3(Random.Range(-8,8), 1F, Random.Range(-10,-15)); // DEFINE IT MANUALY HERE FOR NOW
		CreateNewVillager ("Name Surname", 0, null, villagersSpawner);
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
