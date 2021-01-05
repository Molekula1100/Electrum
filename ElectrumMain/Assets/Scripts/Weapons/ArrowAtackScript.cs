using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAtackScript : MonoBehaviour
{
    Vector2 startPos;

    void Awake()
    {
        startPos = transform.position;
    }
    void Update()
    {
        if(transform.parent.gameObject.tag == "Enemy")
        {
            if(Vector2.Distance(transform.parent.gameObject.transform.position, this.gameObject.transform.position) 
            > transform.parent.gameObject.GetComponent<DistanceAttackEnemy>().atackDistance)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if(Vector2.Distance(transform.parent.gameObject.transform.position, transform.position) 
            > transform.parent.gameObject.GetComponent<DistanceAttackNotWalkableEnemy>().atackDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject != null)
        {
            if(col.gameObject.layer != 2)
            {
                Destroy(gameObject);
            }
            if(col.gameObject.layer == 8
            && Vector2.Distance(startPos, transform.position) < 0.2f)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
