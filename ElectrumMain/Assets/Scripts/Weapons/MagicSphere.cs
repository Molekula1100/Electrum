using UnityEngine;

public class MagicSphere : Spell
{
    private float speed = 2.5f;

    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
    }

    public override void DestroySelf()
    {
        Destroy(gameObject);
    }
}
