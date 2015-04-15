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
using Excelsion.Tasks;

namespace Excelsion.Villagers
{

	public class Villager : MonoBehaviour
	{
		public string Name;
		public float Age;
		public Sprite Icon;

		[SerializeField]
		private Task task;
		private VillagerController vc;
		private TaskController tc;

		#region --GET from-- / --SET to-- VillagerMotor
		// TO MAKE "transform" get the VillagerMotor Transform
		private Transform motorTransform;
		public new Transform transform
		{
			get
			{
				if (motorTransform == null)
				{
					motorTransform = GetComponentInChildren<VillagerMotor>().transform;
				}
				return motorTransform;
			}
		}

		// TO MAKE "goal" get the VillagerMotor Transform
		private Vector3 motorGoal;
		public new Vector3 goal
		{
			get
			{
			//	if (motorGoal == null || motorGoal == Vector3.zero)
				motorGoal = GetComponentInChildren<VillagerMotor>().Goal;
				return motorGoal;
			}
			set
			{
				motorGoal = value;
				GetComponentInChildren<VillagerMotor>().Goal = value;
			}
		}

		[SerializeField]
		private float MinDistanceToGoal = 1.5F;
		public bool IsOnGoal
		{
			get
			{
				if (Vector3.Distance(transform.position, goal) <= MinDistanceToGoal)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		#endregion

		void Awake () {
			vc = VillagerController.Get();
			vc.SubscribeVillager(this); // --- to add villager into VillagerController
			task = this.gameObject.GetComponentInChildren<Task>();
			tc = TaskController.Get();
		}

		public void Rename(string newName)
		{
			name = newName;
			Name = newName;
		}

		#region work with Task
		// Get Current task
		public Task GetCurTask ()
		{
			return task;
		}

		// Assign New Task Instead of Current
		public void AssignTask (Task task)
		{
			GameObject prevTask = this.task.gameObject;
			GameObject newTask = Instantiate (task.gameObject) as GameObject;
			newTask.transform.SetParent (this.GetComponent<Transform>());
			this.task = newTask.GetComponent<Task>();
			Destroy(prevTask.gameObject);
			DoTask();
		}
		public void DoTask (Task task)
		{
			task.setupTask();
			GetComponentInChildren<VillagerMotor>().MoveTo(task.transform.position);
		}
		public void DoTask ()
		{
			DoTask(this.task);
		}
		#endregion

		//Just for variaty
		public void TeleportTo(Vector3 pos)
		{
			transform.position = pos;
		}

		void OnDestroy()
		{
			vc.UnSubscribeVillager(this);
		}
	}

}
