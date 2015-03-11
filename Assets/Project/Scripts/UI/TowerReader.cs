using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Excelsion.Towers;
using Excelsion.Inventory;

//Stephan Ennen - 3/7/2015

namespace Excelsion.UI
{
	public class TowerReader : MonoBehaviour 
	{
		TowerBase selected;

		//Object references
		public GameObject selectedInfoToggle;
		//Stats
		public Text info_rangeDisplay;
		public Text info_speedDisplay;
		public Text info_dmgDisplay;
		public Text info_luckDisplay;

		public Image inv_one;
		public Image inv_two;
		public Image inv_three;
		public Image inv_four;
		public Image inv_five;
		public Image inv_six;

		//GameObject drugObject;
		//int drugSlot;

		void Start () 
		{
			selectedInfoToggle.SetActive(false);
		}
		

		void Update () 
		{
			selected = TowerBase.GetSelected();

			if( selected != null )
			{
				selectedInfoToggle.SetActive(true);
				DisplayStats();
				DisplayInventory();
			}
			else
			{
				selectedInfoToggle.SetActive(false);
			}
			//if( drugObject != null )
			//{
			//	drugObject.transform.position = Input.mousePosition;
			//}
		}

		public static void GUIBegunDrag( int slot )
		{
			/*drugSlot = slot;
			switch( slot )
			{
				case 0:
					drugObject = inv_one;
					continue;
				case 1:
					drugObject = inv_two;
					continue;
				case 2:
					drugObject = inv_three;
					continue;
				case 3:
					drugObject = inv_four;
					continue;
				case 4:
					drugObject = inv_five;
					continue;
				case 5:
					drugObject = inv_six;
					continue;
			}*/
		}

		void DisplayStats()
		{
			info_rangeDisplay.text = "Range: "+ selected.stats.range.ToString("F1"); //F1 makes it only display the first decimal point (Tenths)
			info_speedDisplay.text = "Speed: "+ selected.stats.speed.ToString("F1"); //F2 makes it only display to the second decimal place (Hundredths)
			info_dmgDisplay.text = "Damage: "+ selected.stats.damage.ToString();
			info_luckDisplay.text = "Luck: "+ (selected.stats.luck * 100.0f).ToString("F1") +"%";
		}

		void DisplayInventory()
		{
			inv_one.sprite =   selected.inventory.contents[0].Icon;
			inv_two.sprite =   selected.inventory.contents[1].Icon;
			inv_three.sprite = selected.inventory.contents[2].Icon;
			inv_four.sprite =  selected.inventory.contents[3].Icon;
			inv_five.sprite =  selected.inventory.contents[4].Icon;
			inv_six.sprite =   selected.inventory.contents[5].Icon;
		}
	}
}