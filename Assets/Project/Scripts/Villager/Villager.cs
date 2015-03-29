/// <summary>
/// Villager.cs
/// Sergey Bedov
/// 03/24/2015
/// ----------------------
/// General purpose: have task, knows how to use it
/// ----------------------
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Villager : Person, IVillager
{
	public string Name;
	public float Age;
	public string Spec;
	public Sprite Icon;
	public bool IsAssigned;
	public Button.ButtonClickedEvent thingToDo;

	private Skills villagerSkills;
	private Task villagerTask;
	private VillagerController vm;
	private TaskController tm;

	void Awake () {
		villagerSkills = GetComponent<Skills>();
		vm = VillagerController.Get();
		vm.Refresh();

		tm = TaskController.Get();
		GameObject test = Instantiate(tm.TaskList[0].gameObject) as GameObject;
	}

	void FixedUpdate () {
	
	}

	#region IVillager implementation
	public void DoTask (Task task)
	{

	}
	public void DoTask ()
	{
		DoTask(villagerTask);
	}
	public Task VillagerTask
	{
		get { return villagerTask; }
		set { villagerTask = value; }
	}
	#endregion

	void OnDestroy()
	{
		vm.Refresh();
	}
}
