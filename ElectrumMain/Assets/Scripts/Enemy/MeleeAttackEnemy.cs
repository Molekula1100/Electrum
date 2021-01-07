using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeAttackEnemy : EnemySpecific
{

    private bool isReadyToRetreat;

    private void Awake()
    {
        RetreatSpeed = retreatSpeed;
        AttackDistance = attackDistance;
        FireRate = fireRate;
        Health = health;
        Damage = damage;
    }
    
    void Start()
    {
        RetreatSpeed = retreatSpeed;
        AttackDistance = attackDistance;
        FireRate = 4f;
        Health = health;
        Damage = damage;
        aiPath = GetComponent<AIPath>();
    }

    void Update()
    {
        isReadyToRetreat = IsReadyToRetreat();
        if(isReadyToRetreat)

        transform.Translate(-Direction(player.transform.position, transform.position) * Time.deltaTime * retreatSpeed);
    }

    public override void AtackAnimations()
    {
        animator.SetBool("idle", false);
        animator.SetBool("Atack", true);
        animator.SetBool("Run", false);
    }

    public override void RunAnimations()
    {
        animator.SetBool("idle", false);
        animator.SetBool("Atack", false);
        animator.SetBool("Run", true);
    }

    public override void Atack()
    {
        
    }
}