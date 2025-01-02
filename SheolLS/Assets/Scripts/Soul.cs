using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Soul : MonoBehaviour {
    private float speed = 2.5f;
    private Transform playerTransform;
    private void Start() {
        playerTransform = FindAnyObjectByType<Player>().transform;
    }
    private void Update() {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
    }
void OnTriggerEnter2D(Collider2D collider) {
    if (playerTransform != null) {
        if (collider.CompareTag("Player")) {
        if (playerTransform.GetComponent<Player>().vitality <= 2) {
            playerTransform.GetComponent<Player>().vitality += 1;
        }
            Destroy(gameObject);
        }
    }
}
}
