using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGeneral : EnemyBehaviour
{   

    [SerializeField] protected float retreatSpeed; // скорость отступления

    [SerializeField] private float attackRate;

    protected float AttackRate
    {
        get
        {
            return attackRate;
        }

        set
        {
            if(value <= 0)
            {
                attackRate = 0.2f;
            }
            else
            {
                attackRate = value;
            }
        }
    }
    
    [SerializeField] public float atackDistance; 

    [SerializeField] protected float minDistance = 1f;  // минимальная дистанция от плеера, при нарушении которой враг начинает отступать


    [SerializeField] private float health;

    public float Health
    {
        get
        {
            return health;
        }

        set 
        {
            if(value < 0)               // здоровье не может быть отрицательным
            {
                health = 0;
            }
            else 
            {
                health = value;
            }
        }
    }




    [SerializeField] private float damage;

    public float Damage
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
