using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    private GameObject player; 

    void Start()
    {
        player = GameObject.Find(Player.uniqName);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        int damage = 0;
        switch (player.GetComponent<Player>().weaponType)
        {
            case WeaponTypes.Wand:
                damage = player.GetComponent<Player>().equipedweapon.GetComponent<Wand>().damage;
                break;
            case WeaponTypes.Staff:
                damage = player.GetComponent<Player>().equipedweapon.GetComponent<Staff>().damage;
                break;
            case WeaponTypes.Sword:
                damage = player.GetComponent<Player>().equipedweapon.GetComponent<Sword>().damage;
                break;
        }
        if(col.GetComponent<DistanceAttackEnemy>() != null){
            col.GetComponent<DistanceAttackEnemy>().Health -= damage;
        }
        else if(col.GetComponent<MeleeAttackEnemy>() != null){
            col.GetComponent<MeleeAttackEnemy>().Health -= damage;
        }
        else if(col.GetComponent<DistanceAttackNotWalkableEnemy>() != null){
            col.GetComponent<DistanceAttackNotWalkableEnemy>().Health -= damage;
        }
        else if(col.gameObject != player && col.gameObject.name != "ShotPoint" && col.gameObject.transform.parent != null &&
        col.gameObject.transform.parent.gameObject != player && col.gameObject.tag != "enemyBullet" && col.gameObject.tag != "point"
         && col.gameObject.tag != "Staff")//?
         
        {         
                print(col.gameObject.name);           
            DestroySelf();
        }
        
    }

    public virtual void DestroySelf()
    {

    }
}
