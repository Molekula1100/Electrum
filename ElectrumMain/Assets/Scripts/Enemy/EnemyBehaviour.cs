using System.Collections;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : MonoBehaviour
{
    private const float ACTIVATING_DISTANCE = 10f;
    private const float DISACTIVATING_DISTANCE = 30f;

    private bool facingRight = true;
    private bool isSleeping = true;
    private float flipDifference;

    protected bool reachedEndOfPath = false;      
    protected bool isActive;
    public bool IsReadyToAtack;
    protected float timer;

    private EnemyGeneral enemyGeneral;
    private AIDestinationSetter destinationSetter;
    private AstarPath astarPath;
    private GameObject particle;

    protected AIPath aiPath;     
    protected GameObject player;  
    protected Collider2D myCollider;
    protected Animator animator;
  
    protected static Healthbar healthbar;

    void Awake()
    {
        healthbar = GameObject.Find("healthbar").GetComponent<Healthbar>();
        GameManager.enemiesOnScene.Add(this.gameObject);
        enemyGeneral = GetComponent<EnemyGeneral>();
        aiPath = GetComponent<AIPath>();
        aiPath.enabled = false;
        astarPath = GameObject.Find("PathfindingController").GetComponent<AstarPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        myCollider = GetComponent<Collider2D>();

        player = GameObject.Find(Player.uniqName);
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), myCollider);
        StartCoroutine("Activator");
        destinationSetter.target = player.transform;

        Invoke("FindEnemys", 1f);
        animator = GetComponent<Animator>();
        animator.SetBool("idle", true);
        particle = GameObject.Find("EnemyDeathParticle");
    }


    void Update()
    {
        //print(enemysReadyToAtack);
        Flipping();

        if(enemyGeneral.Health <= 0)
        {
            Dead();
        }
    }



    IEnumerator Activator ()   // активирование врага, когда он заметил плеера
    {
		while (isSleeping) {	
			Vector2 direction = Direction(player.transform.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, ACTIVATING_DISTANCE);
            if(hit.collider != null)
            {
                //Debug.LogWarning(hit.transform.gameObject.name);
                if(hit.transform.gameObject.name == Player.uniqName)
                {
                    StartCoroutine("DisActivator");
                    GetComponent<Animator>().SetBool("idle", false);
                    GetComponent<Animator>().SetBool("Run", true);
                    isSleeping = false;
                    isActive = true;
                }
            }

            yield return isActive;
		}
	}



    IEnumerator DisActivator()
    {
        while (!isSleeping) {	
			float distance = Vector2.Distance(player.transform.position, transform.position);
            Vector2 direction = Direction(player.transform.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
            if(hit.collider != null)
            {
                if(hit.transform.gameObject.name != Player.uniqName && distance > DISACTIVATING_DISTANCE)
                {
                    DisActivate();
                    StartCoroutine("Activator");
                }
            }

            yield return null;
		}
    }



    protected bool IsReadyToRetreat(float minDistance)   // отступление
    {
        player = GameObject.Find(Player.uniqName);
        if(Vector2.Distance(player.transform.position, transform.position) <= minDistance)
        {
            Vector2 direction = Direction(player.transform.position, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -direction, minDistance);
            if(hit.collider == null)
            {
                animator.SetBool("idle", false);
                return true;
            }
        }

        else if(Vector2.Distance(player.transform.position, transform.position) <= (minDistance * 4))
        {
            return false;
        }

        return false;
    }


    protected void EnemyIsActive(float atackDistance, Vector2 pos)
    {

        reachedEndOfPath = aiPath.reachedDestination;

        myCollider.enabled = true;
        aiPath.enabled = true;

        Vector2 direction = Direction(player.transform.position, pos);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, atackDistance);
        float distance  = Vector2.Distance(player.transform.position, pos);
        if(hit.collider != null)
        {
           // print(hit.transform.gameObject.name);
            if(hit.transform.gameObject.name == Player.uniqName && distance <= atackDistance || reachedEndOfPath)
            {
                IsReadyToAtack = true;
                isActive = false;
                aiPath.enabled = false;
                animator.SetBool("idle", false);
                animator.SetBool("Run", false);
                animator.SetBool("Atack", true);

                myCollider.enabled = false;
            } 
        }
    }


    protected void EnemyIsReadyToAtack(float atackDistance, Vector2 pos, float fireRate, float damage)
    {
        reachedEndOfPath = aiPath.reachedDestination;

        timer += Time.deltaTime;

        player = GameObject.FindGameObjectWithTag(Player.uniqName);
        Vector2 direction = Direction(player.transform.position, pos);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, atackDistance);
        float distance  = Vector2.Distance(player.transform.position, pos);
        if(hit.collider != null)
        {
            if(hit.transform.gameObject.name == Player.uniqName && distance <= atackDistance || reachedEndOfPath)
            {
                if(timer >= (1f / fireRate))
                {
                    AtackAnimations();
                    Atack();
                    AtackWorm();
                    timer = 0f;
                    Player.playerHealth -= damage;
                    healthbar.Set(Player.playerHealth);
                }
            }
            else
            {
                timer = 0f;
                isActive = true;
                IsReadyToAtack = false;
                aiPath.enabled = true;
                RunAnimations();

            }
        }
    }

    public virtual void AtackAnimations()
    {
       
    }

    public virtual void RunAnimations()
    {
        
    }


    void Flipping()
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


    void Flip()
    {
        facingRight = !facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }



    void DisActivate()
    {
        isActive = false;
        isSleeping = true;
        aiPath.enabled = false;
        IsReadyToAtack = false;
        animator.SetBool("Run", false);
        animator.SetBool("Atack", false);
        animator.SetBool("idle", true);
    }

    void Dead()
    {
        GameObject enemyParticle = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(enemyParticle, 1f);
        Destroy(gameObject);
    }

    public virtual void Atack()
    {   
       // print("atack");
    }   

    public virtual void AtackWorm()
    {
        //print("wormAtack");
    }

    public Vector2 Direction(Vector2 vectorTarget, Vector2 vectorStart) 
    {
        Vector2 difference = vectorTarget - vectorStart;
        float distance = difference.magnitude;
        Vector2 direction = difference / distance;
        return direction;
    }
}
