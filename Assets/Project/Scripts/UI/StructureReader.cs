using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Excelsion.Towers;
using Excelsion.Inventory;

// Matt McGrath - 4/22/2015, basically using Stephan's TowerReader until we come up with a more flexible system for displaying Selectable objects.
namespace Excelsion.UI
{
	public class StructureReader : MonoBehaviour 
	{
		Structure selected;
		
		// Object references
		public GameObject selectedInfoToggle;

		// Stats
		public Text info_structureName;
		public Text info_structureAge;
		public Text info_structureType;
		public Image info_structureIcon;

		void Start () 
		{
			selectedInfoToggle.SetActive(false);
			SelectionController.Get().onSelectionChanged += this.OnSelectionChanged;
		}
		
		void OnSelectionChanged( Component newSelection )
		{
			if( newSelection is Structure )
				selected = newSelection as Structure;
			else
				selected = null;
		}
		
		void Update () 
		{
			if( selected != null )
			{
				selectedInfoToggle.SetActive(true);
				DisplayStats();
				DisplayIcon();
			}
			else
			{
				selectedInfoToggle.SetActive(false);
			}
		}
		
		void DisplayStats()
		{
			info_structureName.text = "Name: " + selected.Name;
			info_structureAge.text = "Age: " + selected.Age.ToString("F0");
			info_structureType.text = "Type: " + selected.StructureType.ToString();
		}
		
		void DisplayIcon()
		{
			info_structureIcon.sprite = selected.Icon;
			if (info_structureIcon == null)
				Debug.Log ("ICON NULL!");
		}
	}
}