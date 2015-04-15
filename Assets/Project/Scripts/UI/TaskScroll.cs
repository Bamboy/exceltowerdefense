using UnityEngine;
using System.Collections;
using Excelsion.Tasks;

public class TaskScroll : MonoBehaviour
{
	public GameObject taskButton;
	private Task[] taskList;

	public Transform contentPanel;

	void Start ()
	{
		taskList = TaskController.Get().TaskList;
		PopulateScroll ();
	}

	void PopulateScroll () {
		foreach (Task task in taskList) {
			GameObject newButton = Instantiate (taskButton) as GameObject;
			TaskButton button = newButton.GetComponent <TaskButton> ();
			button.Papulate(task);
			newButton.transform.SetParent (contentPanel);
			button.transform.localScale = Vector3.one; // this is to make sure, the button is not scaled by Layout
		}
	}
}
