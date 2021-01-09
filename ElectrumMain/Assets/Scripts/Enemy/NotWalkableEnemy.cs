using UnityEngine;

public class NotWalkableEnemy : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 600f; 

    [SerializeField] private GameObject bulletPref, bulletSpawnPoint;
    private GameObject player;
    private EnemyBehaviour enemyBehaviour;

    private void Start()
    {
        player = GameObject.Find(Player.uniqName);
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        enemyBehaviour.Attack += Attack;
        Invoke("RemoveCollider", 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == 2)
        {
            Destroy(gameObject);
        }
    }

    private void RemoveCollider()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void Attack()
    {
        Vector3 direction = enemyBehaviour.DirectionToPlayer();
        GameObject arrow = Instantiate(bulletPref, bulletSpawnPoint.transform.position, Quaternion.identity);
        Rigidbody2D rbArrow = arrow.GetComponent<Rigidbody2D>();
        arrow.transform.SetParent(this.gameObject.transform);
        rbArrow.AddForce(direction * bulletSpeed);
    }
}
