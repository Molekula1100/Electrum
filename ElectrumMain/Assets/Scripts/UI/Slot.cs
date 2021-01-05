using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    private const string ITEM_NULL_NAME = "none";
    public bool inInv;
    public Item item;
    
    public void Respawn()
    {
        if(item != null) 
        {
            gameObject.GetComponent<Image>().sprite = item.icon;
            gameObject.GetComponent<Image>().color = new Color(255,255,255,255);
            gameObject.name = item.uniqName;
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(255,255,255,0);
            gameObject.name = ITEM_NULL_NAME;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
	{
        if(inInv)
        {
            InventoryManager.selectedItem = item;
        }
		else
        {
            Shop.selectedItem = item;
        }
        InventoryManager.UpdateDescr();
	}
}
