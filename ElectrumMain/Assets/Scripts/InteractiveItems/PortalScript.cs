using UnityEngine;

public class PortalScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 2)
        {
            transform.position = new Vector2(Random.Range((int)(transform.position.x - 8f), (int)(transform.position.x + 8f)),
            Random.Range((int)(transform.position.y - 8f), (int)(transform.position.y + 8f)));
        }
    }
}
