using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Excelsion.Villagers;

public class VillagersScroll : MonoBehaviour
{
	public GameObject villagerButton;
	private Villager [] villagersList;
	
	public Transform contentPanel;
	
	void Start () {
		villagersList = VillagerController.Get().VillagerList;
		PopulateScroll ();
	}
	
	void PopulateScroll () {
		foreach (Villager villager in villagersList) {
			GameObject newButton = Instantiate (villagerButton) as GameObject;
			VillagerButton button = newButton.GetComponent <VillagerButton> ();
			button.Papulate(villager);
			newButton.transform.SetParent (contentPanel, false);
		}
	}
}
