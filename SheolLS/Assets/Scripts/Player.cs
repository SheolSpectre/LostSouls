using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour {
    private float speed = 2;
    private float horizontal;
    private bool facingRight = true;
    private bool attacking = false;
    private int attackDamage = 1;
    private float attackDelay = 1;
    private float attackRange = 0.5f;
    public int vitality = 3;
    public bool isTalking = false;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    private async void Update() {
        if (!attacking) {
            horizontal = Input.GetAxisRaw("Horizontal");
            rb.linearVelocityX = horizontal * speed;
        }
        if (horizontal < 0 && facingRight || horizontal > 0 && !facingRight) {
            facingRight =! facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        attackDelay -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && attackDelay <= 0) {
            attackDelay = 1;
            rb.linearVelocityX = 0;
            animator.SetTrigger("Attacking");
            attacking = true;
            Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            for (int i = 0; i < enemiesToAttack.Length; i++) {
                var enemy = enemiesToAttack[i].GetComponent<Enemy>();
                if (enemy != null) {
                    enemy.vitality -= attackDamage;
                }
            }
            await Task.Delay(500);
            attacking = false;
        }
        if (vitality <= 0) {
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}