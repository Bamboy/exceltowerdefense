/// <summary>
/// Task.cs
/// Sergey Bedov
/// 03/24/2015
/// ----------------------
/// General purpose: Holds task
/// ----------------------
/// </summary>
using UnityEngine;
using System.Collections;

public class Task : MonoBehaviour
{
	public string TaskName;
	public string TaskDescription;
	public Texture TaskImage;
	public string TaskGoalTag;
	public float TaskGoalStayTime;

	void Awake()
	{
		if (GameObject.Find("_VillagersTasks") == null) {new GameObject("_VillagersTasks");}
		this.transform.parent = GameObject.Find("_VillagersTasks").transform;
	}
}
