using UnityEngine;
using System.Collections;

namespace Excelsion.Inventory
{
	public class InventoryPlayer : Bag
	{
		public InventoryPlayer()
		{
			for( int i = 0; i < 12; i ++)
			{
				contents[i] = new ItemNull();
			}
		}

		// Use this for initialization
		void Start ()
		{
			
		}
		
		// Update is called once per frame
		void Update ()
		{
			
		}
	}
}
