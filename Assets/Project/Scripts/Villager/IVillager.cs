/// <summary>
/// IVillager.cs
/// Sergey Bedov
/// 03/24/2015
/// ----------------------
/// General purpose: Declare all the public methods of Villager.cs
/// ----------------------
/// </summary>
using UnityEngine;

public interface IVillager
{
	void DoTask(Task task);
	Task VillagerTask{get; set;}
	void DoTask();
}
