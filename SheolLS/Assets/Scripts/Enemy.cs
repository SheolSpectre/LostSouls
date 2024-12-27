using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int vital = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private float visionRange = 5;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 1;
    private float attackTimer = 0f;
    [SerializeField] private LayerMask playerLayer;
    private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPos;
    private enum State { Idle, Chasing, Attacking };
    private State currentState = State.Idle;
    private void Update() {
        if (vital <= 0) {
            Destroy(gameObject);
        }
        attackTimer -= Time.deltaTime;
        Collider2D playerInVision = Physics2D.OverlapCircle(transform.position, visionRange, playerLayer);
        Collider2D playerInAttack = Physics2D.OverlapCircle(attackPos.position, attackRange, playerLayer);
        if (playerInAttack != null) {
            currentState = State.Attacking;
        } else if (playerInVision != null) {
            currentState = State.Chasing;
        } else {
            currentState = State.Idle;
        }
        switch (currentState) {
            case State.Idle:
            animator.SetTrigger("Idle");
            break;
            case State.Chasing:
            ChasePlayer(playerInVision.transform);
            break;
            case State.Attacking:
            AttackPlayer(playerInAttack.transform);
            break;
        }
    }
    private void ChasePlayer(Transform playerTransform) {
        animator.SetTrigger("Walk");
        player = playerTransform;
        Vector2 direction = (player.position - transform.position).normalized;
        if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0) {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
    private async void AttackPlayer(Transform playerTransform){
        if (attackTimer <= 0f) {
            animator.SetTrigger("Attack");
            attackTimer = attackCooldown;
            playerTransform.GetComponent<Player>().vital -= attackDamage;
        }
        await Task.Delay(1000);
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}