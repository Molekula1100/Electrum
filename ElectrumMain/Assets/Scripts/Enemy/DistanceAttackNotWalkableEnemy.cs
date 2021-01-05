using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DistanceAttackNotWalkableEnemy : EnemyGeneral
{
    [SerializeField] public GameObject wormBulletPref, bulletSpawnPoint;
    [SerializeField] private float bulletAddForceSpeed = 600f;  // скорость пули


    void Awake()
    {
        animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        Invoke("RemoveCol", 05f);
    }

    void RemoveCol()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Update()
    {
        //if(isActive)

        EnemyIsReadyToAtack(atackDistance, bulletSpawnPoint.transform.position, AttackRate, Damage);
    }

    public override void AtackAnimations()
    {
        animator.SetBool("idle", false);
        animator.SetBool("Atack", false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == 2)
        {
            Destroy(gameObject);
        }
    }

    public override void AtackWorm()
    {
        Vector3 direction = Direction(player.transform.position, transform.position);
        GameObject arrow = Instantiate(wormBulletPref, bulletSpawnPoint.transform.position, Quaternion.identity);
        Rigidbody2D rbArrow = arrow.GetComponent<Rigidbody2D>();
        arrow.transform.SetParent(this.gameObject.transform);
        rbArrow.AddForce(direction * bulletAddForceSpeed);
    }
}
