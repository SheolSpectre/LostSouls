using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Soul : MonoBehaviour {
    [SerializeField] private float speed = 1f;
    private Transform player;
    private void Start() {
        player = FindAnyObjectByType<Player>().transform;
    }
    private void Update() {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(player != null){
            if(collision.CompareTag("Player")){
                collision.GetComponent<Player>().souls += 1;
                collision.GetComponent<Player>().vital += 1;
                Destroy(gameObject);
            }
        }
    }
}
