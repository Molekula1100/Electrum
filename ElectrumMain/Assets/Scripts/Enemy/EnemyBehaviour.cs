using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : EnemyGeneral
{
    private const string PATHFINDING_CONTROLLER = "PathfindingController", HEALTH_BAR = "healthbar", DEATH_PARTICLE = "DeathParticle";
    private const string IDLE_ANIM = "idle", RUN_ANIM = "Run",  ATTACK_ANIM = "Attack";
    private const string ACTIVATOR = "Activator", DIS_ACTIVATOR = "DisActivator", PURSUE_PLAYER = "PursuePlayer", ATTACKING = "Attacking";

    private const float ACTIVATING_DISTANCE = 10f;
    private const float DISACTIVATING_DISTANCE = 30f;

    private bool facingRight = true;
    private bool isSleeping = true;
    private float flipDifference;

    protected bool reachedEndOfPath = false;      
    protected bool isActive;
    protected bool IsReadyToAtack;
    protected float timer;

    private EnemyGeneral enemyGeneral;
    private EnemySpecific enemySpecific;
    private AIDestinationSetter destinationSetter;
    private AstarPath astarPath;
    private GameObject particle;

    protected AIPath aiPath;     
    protected GameObject player;  
    protected Collider2D myCollider;
    protected Animator animator;
  
    protected static Healthbar healthbar;

    private void Start()
    {
        healthbar = GameObject.Find(HEALTH_BAR).GetComponent<Healthbar>();
        astarPath = GameObject.Find(PATHFINDING_CONTROLLER).GetComponent<AstarPath>();
        particle = GameObject.Find(DEATH_PARTICLE);
        player = GameObject.Find(Player.uniqName);

        enemyGeneral = GetComponent<EnemyGeneral>();
        enemySpecific = GetComponent<EnemySpecific>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        myCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        GameManager.enemiesOnScene.Add(this.gameObject);
        destinationSetter.target = player.transform;
        aiPath.enabled = false;
        animator.SetBool(IDLE_ANIM, true);

        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), myCollider);
        StartCoroutine(ACTIVATOR);
    }

    private void Update()
    {
        print(FireRate);
        Flipping();

        if(enemyGeneral.Health <= 0)
        {
            Death();
        }
    }

    private IEnumerator Activator ()   // активирование врага, когда он заметил игрока
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
            Vector2 pos = Vector2.one;
			reachedEndOfPath = aiPath.reachedDestination;
            myCollider.enabled = true;
            aiPath.enabled = true;
            Vector2 direction = Direction(player.transform.position, pos);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, AttackDistance);
            float distance  = Vector2.Distance(player.transform.position, pos);
            if(hit.collider != null)
            {
                if(hit.transform.gameObject.name == Player.uniqName && distance <= AttackDistance || reachedEndOfPath)
                {
                    ResetValues();
                    myCollider.enabled = false;
                    IsReadyToAtack = true;
                    animator.SetBool(ATTACK_ANIM, true);
                    StartCoroutine(ATTACKING);
                } 
            }
            yield return null;
		}
    }

    IEnumerator Attacking()
    {
        while (IsReadyToAtack) 
        {	
            yield return new WaitForSeconds(1f / FireRate);

            Vector2 pos = Vector2.one;
			reachedEndOfPath = aiPath.reachedDestination;
            player = GameObject.FindGameObjectWithTag(Player.uniqName);
            Vector2 direction = Direction(player.transform.position, pos);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, AttackDistance);
            float distance  = Vector2.Distance(player.transform.position, pos);
            if(hit.collider != null)
            {
                if(hit.transform.gameObject.name == Player.uniqName && distance <= AttackDistance || reachedEndOfPath)
                {
                    AtackAnimations();
                    Atack();
                    AtackWorm();
                    Player.playerHealth -= Damage;
                }
                else
                {
                    ResetValues();
                    isActive = true;
                    aiPath.enabled = true;
                    RunAnimations();
                    StartCoroutine(PURSUE_PLAYER);
                }
            }
		}
    }

    protected bool IsReadyToRetreat()   // отступление
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find(Player.uniqName);
        if(Vector2.Distance(player.transform.position, transform.position) <= minDistance)
        {
            Vector2 direction = Direction(player.transform.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -direction, minDistance);
            if(hit.collider == null)
            {
                animator.SetBool(IDLE_ANIM, false);
                return true;
            }
        }
        return false;
    }

    public virtual void AtackAnimations()
    {
       
    }

    public virtual void RunAnimations()
    {
        
    }

    private void Flipping()
    {
        if(isActive)
        {
            flipDifference = player.transform.position.x - transform.position.x;
            if (flipDifference < 0 && !facingRight) 
            {
			    Flip();
		    } 
            else if (flipDifference > 0 && facingRight) 
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
        isActive = false;
        isSleeping = false;
        aiPath.enabled = false;
        IsReadyToAtack = false;
        animator.SetBool(IDLE_ANIM, false);
        animator.SetBool(RUN_ANIM, false);
        animator.SetBool(ATTACK_ANIM, false);
    }

    private void Death()
    {
        /*GameObject enemyParticle = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(enemyParticle, 1f);
        Destroy(gameObject);*/
    }

    public virtual void Atack()
    {   
       
    }   

    public virtual void AtackWorm()
    {
        
    }

    public Vector2 Direction(Vector2 target, Vector2 startPos) 
    {
        Vector2 difference = target - startPos;
        Vector2 direction = difference / difference.magnitude;
        return direction;
    }
}
