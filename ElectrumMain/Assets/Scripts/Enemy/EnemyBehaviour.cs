using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : EnemyGeneral
{
    private const string PATHFINDING_CONTROLLER = "PathfindingController", HEALTH_BAR = "healthbar", DEATH_PARTICLE = "EnemyDeathParticle";
    private const string IDLE_ANIM = "idle", RUN_ANIM = "Run",  ATTACK_ANIM = "Attack";
    private const string ACTIVATOR = "Activator", DIS_ACTIVATOR = "DisActivator", PURSUE_PLAYER = "PursuePlayer", ATTACKING = "Attacking";

    private const float ACTIVATING_DISTANCE = 10f;
    private const float DISACTIVATING_DISTANCE = 30f;

    private bool facingRight = true;
    private bool isSleeping = true;

    private bool reachedEndOfPath = false;      
    private bool isActive;
    private bool isReadyToAtack;
    private float attackTimer;

    private EnemyGeneral enemyGeneral;
    private AIDestinationSetter destinationSetter;
    private AstarPath astarPath;
    private GameObject particle;

    private AIPath aiPath;     
    private GameObject player;  
    private Collider2D myCollider;
    private Animator animator;
  
    protected static Healthbar healthbar;

    public delegate void AttackManager();
    public event AttackManager Attack;        

    private void Start()
    {
        healthbar = GameObject.Find(HEALTH_BAR).GetComponent<Healthbar>();
        astarPath = GameObject.Find(PATHFINDING_CONTROLLER).GetComponent<AstarPath>();
        particle = GameObject.Find(DEATH_PARTICLE);
        player = GameObject.Find(Player.uniqName);

        enemyGeneral = GetComponent<EnemyGeneral>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        myCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        GameManager.enemiesOnScene.Add(this.gameObject);
        destinationSetter.target = GameObject.Find(Player.uniqName).transform;
        aiPath.enabled = false;
        animator.SetBool(IDLE_ANIM, true);

        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), myCollider);
        StartCoroutine(ACTIVATOR);
    }

    private void Update()
    {
        Flipping();
        CheckToRetreat();

        if(Health <= 0)
        {
            Death();
        }
    }

    private IEnumerator Activator ()   
    {
		while (isSleeping) {	
			Vector2 direction = Direction(player.transform.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, ACTIVATING_DISTANCE);
            if(hit.collider != null)
            {
                if(hit.transform.gameObject.name == Player.uniqName)
                {
                    ResetValues();

                    isActive = true;
                    animator.SetBool(RUN_ANIM, true);
                    StartCoroutine(DIS_ACTIVATOR);
                    StartCoroutine(PURSUE_PLAYER);
                }
            }

            yield return isActive;
		}
	}

    private IEnumerator DisActivator()
    {
        while (!isSleeping) {	
			float distance = Vector2.Distance(player.transform.position, transform.position);
            Vector2 direction = Direction(player.transform.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
            if(hit.collider != null)
            {
                if(hit.transform.gameObject.name != Player.uniqName && distance > DISACTIVATING_DISTANCE)
                {
                    ResetValues();
                    isSleeping = true;
                    animator.SetBool(IDLE_ANIM, true);
                    StartCoroutine(ACTIVATOR);
                }
            }

            yield return null;
		}
    }

    IEnumerator PursuePlayer()
    {
        while (isActive) 
        {	
            myCollider.enabled = true;
            aiPath.enabled = true;

            reachedEndOfPath = aiPath.reachedDestination;
            Vector2 direction = Direction(player.transform.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, AttackDistance);
            float distance  = Vector2.Distance(player.transform.position, transform.position);
            if(hit.collider != null)
            {
                if(hit.transform.gameObject.name == Player.uniqName && distance <= AttackDistance || reachedEndOfPath)
                {
                    ResetValues();
                    myCollider.enabled = false;
                    isReadyToAtack = true;
                    animator.SetBool(ATTACK_ANIM, true);
                    StartCoroutine(ATTACKING);
                }
            }

            yield return null;
		}
    }

    IEnumerator Attacking()
    {
        while (isReadyToAtack) 
        {	
            attackTimer += Time.deltaTime;

            reachedEndOfPath = aiPath.reachedDestination;
            Vector2 direction = Direction(player.transform.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, AttackDistance);
            float distance  = Vector2.Distance(player.transform.position, transform.position);
            if(hit.collider != null)
            {
                if(hit.transform.gameObject.name == Player.uniqName && distance <= AttackDistance || reachedEndOfPath)
                {
                    if(attackTimer > (1f / FireRate))
                    {
                        Attack?.Invoke();
                        attackTimer = 0f;
                    }
                }
                else
                {
                    ResetValues();
                    aiPath.enabled = true;
                    isActive = true;
                    animator.SetBool(RUN_ANIM, true);
                    StartCoroutine(PURSUE_PLAYER);
                }
            }

            yield return null;
		}
    }

    private void CheckToRetreat()  
    {
        animator = GetComponent<Animator>();
        if(Vector2.Distance(player.transform.position, transform.position) <= minDistance)
        {
            Vector2 direction = Direction(player.transform.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -direction, minDistance);
            if(hit.collider == null)
            {
                animator.SetBool(IDLE_ANIM, false);
                Retreat();
            }
        }
    }

    void Retreat()
    {
        transform.Translate(-Direction(player.transform.position, transform.position)
         * Time.deltaTime * RetreatSpeed);
    }

    private void Flipping()
    {
        if(!isSleeping)
        {
            if (player.transform.position.x > transform.position.x && facingRight) 
            {
			    Flip();
		    } 
            if (player.transform.position.x < transform.position.x && !facingRight) 
            {
			    Flip();
		    }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void ResetValues()
    {
        attackTimer = 0;
        isActive = false;
        isSleeping = false;
        isReadyToAtack = false;
        aiPath.enabled = false;
        
        animator.SetBool(IDLE_ANIM, false);
        animator.SetBool(RUN_ANIM, false);
        animator.SetBool(ATTACK_ANIM, false);
    }

    private void Death()
    {
        GameObject enemyParticle = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(enemyParticle, 1f);
        Destroy(gameObject);
    }

    public static Vector2 Direction(Vector2 target, Vector2 startPos) 
    {
        Vector2 difference = target - startPos;
        Vector2 direction = difference / difference.magnitude;
        return direction;
    }
}