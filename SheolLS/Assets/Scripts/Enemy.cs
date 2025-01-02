using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int vitality = 1;
    private float speed = 1;
    private float visionRange = 5;
    private float attackRange = 0.3f;
    private int attackDamage = 1;
    private float attackDelay = 0;
    private Transform player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator animator;
    private enum State { Idle, Chasing, Attacking };
    private State currentState = State.Idle;
    private void Update() {
        if (vitality <= 0) {
            Destroy(gameObject);
        }
        attackDelay -= Time.deltaTime;
        Collider2D playerInVision = Physics2D.OverlapCircle(transform.position, visionRange, playerLayer);
        Collider2D playerInAttack = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
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
    private void AttackPlayer(Transform playerTransform){
        if (attackDelay <= 0f) {
            animator.SetTrigger("Attack");
            attackDelay = 1;
            playerTransform.GetComponent<Player>().vitality -= attackDamage;
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}