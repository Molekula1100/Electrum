using UnityEngine;

public class DistanceAttackEnemy : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 400f;

    [SerializeField] private GameObject bulletPref;
    private GameObject player;
    private EnemyBehaviour enemyBehaviour;
    
    private void Start()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        player = GameObject.Find(Player.uniqName);
        enemyBehaviour.Attack += Attack;

        foreach(GameObject enemyClose in GameObject.FindGameObjectsWithTag("EnemyClose"))
        {
            Physics2D.IgnoreCollision(enemyClose.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
        }
    }

    public void Attack()
    {
        Vector3 direction = enemyBehaviour.DirectionToPlayer();
        GameObject arrow = Instantiate(bulletPref, transform.position, Quaternion.identity);
        arrow.transform.SetParent(this.gameObject.transform);
        Rigidbody2D rbArrow = arrow.GetComponent<Rigidbody2D>();
        rbArrow.AddForce(direction * bulletSpeed);
    }
}
