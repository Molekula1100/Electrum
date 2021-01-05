using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Slot[] slots = new Slot[6];
    [SerializeField]
    public Slot[] weaponSlots = new Slot[2];
    public Player player;

    public static GameObject inventoryPanel;
    public static Text descrText;
    public static Item selectedItem;    
    public static GameObject useButton, dropButton;

    private void Start()
    {
        inventoryPanel = GameObject.Find("Inventory");
        inventoryPanel.SetActive(false);
        GameObject descr = inventoryPanel.transform.GetChild(1).gameObject;
        descrText = descr.GetComponent<Text>();
        descrText.text = "None item selected";
        useButton = inventoryPanel.transform.Find("UseButton").gameObject;
        dropButton = inventoryPanel.transform.Find("DropButton").gameObject;
    }

    public void UpdateInventory()
    {
       for(int i = 0; i < Inventory.capacity; i++)
        {
            if(i >= Inventory.items.Count)
            {
                slots[i].item = null;
            }
            else
            {
                slots[i].item = Inventory.items[i];
            }
        }
        for(int i = 0; i < Inventory.weapons.Length; i++)
        {
            weaponSlots[i].item = Inventory.weapons[i];
            weaponSlots[i].Respawn();
        }
        foreach (Slot slot in slots)
        {
            slot.Respawn();
        }
        if(Inventory.items.Count == 0)
        {
            foreach (Slot slot in slots)
            {
                slot.item = null;
                selectedItem = null;
                slot.Respawn();
            }
        }
    }

    public static void UpdateDescr()
    {
        if(Shop.selectedItem != null)
        {
            Item oitem = Shop.selectedItem;
            string otext = oitem.uniqName +"\n";
            otext += oitem.description +"\n";
            switch(oitem.id)
            {
                case 7:
                    otext += "Healing rate " + oitem.gameObject.GetComponent<HealPotion>().healingRate.ToString() + "\n";
                    break;
                case 8:
                    otext += "Speeding up rate " + oitem.gameObject.GetComponent<SpeedPotion>().speedingUpRate.ToString() + "\n";
                    break;
                default:
                    break;
            }
            otext += "Price " + oitem.price + " coins" + "\n";
            Shop.descrText.text = otext;
        }
        if(selectedItem != null)
        {
            Item oitem = selectedItem;
            string otext = oitem.uniqName +"\n";
            otext += oitem.description +"\n";
            switch(oitem.id)
            {
                case 7:
                    otext += "Healing rate " + oitem.gameObject.GetComponent<HealPotion>().healingRate.ToString() + "\n";
                    break;
                case 8:
                    otext += "Speeding up rate " + oitem.gameObject.GetComponent<SpeedPotion>().speedingUpRate.ToString() + "\n";
                    break;
                case 6:
                    break;
                default:
                    otext += "Damage " + oitem.gameObject.GetComponent<Weapon>().damage + "\n";
                    break;
            }
            otext += "Price " + oitem.price + " coins" + "\n";
            descrText.text = otext;
        }
        if(selectedItem == null)
        {
            descrText.text = "No items selcted";
        }
        if(Shop.selectedItem == null)
        {
            Shop.descrText.text = "No items selcted";
        }

        if(selectedItem != null && selectedItem.id < 6)
        {
            useButton.SetActive(false);
            dropButton.SetActive(false);
        }
        else
        {
            useButton.SetActive(true);
            dropButton.SetActive(true);
        }
    }

    public void Drop()
    {
        Player.DropItem(selectedItem.gameObject);
        selectedItem = null;
        UpdateDescr();
    }
    public void Use()
    {
        Player.UseItem(selectedItem.gameObject);
        selectedItem = null;
        UpdateDescr();
    }

    public void Buy()
    {
        player.BuyItem(Shop.selectedItem);
        Shop.selectedItem = null;
        UpdateDescr();
    }
}
