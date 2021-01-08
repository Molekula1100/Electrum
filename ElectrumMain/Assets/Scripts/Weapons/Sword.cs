using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{  
    Animation anim;
  
    void Start()
    {
        timeBtwStrikes =0.8f;
        damage = 2;
        anim = GetComponent<Animation>();
        attackRange = 0.9f;
        enemiesLayer = LayerMask.NameToLayer("IgnoreRaycast");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public override void Strike()
    {
       if(Time.time > lastStrikeTime + timeBtwStrikes)
       {
           lastStrikeTime = Time.time;
            anim.Play();
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemiesLayer);
            for (int i = 0; i < hitEnemies.Length; i++)
            {
                if(hitEnemies[i].GetComponent<EnemyBehaviour>() != null)
                hitEnemies[i].GetComponent<EnemyBehaviour>().Health--;
            }
        }
    }

   
}
