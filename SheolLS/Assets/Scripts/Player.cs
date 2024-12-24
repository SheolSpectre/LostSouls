using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour {
    float speed = 2f;
    float horizontal;
    bool facingRight = true;
    int attackDamage = 1;
    float attackDelay = 1f;
    float attackRange = 0.5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.linearVelocityX = horizontal * speed;
        if(horizontal < 0 && facingRight || horizontal > 0 && !facingRight) {
            facingRight =! facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        attackDelay -= Time.deltaTime;
        if(Input.GetKeyDown (KeyCode.Space) && attackDelay <= 0) {
            attackDelay = 2;
            Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            for(int i = 0; i < enemiesToAttack.Length; i++){
                var enemy = enemiesToAttack[i].GetComponent<Enemy>();
                if(enemy != null) {
                    enemy.vital  -= attackDamage;
                }
            }
            animator.SetTrigger("Attacking");
        }
    }
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}