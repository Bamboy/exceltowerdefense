using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Excelsion.Towers;
using Excelsion.Inventory;

// Matt McGrath - 4/22/2015, basically mimicing Stephan's TowerReader until we come up with a more flexible system for displaying any Selectable object.
namespace Excelsion.UI
{
	public class StructureReader : MonoBehaviour 
	{
		#region Fields
		// The Structure that has been Selected.
		Structure structureSelected;
		
		// Reference to our object which contains the UI elements we will display, set and manipulate.
		public GameObject selectedInfoToggle;

		private GameObject selectedSubInfoToggle;

		//public GameObject upgradeInfoToggle;

		// UI elements all types Structures share.
		public Text info_structureName;
		public Text info_structureLevel;
		public Text info_structureType;
		public Image info_structureIcon;
		#endregion

		#region Initialization
		void Start() 
		{
			// We don't want to show our Structure UI yet, so set it to not Active.
			selectedInfoToggle.SetActive(false);

			// Using our SelectionController, hook up our event for what happens when a Selection has changed.
			SelectionController.Get().onSelectionChanged += this.OnSelectionChanged;
		}
		#endregion

		#region Updates
		// Monobehavior Update. Called every frame that we're active.
		void Update() 
		{
			// If we have a valid  Structure object selected...
			if(structureSelected != null)
			{
				// Set the parent UI object to active, so it can render to the screen.
				selectedInfoToggle.SetActive(true);
				//upgradeInfoToggle.SetActive(true);

				if (selectedSubInfoToggle != null)
				{
					selectedSubInfoToggle.SetActive(true);
				}

				// The above now allows us access to UI elements, so let's update them so they're displayed accurately.
				UpdateStats();
				UpdateIcons();
				UpdateSpecificStats();
			}
			// Otherwise our selection isn't even a Structure, so we don't want to show our Structure UI, so set it as not Active.
			else
			{
				selectedInfoToggle.SetActive(false);
				//upgradeInfoToggle.SetActive(false);

				if (selectedSubInfoToggle != null)
				{
					selectedSubInfoToggle.SetActive(false);
				}
			}
		}

		// Updates the UI Text elements all Structures [may] share.
		void UpdateStats()
		{
			info_structureName.text = "Name: " + structureSelected.Name;

			// Can houses have levels?
			if (structureSelected is StructureHouse == false)
			{
				if (structureSelected.Level > 0)
				{
					info_structureLevel.text = "Level: " + structureSelected.Level.ToString();
				}
				else if (structureSelected.isBeingBuilt)
				{
					info_structureLevel.text = "Currently being constructed";
				}
			}
			// If structure is a house, don't display level?
			else
			{
				info_structureLevel.text = "";
			}

			// We don't really use this right now.
			info_structureType.text = "Type: " + structureSelected.StructureType.ToString();
		}
		
		// Updates the Icons (though they'll probably never change, will they?).
		void UpdateIcons()
		{
			info_structureIcon.sprite = structureSelected.Icon;
			if (info_structureIcon == null)
				Debug.Log ("ICON NULL!");
			
			//TODO: Display the icon, if we choose to use icons instead of text like "Food:", which looks kind of lame on UIs IMO.
		}

		// Updates Structure-specific UI elements. For example, a Windmill has a Production statt we'll want to display.
		// TODO: MUCH better way to design this. As is, this requires knowing what UI labels we have set up in the canvas. We shouldn't have to know that.
		void UpdateSpecificStats()
		{
			if (structureSelected is StructureWindmill)
			{
				foreach (Transform go in selectedInfoToggle.transform)
				{
					if (go.name == "Windmill Stats")
					{
						if (selectedSubInfoToggle != null)
							selectedSubInfoToggle.SetActive(false);

						selectedSubInfoToggle = go.gameObject;
						selectedSubInfoToggle.SetActive(true);
						Text[] windmillTexts = selectedSubInfoToggle.GetComponentsInChildren<Text>();
						windmillTexts[0].text = "Production: " + ((StructureWindmill)structureSelected).FoodProducedPerDay.ToString () + " Food / Day";
						windmillTexts[1].text = "";//"Upgrade Requirements: ";
					}
					else
					{
						// 
					}
				}
			}
			if (structureSelected is StructureHouse)
			{
				foreach (Transform go in selectedInfoToggle.transform)
				{
					if (go.name == "House Stats")
					{
						if (selectedSubInfoToggle != null)
							selectedSubInfoToggle.SetActive(false);

						selectedSubInfoToggle = go.gameObject;
						selectedSubInfoToggle.SetActive(true);
						Text[] houseTexts = selectedSubInfoToggle.GetComponentsInChildren<Text>();
						houseTexts[0].text = "Home to a villager.";
					}
					else
					{
						// 
					}
				}
			}
			if (structureSelected is StructureWoodCutter)
			{
				foreach (Transform go in selectedInfoToggle.transform)
				{
					if (go.name == "Woodcutter Stats")
					{
						if (selectedSubInfoToggle != null)
							selectedSubInfoToggle.SetActive(false);
						
						selectedSubInfoToggle = go.gameObject;
						selectedSubInfoToggle.SetActive(true);
						Text[] windmillTexts = selectedSubInfoToggle.GetComponentsInChildren<Text>();
						windmillTexts[0].text = "Production: " + ((StructureWoodCutter)structureSelected).WoodProducedPerDay.ToString () + " Wood / Day";
						windmillTexts[1].text = "";//"Upgrade Requirements: ";
					}
					else
					{
						// 
					}
				}
			}
			if (structureSelected is StructureStoneCutter)
			{
				foreach (Transform go in selectedInfoToggle.transform)
				{
					if (go.name == "Stonecutter Stats")
					{
						if (selectedSubInfoToggle != null)
							selectedSubInfoToggle.SetActive(false);
						
						selectedSubInfoToggle = go.gameObject;
						selectedSubInfoToggle.SetActive(true);
						Text[] windmillTexts = selectedSubInfoToggle.GetComponentsInChildren<Text>();
						windmillTexts[0].text = "Production: " + ((StructureStoneCutter)structureSelected).StoneMinedPerDay.ToString () + " Stone / Day";
						windmillTexts[1].text = "";//"Upgrade Requirements: ";
					}
					else
					{
						// 
					}
				}
			}

		}
		#endregion

		#region Selection Changing
		// What happens when our Selection is changed? (Hook this to SelectionController's OnSelectionChanged delegate.
		void OnSelectionChanged(Component newSelection)
		{
			// If we've Selected a Structure, set our structureSelected reference to the selected object.
			if (newSelection is Structure)
				structureSelected = newSelection as Structure;
			// Otherwise, we didn't select a Structure, so we don't want to handle it in this reader.
			else
				structureSelected = null;
		}
		#endregion
	}
}