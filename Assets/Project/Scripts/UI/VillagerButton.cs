using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Excelsion.Villagers;
using Excelsion.Tasks;

public class VillagerButton : MonoBehaviour
{
	public Button Button;
	public Text Name;
	public Text Age;
	public Image Icon;
	private ColorBlock activeVillagerColors;
	private ColorBlock assignedVillagerColors;
	private ColorBlock unassignedVillagerColors;

	[SerializeField]
	private Villager villager;

	void Awake ()
	{
		#region Villager Button Colors
		//ACTIVE (when button selected)
		assignedVillagerColors = Button.colors;
		assignedVillagerColors.normalColor = Color.yellow;
		assignedVillagerColors.highlightedColor = Color.yellow;
		assignedVillagerColors.pressedColor = Color.yellow;
		//ASSIGNED (when villager has task)
		assignedVillagerColors = Button.colors;
		assignedVillagerColors.normalColor = Color.green;
		assignedVillagerColors.highlightedColor = Color.yellow;
		assignedVillagerColors.pressedColor = Color.white;
		//UNASSIGNED (when villager don't have task)
		unassignedVillagerColors = Button.colors;
		unassignedVillagerColors.normalColor = Color.red;
		unassignedVillagerColors.highlightedColor = Color.yellow;
		unassignedVillagerColors.pressedColor = Color.white;
		#endregion
	}

	public void Papulate (Villager villager)
	{
		this.villager = villager;
		Name.text = villager.Name;
		Age.text = villager.Age.ToString();
		Icon.sprite = villager.Icon;
		UpdateColor();
	}

	// --- to color the button in Green/Red to show if the villager has task or not
	public void UpdateColor ()
	{
		if (villager.GetCurTask().Name == "Empty") {
			Button.colors = unassignedVillagerColors; print ("unassigned COLOR");
		} else {
			Button.colors = assignedVillagerColors; print ("assigned COLOR");
		}
	}

	public void PapulateAssignVillager ()
	{
		TaskMenu.Get().PapulateAssignVillager(villager, this);
		TaskMenu.Get().PapulateAssignTask(villager.GetCurTask());
	}
}
