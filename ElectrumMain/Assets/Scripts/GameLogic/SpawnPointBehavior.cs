using System.Collections;
using UnityEngine;

public class SpawnPointBehavior : MonoBehaviour
{
    private const float MAX_OFFSET = 7f;

    private void Awake()
    {
        if(Mathf.Abs(gameObject.transform.parent.transform.position.x - transform.position.x) > MAX_OFFSET
        || Mathf.Abs(gameObject.transform.parent.transform.position.y - transform.position.y) > MAX_OFFSET)
    
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 8)

        Destroy(this.gameObject);
    }
}
