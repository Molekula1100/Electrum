using System;
using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour 
{
    private const int WEAPON_COUNT = 2;

    public static int capacity = 6;
    public static int coins = 1000;

    public static List<Item> items = new List<Item>();
    public static Weapon[] weapons = new Weapon[WEAPON_COUNT];
    public static InventoryManager invManager;
    public static Shop shop;

    private void Start()
    {
        GameObject shopObj = GameObject.Find("GameController");
        shop = shopObj.GetComponent<Shop>();
        GameObject invObj = GameObject.FindWithTag("InvObj");
        invManager = invObj.GetComponent<InventoryManager>();
        items.Clear();
    }
    public static void AddToInventory(Item item)
    {
        if(items.Count < capacity)
        {
            items.Add(item);
            invManager.UpdateInventory();
            shop.Respawn();
        }
    }

    public static void Equip(Weapon weapon, int number)
    {
        weapon.number = number;
        weapons[number-1] = weapon;
    }

    public static bool CheckWeapon(int number)
    {
        return weapons[number-1] != null;
    }
}
