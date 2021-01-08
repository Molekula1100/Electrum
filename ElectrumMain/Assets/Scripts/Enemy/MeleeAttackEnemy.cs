using UnityEngine;

public class MeleeAttackEnemy : MonoBehaviour
{
    private bool isReadyToRetreat;

    private EnemyBehaviour enemyBehaviour;

    private void Start()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        enemyBehaviour.Attack += Attack;
    }

    public void Attack()
    {
        Player.playerHealth -= enemyBehaviour.Damage;
    }
}