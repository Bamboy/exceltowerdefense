using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Stephan Ennen - 3/7/2015

namespace Excelsion.Inventory
{
	//Represents a collection of items.
	public class Bag : System.Object
	{
		public Item[] contents;
		public Bag( int size )
		{
			contents = new Item[ Mathf.Max(0, size) ];
			contents = ArrayTools.CreateRepeat<Item>( new ItemNull(), Mathf.Max(0, size) );
			for( int i = 0; i < size; i ++)
			{
				contents[i] = new ItemNull();
			}
		}

		public Bag()
		{

		}

		//Search for empty slot and add the item. Returns false if it isnt possible.
		bool Add( Item obj ) //TODO - make it so it takes a slot number instead?
		{
			if( Contains( obj ) )
				return false;
			else
			{
				contents = ArrayTools.PushLast<Item>( contents, obj );
				OnChanged ();
				return true;
			}
		}

		//Attempt to take (remove) the item in the slot.
		public bool Take( int slot )
		{
			if( IsNull(contents[slot]) )
			{
				return false; //It's already empty!
			}
			else
			{
				contents[slot] = new ItemNull();
				OnChanged();
				return true;
			}
		}
		//Try to put the item in the first free spot.
		public bool Give( Item obj )
		{
			int slot = GetEmpty();
			if( slot == -1 )
				return false;
			else
			{
				contents[slot] = obj;
				OnChanged();
				return true;
			}
		}
		//Increase inventory size by one. (Use ArrayTools.cs)
		public void IncreaseSize()
		{
			Add( new ItemNull() );
			//TODO - Take away money depending on total slot size.
		}
		//Called when an item is taken or given to this bag.
		public void OnChanged()
		{

		}

		public int GetEmpty() //Returns first slot at which there is an empty space. Returns -1 if there isnt one.
		{
			for(int i = 0; i < contents.Length; i++ )
			{
				if( IsNull(contents[i]) ) //If this element equals value and it ISNT null...
				{
					return i;
				}
			}
			return -1;
		}
		public bool HasEmpty() //Same as above but returns true/false
		{ return (GetEmpty() != -1); }

		public bool Contains( Item obj )
		{
			foreach( Item i in contents )
			{
				if( i == obj && !IsNull(i) ) //If this element equals value and it ISNT null...
				{
					return true;
				}
			}
			return false;
		}


		static bool IsNull( Item obj )
		{
			if( obj as ItemNull != null )
				return false;
			else
				return true;
		}
	}
}