using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Excelsion.Tasks;

public class TaskAssignPanel : MonoBehaviour
{
	public Text Name;
	public Text Details;
	public Text Reward; // TODO has to be not string, ut something like Resource or System.Type + Ammount...
	public Image Icon;
	
	public Task task;
	
	public void Papulate (Task task)
	{
		this.task = task;
		Name.text = task.Name;
		Details.text = task.Details;
	//	Reward.text = task.Reward;
		Icon.sprite = task.Icon;
	}
}
