/// <summary>
/// AssignTaskMenu.cs
/// Sergey Bedov
/// 03/24/2015
/// ----------------------
/// General purpose: UI Menu to assign Tasks to Villagers
/// ----------------------
/// </summary>
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Excelsion.Villagers;
using Excelsion.Tasks;

public class TaskMenu : MonoBehaviour
{
	public VillagerAssignPanel villagerAssignPanel;
	public TaskAssignPanel taskAssignPanel;
	public Button ReAssignButton;
	public Button AutoAssignButton;
	public Button BackToGameButton;

	public VillagerButton ActiveVillagerButton;

	#region Access Instance Anywhere
	private static TaskMenu taskMenu;
	public static TaskMenu Get()
	{
		if( taskMenu != null )
			return taskMenu;
		else
		{
			taskMenu = (TaskMenu)FindObjectOfType(typeof(TaskMenu));
			if (taskMenu != null)
				return taskMenu;
			else
			{
				GameObject instance = Instantiate(Resources.Load("GUI/TaskMenu/TaskMenu")) as GameObject;
				instance.transform.SetParent(GameObject.Find("Canvas").transform, false);
				taskMenu = instance.GetComponent<TaskMenu>();
				return taskMenu;
			}
		}
	}
	#endregion

	#region papulate Assign Menu Panels
	public void PapulateAssignVillager (Villager villager, VillagerButton villagerButton)
	{
		villagerAssignPanel.Papulate(villager);
		if ((bool)villagerButton)
		{
			villagerButton.UpdateColor();
			ActiveVillagerButton = villagerButton;
		}
	}
	public void PapulateAssignVillager(Villager villager)
	{
		PapulateAssignVillager (villager, this.ActiveVillagerButton);
	}
	public void PapulateAssignTask (Task task)
	{
		taskAssignPanel.Papulate(task);
	}
	#endregion
	void Start ()
	{
		//---Set the first VillagerButton as Active---
	//	foreach(Transform t in gameObject.GetComponentsInChildren<Transform>())
	//	{print ("Transform: " + t.name + "\n");}
	//	ActiveVillagerButton = gameObject.GetComponentInChildren<VillagerButton>();
	//	print (gameObject.GetComponentInChildren<VillagerButton>().Age.text);

		//---Pre-Assign Two Middle Panels with first villager and task ---
		print ("=== TASK MENU === <Start()>\n" + "Villagers Q-ty: " + VillagerController.Get().VillagerList.Length + " | Tasks Q-ty: " + TaskController.Get().TaskList.Length);
		PapulateAssignTask(TaskController.Get().TaskList[0]);
		PapulateAssignVillager(VillagerController.Get().VillagerList[0]);

		// --- Buttons Listeners ---
		ReAssignButton.onClick.AddListener(() => Assign());
		AutoAssignButton.onClick.AddListener(() => AutoAssign());
		BackToGameButton.onClick.AddListener(() => Close());

	}
	#region Menu Buttons Clicks
	void Assign()
	{
		print ("---ASSIGNED TASK TO VILLAGER---" + " \nTASK: " + taskAssignPanel.task.Name + " | VILLAGER: " + villagerAssignPanel.villager.Name);
		Villager villager = villagerAssignPanel.villager;
		villager.AssignTask(taskAssignPanel.task);
		PapulateAssignVillager(villager);
		ActiveVillagerButton.UpdateColor();
	}

	void AutoAssign()
	{
		Debug.Log("Auto-Assign function Doesn't work for now.");
	}

	void Close()
	{
		Destroy(gameObject);
	}
	#endregion
}
