using System.Collections;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    private const string IGNORE_RAYCAST_LAYER = "Ignore Raycast";

    private Vector2 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if(Vector2.Distance(transform.parent.gameObject.transform.position, this.gameObject.transform.position) >
            transform.parent.gameObject.GetComponent<EnemyBehaviour>().AttackDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject != null)
        {
            if(col.gameObject.layer != LayerMask.NameToLayer(IGNORE_RAYCAST_LAYER))
            {
                Destroy(gameObject);
            }
        }
    }
}
