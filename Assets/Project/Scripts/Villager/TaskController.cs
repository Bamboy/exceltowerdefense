/// <summary>
/// TaskController.cs
/// Sergey Bedov
/// 03/27/2015
/// ----------------------
/// General purpose: to get/use/... Tasks from Prefabs/Tasks
/// ----------------------
/// </summary>
using UnityEngine;
using System.Collections;

public class TaskController : MonoBehaviour
{
	public Task[] TaskList;

	#region Access Instance Anywhere
	private static TaskController taskController;
	public static TaskController Get()
	{
		if( taskController != null )
			return taskController;
		else
		{
			GameObject obj = new GameObject("_TaskController");
			if (GameObject.Find("_Controllers") == null) {new GameObject("_Controllers");}
			obj.transform.parent = GameObject.Find("_Controllers").transform;
		//	obj.tag = "TaskController";
			taskController = obj.AddComponent< TaskController >();
			return taskController;
		}
	}
	#endregion

	void Awake ()
	{
		TaskList = new Task[0];
		foreach(Task task in Resources.LoadAll("Prefabs/Tasks", typeof(Task)))
		{
			Debug.Log("prefab found: " + task.name + task.TaskDescription);
			TaskList = ArrayTools.PushLast(TaskList,task);
		}
	}

	public void Temp()
	{

	}
}
