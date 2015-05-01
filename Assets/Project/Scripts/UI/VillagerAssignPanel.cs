using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Excelsion.Villagers;
using Excelsion.Tasks;
using Excelsion.GameManagers;

public class VillagerAssignPanel : MonoBehaviour
{
	public Text Name;
	public Text Age;
	public Image Icon;
	public Image TaskStatus; // to change color & text

	public Villager villager;
	
	public void Papulate (Villager villager)
	{
		this.villager = villager;
		Name.text = villager.Name;
		Age.text = villager.Age.ToString();
		Icon.sprite = villager.Icon;

	// currently assigned task papulating:
		if (VillagerController.Get().TheTaskMenu.isActiveAndEnabled)
		{
			Task task = villager.GetCurTask();
			if (task.Name == "Empty")
				TaskStatus.color = Color.red;
			else
				TaskStatus.color = Color.green;
			TaskStatus.GetComponentInChildren<Text>().text = "CurTask: " + task.Name + ".";
		}
	// ------------------------
	}

}
