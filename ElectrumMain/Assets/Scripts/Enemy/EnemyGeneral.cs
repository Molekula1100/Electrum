using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyGeneral : MonoBehaviour
{   
    protected const float minDistance = 1.5f;  

    protected float RetreatSpeed{ get; set; } 
    protected float AttackDistance{ get; set; }    

    private float fireRate;
    protected float FireRate
    {
        get
        {
            return fireRate;
        }
        set
        {
            if(value < 0.1f)
            {
                fireRate = 0.1f;
            }
            else
            {
                fireRate = value;
            }
        }
    }

    private int health;
    public int Health
    {
        get
        {
            return health;
        }

        set 
        {
            if(value < 0)                                     
            {
                health = 0;
            }
            else 
            {
                health = value;
            }
        }
    }

    private int damage;
    public int Damage
    {
        get
        {
            return damage;
        }

        set 
        {
            if(value <= 0)              // урон не может быть нулевым или отрицательным
            {
                damage = 1;
            }
            else{
                damage = value;
            }
        }
    }
}
