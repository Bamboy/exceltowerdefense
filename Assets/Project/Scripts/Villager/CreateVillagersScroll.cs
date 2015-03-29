using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CreateVillagersScroll : MonoBehaviour
{
	public GameObject villagerButton;
	private Villager [] villagersList;
	
	public Transform contentPanel;
	
	void Start () {
		villagersList = VillagerController.Get().VillagerList;
		PopulateList ();
	}
	
	void PopulateList () {
		foreach (Villager villager in villagersList) {
			GameObject newButton = Instantiate (villagerButton) as GameObject;
			VillagerButton button = newButton.GetComponent <VillagerButton> ();
			button.nameLabel.text = villager.Name;
			button.ageLabel.text = villager.Age.ToString();
			button.specLabel.text = villager.Spec;
			button.icon.sprite = villager.Icon;
			button.IsAssigned.isOn = villager.IsAssigned;
			// button.button.onClick = villager.thingToDo;
			newButton.transform.SetParent (contentPanel);
		}
	}
	
	public void SomethingToDo () {
		Debug.Log ("I done did something!");
	}
	
	public void SomethingElseToDo (GameObject villager) {
		Debug.Log (villager.name);
	}
}
