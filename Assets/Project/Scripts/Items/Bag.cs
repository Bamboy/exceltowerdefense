using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Excelsion.Inventory
{
	//Represents a collection of items.
	public class Bag : System.Object
	{
		public Item[] contents;
		public Bag( int size )
		{
			contents = new Item[ Mathf.Max(0, size) ];
		}

		//Search for empty slot and add the item. Returns false if it isnt possible.
		public bool Add( Item obj )
		{
			return false;
		}
		//Increase inventory size by one.
		public void IncreaseSize()
		{

		}
	}
}