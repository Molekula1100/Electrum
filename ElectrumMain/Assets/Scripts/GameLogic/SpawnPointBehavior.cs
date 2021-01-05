using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointBehavior : MonoBehaviour
{
    void Awake()
    {
        if(Mathf.Abs(gameObject.transform.parent.transform.position.x - transform.position.x) > 7f
        || Mathf.Abs(gameObject.transform.parent.transform.position.y - transform.position.y) > 7f)
    
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 8)

        Destroy(gameObject);
    }
}
