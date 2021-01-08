using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : Weapon
{
  
    public bool isMeleeRange;
    private Transform shotPoint;
    [SerializeField]private GameObject fireball;
    [SerializeField]private Color color;

    void Start()
    {
        joystickSecond = GameObject.Find("Joystick2").GetComponent<JoystickSecond>();
        isMeleeRange = false;
        attackRange = 0.5f;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        shotPoint = transform.Find("ShotPoint").GetComponent<Transform>();
        enemiesLayer = LayerMask.NameToLayer("IgnoreRaycast");
    }


    public override void Strike()
    {   
        if(Time.time > lastStrikeTime + timeBtwStrikes)
        {
            lastStrikeTime = Time.time;
            if (!isMeleeRange)
            {
                GameObject go = Instantiate(fireball, shotPoint.position, Quaternion.Euler(0, 0, Mathf.Atan2(joystickSecond.Vertical2, joystickSecond.Horizontal2) * Mathf.Rad2Deg));
                go.GetComponent<SpriteRenderer>().color = color;
            }
            else
            {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemiesLayer);
                for (int i = 0; i < hitEnemies.Length; i++)
                {
                    if(hitEnemies[i].GetComponent<EnemyBehaviour>() != null)
                    hitEnemies[i].GetComponent<EnemyBehaviour>().Health--;
                }
            }
        }
       
    }
}
