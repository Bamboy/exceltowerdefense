using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Excelsion.Tasks;

public class TaskButton : MonoBehaviour
{
	public Button Button;
	public Text Name;
	public Text Details;
	public Text Reward;
	public Image Icon;
	
	[SerializeField]
	private Task task;

	public void Papulate (Task task)
	{
		this.task = task;
		Name.text = task.Name;
		Details.text = task.Details;
	//	Reward.text = task.Reward; //string for now TODO ex. make it System.Type
		Icon.sprite = task.Icon;
	}
	public void PapulateAssignTask ()
	{
		TaskMenu.Get().PapulateAssignTask(task);
	}
}
