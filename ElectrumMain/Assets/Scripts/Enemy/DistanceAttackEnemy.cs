using UnityEngine;
using Pathfinding;

public class DistanceAttackEnemy : EnemySpecific
{

    [SerializeField] GameObject bulletPref;

    private bool isReadyToRetreat;

    [SerializeField] private float atackAddForceSpeed = 400f;  // скорость пули

    private void Awake()
    {
        RetreatSpeed = retreatSpeed;
        AttackDistance = attackDistance;
        FireRate = fireRate;
        Health = health;
        Damage = damage;
    }
    
    void Start()
    {
        aiPath = GetComponent<AIPath>();

        foreach(GameObject enemyClose in GameObject.FindGameObjectsWithTag("EnemyClose"))
        {
            Physics2D.IgnoreCollision(enemyClose.GetComponent<Collider2D>(), this.gameObject.GetComponent<Collider2D>());
        }
    }


     void Update()
    {
        isReadyToRetreat = IsReadyToRetreat();
        if(isReadyToRetreat)

        transform.Translate(-Direction(player.transform.position, transform.position) * Time.deltaTime * retreatSpeed);
    }

    public override void AtackAnimations()
    {
        animator.SetBool("idle", false);
        animator.SetBool("Atack", true);
        animator.SetBool("Run", false);
    }

    public override void RunAnimations()
    {
        animator.SetBool("idle", false);
        animator.SetBool("Atack", false);
        animator.SetBool("Run", true);
    }


    public override void Atack()
    {
        Vector3 direction = Direction(player.transform.position, transform.position);
        GameObject arrow = Instantiate(bulletPref, transform.position, Quaternion.identity);
        arrow.transform.SetParent(this.gameObject.transform);
        Rigidbody2D rbArrow = arrow.GetComponent<Rigidbody2D>();
        rbArrow.AddForce(direction * atackAddForceSpeed);
    }
}
