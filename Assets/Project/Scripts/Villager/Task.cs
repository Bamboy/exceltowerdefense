/// <summary>
/// Task.cs
/// Sergey Bedov
/// 04/02/2015
/// ----------------------
/// General purpose: Holds &&& Controls GOAL, PROCESS & other task info
/// ----------------------
/// </summary>
using UnityEngine;
using System.Collections;
using Excelsion.Villagers;
using Excelsion.GameManagers;

namespace Excelsion.Tasks
{
	[System.Serializable]
	public class Task : MonoBehaviour
	{
		// this are to make Tasks prefabs
		public enum TaskType{Stay, GoTo, Gather, Build, GoHome}
		public TaskType TheTaskType;

		public delegate void SetupTask();
		public SetupTask setupTask;

		public string Name;
		public string Details;
		public Sprite Icon;

		public string GoalTag; //--- same as GoalTask, but looking for the closest object with Tag (if both populated - chouses the closest)

		public float TaskProgress; // --- task progress in percents/100 (min=0, max=1)

		public float GoalDelay=10F; // --- this is time villager has to stay on goal to gain Reward

//		public Reward TaskReward;
		public GameResources RewardResources;

		public Task NextTask; // --- GoHome or Empty by default

		private Villager curVillager;
		private Villager villager
		{
			get{
				if (curVillager == null)
				{
					curVillager = GetComponentInParent<Villager>();
				}
				return curVillager;
			}
		}

		void Awake ()
		{
			switch (TheTaskType)
			{
			case TaskType.GoTo:
				setupTask = setupGoToTask;
				break;
			case TaskType.Gather:
				setupTask = setupGatherTask;
				break;
			case TaskType.Build:
				setupTask = setupBuildTask;
				break;
			case TaskType.GoHome:
				setupTask = setupGoHomeTask;
				break;
			default: // same that TaskType.Stay
				setupTask = setupStayTask;
				break;
			}
			TaskProgress = 0F;
		}

		void Start ()
		{

		}

		#region SetupTask Delegate methods
		void setupStayTask()
		{
			transform.position = GetComponentInParent<Villager>().transform.position;
		}
		void setupGoToTask()
		{
			if (GoalTag.Length != 0)
			{
				transform.position = ClosestWithTag(GoalTag).transform.position;
			}
		}
		void setupGatherTask()
		{

			if (GoalTag.Length != 0)
			{
				transform.position = ClosestWithTag(GoalTag).transform.position;
			}

		}
		void setupBuildTask()
		{
		}

		void setupGoHomeTask()
		{
			transform.position = villager.GetComponent<Transform>().position;
		}

		#endregion

		private GameObject ClosestWithTag(string theTagObject)
		{
			GameObject gObjWithTag = null; //so that villager just stay on place if there is no such tag
			Vector3 curPos = GetComponentInParent<Villager>().transform.position;
			float minDistance = 99999F;
			foreach (GameObject gObj in GameObject.FindGameObjectsWithTag(theTagObject))
			{
				float distance = Vector3.Distance(curPos, gObj.transform.position);
				if (distance > 0.1F && distance < minDistance)
				{
					minDistance = distance;
					gObjWithTag = gObj;
				}
			}
			return gObjWithTag;
		}

		void Update ()
		{
			//HOLD ON GOAL
			if (Name != "Empty")
			{
				if (GetComponentInParent<Villager>().IsOnGoal) // if Person Reached the Goal Point
				{
					if (TaskProgress < 1F)
					{
						TaskProgress = TaskProgress + Time.deltaTime / GoalDelay;
					}
					else
					{
						Done ();
					}
				}
			}
		}

		#region Task playback

		public void OnGoal()
		{
			print (TaskProgress);
			if (TaskProgress < 1F)
			{
				TaskProgress = TaskProgress + Time.deltaTime / GoalDelay;

			}
			else
			{
				Done ();
			}
		}

		// --- called when the task is done, and is time to gain Reward
		public void Done()
		{
			string taskDoneMessage = "" + villager.Name + " | " + this.Name + " | F="
				+ RewardResources.Food + "|W=" + RewardResources.Wood + "|S="
					+ RewardResources.Stone + "|P=" + RewardResources.Population; // TEMP
			NotificationLog.Get ().PushNotification(new Notification(taskDoneMessage, Color.green, 5.0f));
			ResourceController.Get().AddResources(RewardResources);
			if (NextTask == null)
				NextTask = TaskController.Get().GetEmpty();
			print ("--- ASSIGNED TASK ---\n Villager: " + villager + " | Task: " + NextTask);
			villager.AssignTask(NextTask);
			TaskMenu.Get().PapulateAssignVillager(villager);
			TaskMenu.Get().PapulateAssignTask(NextTask);
		}

		#endregion


	}

}
