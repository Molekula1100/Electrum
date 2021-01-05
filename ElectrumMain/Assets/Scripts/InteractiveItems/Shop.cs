using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private Item[] AvailableItems;

    public Slot[] slots;

    public static List<Item> items = new List<Item>();
    public static Item selectedItem;
    public static Text descrText;
    public static GameObject ShopPanel;


    private void Start()
    {
        ShopPanel = GameObject.Find("Shop");
        ShopPanel.SetActive(false);
        GameObject descr = ShopPanel.transform.GetChild(1).gameObject;
        descrText = descr.GetComponent<Text>();
        descrText.text = "None item selected";
        SpawnItems();
        Respawn();
    }
    private void SpawnItems()
    {
        for(int i = 0; i < Random.Range(1,slots.Length+1); i++)
        {
            Item item = GetRandomItem();
            items.Add(item); 
        }
    }
    private Item GetRandomItem()
    {
        int randItem = (int)Random.Range(0f, AvailableItems.Length);
        Item item = AvailableItems[randItem];
        return item;
    }

    public void Respawn()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i >= items.Count)
            {
                slots[i].item = null;
            }
            else
            {
                slots[i].item = items[i];
            }
            slots[i].Respawn();
        }
    }
}
