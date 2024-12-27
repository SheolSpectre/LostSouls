using UnityEngine;

public class Human : MonoBehaviour {
    private bool isPlayerNearby = false;
    public Dialogue dialogue;
    public GameObject soul;
    void Update() {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) {
            Object.FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
            Object.FindAnyObjectByType<Player>().isTalking = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerNearby = true;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerNearby = false;
        }
    }
    public void DisappearAfterDialogue() {
        Instantiate(soul, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}