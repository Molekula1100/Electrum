using System;
using UnityEngine;
using System.Collections.Generic;


[Serializable]
public class Item : MonoBehaviour
{
    public string uniqName;
    public string description;
    public int id;
    public int price;  
    
    public Sprite icon;
}
