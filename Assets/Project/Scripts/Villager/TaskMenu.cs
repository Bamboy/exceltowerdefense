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

public class TaskMenu : MonoBehaviour
{
	#region Access Instance Anywhere
	private static TaskMenu taskMenu;
	public static TaskMenu Get()
	{
		if( taskMenu != null )
			return taskMenu;
		else
		{
			throw new Exception("There is no Task Menu GameObject."); //TODO get the menue from Prefabs
		}
	}
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
