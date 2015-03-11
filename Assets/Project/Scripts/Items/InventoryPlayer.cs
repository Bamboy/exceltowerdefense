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
			Debug.Log ("Null: " + inventory.contents [4].Name);
			for( int i = 0; i < icons.Length; i++ )
			{
				icons[i].sprite = inventory.contents[i].Icon;
			}
		}
	}
}
