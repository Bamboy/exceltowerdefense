using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Excelsion.Inventory
{
	public class InventoryPlayer : MonoBehaviour
	{
		public Image[] icons = new Image[6];
		Bag inventory = new Bag(6);

		public InventoryPlayer()
		{

		}

		// Use this for initialization
		void Start ()
		{

		}
		
		// Update is called once per frame
		void Update ()
		{
			//Debug.Log ("Slot 0: " + inventory.contents [0].Name);
			for( int i = 0; i < icons.Length; i++ )
			{
				icons[i].sprite = inventory.contents[i].Icon;
			}
		}

		public void AddItemTest()
		{
			AddItem (0, 0);
			AddItem (1, 2);
		}

		public void AddItem(int id, int slot)
		{
			Debug.Log ("Adding fire!");
			switch(id)
			{
				case 0:
					inventory.contents [slot] = new ItemFireball ();
					break;
				case 1:
					inventory.contents [slot] = new ItemPaper();
					break;
				default:
					inventory.contents[slot] = new ItemNull();
					break;
			}
		}
	}
}
