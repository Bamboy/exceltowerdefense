using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Excelsion.Inventory;

//Tristan Kidder - 3/15/2015

public class SlotScript : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler, IDragHandler
{

	public Item item;
	public int slotNumber;

	Image itemImage;
	InventoryPlayer inventory;

	// Use this for initialization
	void Start ()
	{
		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<InventoryPlayer> ();
		itemImage = gameObject.transform.GetChild (0).GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (inventory.Items[slotNumber].Icon != null)
		{
			item = inventory.Items[slotNumber];
			itemImage.enabled = true;
			itemImage.gameObject.SetActive(true);
			itemImage.sprite = inventory.Items[slotNumber].Icon;
		}
		else
		{
			itemImage.enabled = false;
			itemImage.gameObject.SetActive(false);
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
		if (inventory.Items[slotNumber].Name != null && !inventory.draggingItem)
		{
			inventory.ShowTooltip(inventory.slotList[slotNumber].GetComponent<RectTransform>().localPosition, inventory.Items[slotNumber]);
		}
	}

	public void OnPointerExit(PointerEventData data)
	{
		if (inventory.Items [slotNumber].Name != null)
		{
			inventory.CloseTooltip ();
		}
	}

	public void OnDrag(PointerEventData data)
	{
		if (inventory.Items [slotNumber].Name != null)
		{
			inventory.ShowDraggedItem(inventory.Items[slotNumber], slotNumber);
		}
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (inventory.Items [slotNumber].Name == null && inventory.draggingItem)
		{
			inventory.Items [slotNumber] = inventory.draggedItem;
			inventory.CloseDraggedItem();
		}
		else if (inventory.Items [slotNumber].Name != null && inventory.draggingItem)
		{
			inventory.Items [inventory.indexOfDraggedItem] = inventory.Items [slotNumber];
			inventory.Items [slotNumber] = inventory.draggedItem;
			inventory.CloseDraggedItem();
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		Debug.Log("index: " + inventory.indexOfDraggedItem + " slots: " + slotNumber);

		if (inventory.Items [slotNumber].Name == null)
		{
			inventory.Items [slotNumber] = inventory.draggedItem;
		}
		else if (inventory.Items [slotNumber].Name != null && inventory.draggingItem)
		{
			inventory.Items [inventory.indexOfDraggedItem] = inventory.Items [slotNumber];
			inventory.Items [slotNumber] = inventory.draggedItem;
		}
	}
}
