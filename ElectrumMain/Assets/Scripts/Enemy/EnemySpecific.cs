using System.Collections;
using UnityEngine;

public class EnemySpecific : EnemyBehaviour
{
    public float retreatSpeed;
    public float attackDistance; 
    public float fireRate;
    public int health;
    public int damage;

    public EnemySpecific()
    {
        RetreatSpeed = retreatSpeed;
        AttackDistance = attackDistance;
        FireRate = fireRate;
        Health = health;
        Damage = damage;
    }
}
