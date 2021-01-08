using System.Collections;
using UnityEngine;

public class EnemyGeneral : MonoBehaviour
{   
    protected const float minDistance = 1.5f;  

    [SerializeField] private float retreatSpeed;
    public float RetreatSpeed{ 
        get
        {
            return retreatSpeed;
        }
        set{}
    }  

    [SerializeField] private float attackDistance;
    public float AttackDistance{ 
        get
        {
            return attackDistance;
        }
        set{}
    }    

    [SerializeField] private float fireRate;
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

    [SerializeField] private int health;
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

    [SerializeField] private int damage;
    public int Damage
    {
        get
        {
            return damage;
        }

        set 
        {
            if(value <= 0)          
            {
                damage = 1;
            }
            else{
                damage = value;
            }
        }
    }
}
