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
			}
			else
			{
				selectedInfoToggle.SetActive(false);
			}
		}

		public static void SendButtonDown( int inventorySlot )
		{

		}

		void DisplayStats()
		{
			info_rangeDisplay.text = "Range: "+ selected.stats.range.ToString("F1"); //F1 makes it only display the first decimal point (Tenths)
			info_speedDisplay.text = "Speed: "+ selected.stats.speed.ToString("F2"); //F2 makes it only display to the second decimal place (Hundredths)
			info_dmgDisplay.text = "Damage: "+ selected.stats.damage.ToString();
			info_luckDisplay.text = "Luck: "+ (selected.stats.luck * 100.0f).ToString("F2") +"%";
		}
		void DisplayInventory()
		{

		}
	}
}