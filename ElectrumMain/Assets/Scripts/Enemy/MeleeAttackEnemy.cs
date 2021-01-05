using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeAttackEnemy : EnemyGeneral
{

    private bool isReadyToRetreat;

    
    void Start()
    {
        aiPath = GetComponent<AIPath>();
    }


    void Update()
    {
        if(isActive)        
        EnemyIsActive(atackDistance, transform.position);



        if(IsReadyToAtack)      
        EnemyIsReadyToAtack(atackDistance, transform.position, AttackRate, Damage);


        
        isReadyToRetreat = IsReadyToRetreat(minDistance);
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