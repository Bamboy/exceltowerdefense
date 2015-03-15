using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Excelsion.Inventory;

//Tristan Kidder - 3/15/2015

public class InventoryPlayer : MonoBehaviour
{
	public List<GameObject> slotList = new List<GameObject>();
	public List<Item> Items = new List<Item>();
	public GameObject slots;
	public GameObject tooltip;
	public GameObject draggedItemGameObject;
	public bool draggingItem = false;
	public Item draggedItem;
	public int indexOfDraggedItem;

	
	int x = -160;
	int y = 105;
	// Use this for initialization
	void Start ()
	{
		int slotAmount = 0;
		
		for (int i = 0; i < 2; i++)
		{
			for( int j = 0; j < 3; j++ )
			{
				GameObject slot = (GameObject)Instantiate(slots);
				
				slot.GetComponent<SlotScript>().slotNumber = slotAmount;
				slotList.Add(slot);
				if( i % 2 == 0 )
				{
					Items.Add(new ItemFireball());
				}
				else
				{
					Items.Add(new ItemNull());
				}
				slot.name = "Slot: " + x + "." + y;
				slot.transform.SetParent(this.gameObject.transform, false);
				slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
				x += 62;
				slotAmount++;
			}
			y -= 62;
			x = -160;
		}
		
		//database = GameObject.FindGameObjectWithTag ("ItemDatabase").GetComponent<ItemDatabase> ();
		
		//AddItem (0);
		//AddItem (1);
		//AddItem (2);
		//AddItem (3);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (draggingItem)
		{
			Vector3 pos = Input.mousePosition - GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().localPosition;
			draggedItemGameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3((Input.mousePosition.x - 420) + 10, (Input.mousePosition.y - 183) - 15, 0);// = new Vector3(Input.mousePosition.x, pos.y*2, 0);
		}
	}
	
	void AddItem(int itemID)
	{
		/*for (int i = 0; i < database.Items.Count; i++)
		{
			if( database.Items[i]._itemID == itemID )
			{
				Item item = database.Items[i];
				AddItemToEmptySlot(item);
				break;
			}
		}*/
	}
	
	void AddItemToEmptySlot(Item item)
	{
		for( int i = 0; i < Items.Count; i++ )
		{
			if( Items[i].Name == null )
			{
				Items[i] = item;
				break;
			}
		}
	}
	
	public void ShowTooltip(Vector3 toolPosition, Item item)
	{
		tooltip.SetActive (true);
		tooltip.GetComponent<RectTransform> ().localPosition = new Vector3 (toolPosition.x + 500, toolPosition.y, toolPosition.z);

		tooltip.transform.GetChild (0).GetComponent<Text> ().text = "  " + item.Name;
		tooltip.transform.GetChild (1).GetComponent<Text> ().text = "  " + item.Stats;
	}
	
	public void ShowDraggedItem(Item item, int slotNumber)
	{
		CloseTooltip ();
		indexOfDraggedItem = slotNumber;
		
		draggedItemGameObject.SetActive (true);
		draggedItem = item;
		draggingItem = true;
		draggedItemGameObject.GetComponent<Image> ().sprite = item.Icon;
	}
	
	public void CloseDraggedItem()
	{
		draggingItem = false;
		draggedItemGameObject.SetActive (false);
	} 
	
	public void CloseTooltip()
	{
		tooltip.SetActive (false);
	}
	
}
